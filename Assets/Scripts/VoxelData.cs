using UnityEngine;

namespace VintageBeef.Voxel
{
    /// <summary>
    /// Defines types of voxel blocks in the world
    /// </summary>
    public enum VoxelType : byte
    {
        Air = 0,
        Dirt = 1,
        Grass = 2,
        Stone = 3,
        Sand = 4,
        Gravel = 5,
        Clay = 6,
        Ore_Iron = 7,
        Ore_Copper = 8,
        Snow = 9,
        Ice = 10,
        Water = 11,
        Lava = 12
    }

    /// <summary>
    /// Represents a single voxel with its properties
    /// </summary>
    [System.Serializable]
    public struct Voxel
    {
        public VoxelType type;
        public byte density; // 0-255, used for smooth terrain (Marching Cubes)

        public Voxel(VoxelType type, byte density = 255)
        {
            this.type = type;
            this.density = density;
        }

        public bool IsEmpty()
        {
            return type == VoxelType.Air || density < 128;
        }

        public bool IsSolid()
        {
            return !IsEmpty();
        }
    }

    /// <summary>
    /// Properties and appearance of each voxel type
    /// </summary>
    [System.Serializable]
    public class VoxelTypeData
    {
        public VoxelType type;
        public string displayName;
        public Color color;
        public bool isSolid;
        public bool isTransparent;
        public float hardness; // Mining difficulty

        public VoxelTypeData(VoxelType type, string name, Color color, bool solid = true, bool transparent = false, float hardness = 1f)
        {
            this.type = type;
            this.displayName = name;
            this.color = color;
            this.isSolid = solid;
            this.isTransparent = transparent;
            this.hardness = hardness;
        }
    }

    /// <summary>
    /// Static database of all voxel types
    /// </summary>
    public static class VoxelDatabase
    {
        private static VoxelTypeData[] voxelTypes;

        static VoxelDatabase()
        {
            InitializeVoxelTypes();
        }

        private static void InitializeVoxelTypes()
        {
            voxelTypes = new VoxelTypeData[]
            {
                new VoxelTypeData(VoxelType.Air, "Air", Color.clear, false, true, 0f),
                new VoxelTypeData(VoxelType.Dirt, "Dirt", new Color(0.45f, 0.35f, 0.25f), true, false, 1f),
                new VoxelTypeData(VoxelType.Grass, "Grass", new Color(0.4f, 0.65f, 0.3f), true, false, 1f),
                new VoxelTypeData(VoxelType.Stone, "Stone", new Color(0.5f, 0.5f, 0.5f), true, false, 3f),
                new VoxelTypeData(VoxelType.Sand, "Sand", new Color(0.88f, 0.75f, 0.50f), true, false, 0.8f),
                new VoxelTypeData(VoxelType.Gravel, "Gravel", new Color(0.55f, 0.55f, 0.55f), true, false, 1.2f),
                new VoxelTypeData(VoxelType.Clay, "Clay", new Color(0.7f, 0.65f, 0.6f), true, false, 1.5f),
                new VoxelTypeData(VoxelType.Ore_Iron, "Iron Ore", new Color(0.6f, 0.5f, 0.45f), true, false, 4f),
                new VoxelTypeData(VoxelType.Ore_Copper, "Copper Ore", new Color(0.7f, 0.5f, 0.3f), true, false, 3.5f),
                new VoxelTypeData(VoxelType.Snow, "Snow", new Color(0.95f, 0.95f, 0.98f), true, false, 0.5f),
                new VoxelTypeData(VoxelType.Ice, "Ice", new Color(0.7f, 0.85f, 0.95f), true, true, 2f),
                new VoxelTypeData(VoxelType.Water, "Water", new Color(0.2f, 0.4f, 0.8f, 0.5f), false, true, 0f),
                new VoxelTypeData(VoxelType.Lava, "Lava", new Color(1f, 0.3f, 0f, 0.8f), false, true, 0f),
            };
        }

        public static VoxelTypeData GetVoxelData(VoxelType type)
        {
            int index = (int)type;
            if (index >= 0 && index < voxelTypes.Length)
            {
                return voxelTypes[index];
            }
            return voxelTypes[0]; // Return Air as default
        }

        public static Color GetVoxelColor(VoxelType type)
        {
            return GetVoxelData(type).color;
        }

        public static bool IsSolid(VoxelType type)
        {
            return GetVoxelData(type).isSolid;
        }

        public static bool IsTransparent(VoxelType type)
        {
            return GetVoxelData(type).isTransparent;
        }
    }
}
