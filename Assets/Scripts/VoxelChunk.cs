using UnityEngine;
using System.Collections.Generic;

namespace VintageBeef.Voxel
{
    /// <summary>
    /// Represents a chunk of voxel terrain
    /// Handles voxel data storage and mesh generation
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class VoxelChunk : MonoBehaviour
    {
        public Vector3Int chunkPosition; // Position in chunk coordinates
        public int chunkSize = 16; // Size of chunk in voxels (16x16x16)

        private Voxel[,,] voxels; // 3D array of voxels
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private MeshCollider meshCollider;
        private bool isDirty = false;

        private static Material sharedMaterial; // Shared material for all chunks

        public void Initialize(Vector3Int position, int size)
        {
            chunkPosition = position;
            chunkSize = size;
            voxels = new Voxel[chunkSize, chunkSize, chunkSize];

            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();

            // Use shared material to reduce memory usage
            if (sharedMaterial == null)
            {
                sharedMaterial = new Material(Shader.Find("Standard"));
                sharedMaterial.SetFloat("_Glossiness", 0.1f);
            }
            meshRenderer.material = sharedMaterial;
        }

        /// <summary>
        /// Set a voxel at local coordinates within the chunk
        /// </summary>
        public void SetVoxel(int x, int y, int z, Voxel voxel)
        {
            if (IsValidPosition(x, y, z))
            {
                voxels[x, y, z] = voxel;
                isDirty = true;
            }
        }

        /// <summary>
        /// Get a voxel at local coordinates within the chunk
        /// </summary>
        public Voxel GetVoxel(int x, int y, int z)
        {
            if (IsValidPosition(x, y, z))
            {
                return voxels[x, y, z];
            }
            return new Voxel(VoxelType.Air, 0);
        }

        /// <summary>
        /// Check if position is within chunk bounds
        /// </summary>
        private bool IsValidPosition(int x, int y, int z)
        {
            return x >= 0 && x < chunkSize && 
                   y >= 0 && y < chunkSize && 
                   z >= 0 && z < chunkSize;
        }

        /// <summary>
        /// Generate the mesh for this chunk using greedy meshing
        /// </summary>
        public void GenerateMesh()
        {
            if (!isDirty) return;

            Mesh mesh = new Mesh();
            mesh.name = $"Chunk_{chunkPosition.x}_{chunkPosition.y}_{chunkPosition.z}";

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Color> colors = new List<Color>();
            List<Vector3> normals = new List<Vector3>();

            // Generate mesh using greedy meshing algorithm
            GreedyMesh(vertices, triangles, colors, normals);

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetColors(colors);
            mesh.SetNormals(normals);

            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;

            isDirty = false;
        }

        /// <summary>
        /// Greedy meshing algorithm for efficient voxel rendering
        /// Combines adjacent faces of same type to reduce vertex count
        /// </summary>
        private void GreedyMesh(List<Vector3> vertices, List<int> triangles, List<Color> colors, List<Vector3> normals)
        {
            // Process each axis (X, Y, Z)
            for (int axis = 0; axis < 3; axis++)
            {
                int u = (axis + 1) % 3;
                int v = (axis + 2) % 3;

                int[] dimensions = new int[3];
                dimensions[axis] = chunkSize;
                dimensions[u] = chunkSize;
                dimensions[v] = chunkSize;

                bool[,] mask = new bool[chunkSize, chunkSize];
                VoxelType[,] typeMask = new VoxelType[chunkSize, chunkSize];

                // Process each slice along the axis
                for (int d = 0; d < dimensions[axis]; d++)
                {
                    // Build mask for this slice
                    BuildSliceMask(axis, d, mask, typeMask);

                    // Generate quads from mask
                    GenerateQuadsFromMask(axis, d, mask, typeMask, vertices, triangles, colors, normals);
                }
            }
        }

        /// <summary>
        /// Build a mask of visible faces for a slice
        /// </summary>
        private void BuildSliceMask(int axis, int depth, bool[,] mask, VoxelType[,] typeMask)
        {
            for (int i = 0; i < chunkSize; i++)
            {
                for (int j = 0; j < chunkSize; j++)
                {
                    int[] pos = new int[3];
                    pos[axis] = depth;
                    pos[(axis + 1) % 3] = i;
                    pos[(axis + 2) % 3] = j;

                    Voxel current = GetVoxel(pos[0], pos[1], pos[2]);
                    
                    // Check if we need to render a face
                    pos[axis] = depth - 1;
                    Voxel neighbor = pos[axis] >= 0 ? GetVoxel(pos[0], pos[1], pos[2]) : new Voxel(VoxelType.Air, 0);

                    bool renderFace = current.IsSolid() && neighbor.IsEmpty();

                    mask[i, j] = renderFace;
                    typeMask[i, j] = renderFace ? current.type : VoxelType.Air;
                }
            }
        }

        /// <summary>
        /// Generate quad meshes from the face mask
        /// </summary>
        private void GenerateQuadsFromMask(int axis, int depth, bool[,] mask, VoxelType[,] typeMask, 
                                          List<Vector3> vertices, List<int> triangles, List<Color> colors, List<Vector3> normals)
        {
            for (int i = 0; i < chunkSize; i++)
            {
                for (int j = 0; j < chunkSize; )
                {
                    if (!mask[i, j])
                    {
                        j++;
                        continue;
                    }

                    VoxelType type = typeMask[i, j];

                    // Find width of quad
                    int width = 1;
                    while (j + width < chunkSize && mask[i, j + width] && typeMask[i, j + width] == type)
                    {
                        width++;
                    }

                    // Find height of quad
                    int height = 1;
                    bool done = false;
                    while (i + height < chunkSize && !done)
                    {
                        for (int k = 0; k < width; k++)
                        {
                            if (!mask[i + height, j + k] || typeMask[i + height, j + k] != type)
                            {
                                done = true;
                                break;
                            }
                        }
                        if (!done) height++;
                    }

                    // Create quad
                    AddQuad(axis, depth, i, j, width, height, type, vertices, triangles, colors, normals);

                    // Clear mask for processed area
                    for (int h = 0; h < height; h++)
                    {
                        for (int w = 0; w < width; w++)
                        {
                            mask[i + h, j + w] = false;
                        }
                    }

                    j += width;
                }
            }
        }

        /// <summary>
        /// Add a quad to the mesh
        /// </summary>
        private void AddQuad(int axis, int depth, int i, int j, int width, int height, VoxelType type,
                           List<Vector3> vertices, List<int> triangles, List<Color> colors, List<Vector3> normals)
        {
            int vertexIndex = vertices.Count;

            Vector3[] quadVertices = new Vector3[4];
            Vector3 normal = Vector3.zero;

            // Calculate vertices based on axis
            if (axis == 0) // X axis
            {
                quadVertices[0] = new Vector3(depth, i, j);
                quadVertices[1] = new Vector3(depth, i + height, j);
                quadVertices[2] = new Vector3(depth, i + height, j + width);
                quadVertices[3] = new Vector3(depth, i, j + width);
                normal = Vector3.left;
            }
            else if (axis == 1) // Y axis
            {
                quadVertices[0] = new Vector3(i, depth, j);
                quadVertices[1] = new Vector3(i, depth, j + width);
                quadVertices[2] = new Vector3(i + height, depth, j + width);
                quadVertices[3] = new Vector3(i + height, depth, j);
                normal = Vector3.down;
            }
            else // Z axis
            {
                quadVertices[0] = new Vector3(i, j, depth);
                quadVertices[1] = new Vector3(i, j + width, depth);
                quadVertices[2] = new Vector3(i + height, j + width, depth);
                quadVertices[3] = new Vector3(i + height, j, depth);
                normal = Vector3.back;
            }

            // Add vertices
            for (int v = 0; v < 4; v++)
            {
                vertices.Add(quadVertices[v]);
                colors.Add(VoxelDatabase.GetVoxelColor(type));
                normals.Add(normal);
            }

            // Add triangles
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
        }

        /// <summary>
        /// Mark chunk as needing mesh regeneration
        /// </summary>
        public void MarkDirty()
        {
            isDirty = true;
        }

        /// <summary>
        /// Fill the chunk with voxel data
        /// </summary>
        public void FillVoxelData(Voxel[,,] data)
        {
            if (data.GetLength(0) == chunkSize && 
                data.GetLength(1) == chunkSize && 
                data.GetLength(2) == chunkSize)
            {
                voxels = data;
                isDirty = true;
            }
        }
    }
}
