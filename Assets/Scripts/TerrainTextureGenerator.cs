using UnityEngine;

namespace VintageBeef.World
{
    /// <summary>
    /// Generates and manages textures for the terrain
    /// Creates stylized Palia-like textures procedurally
    /// </summary>
    public class TerrainTextureGenerator : MonoBehaviour
    {
        [Header("Texture Settings")]
        [SerializeField] private int textureSize = 256;
        [SerializeField] private float noiseScale = 0.1f;

        // Constants for texture generation
        private const float GrassBladeChance = 0.05f;

        /// <summary>
        /// Creates a material with a terrain texture for the given biome
        /// </summary>
        public Material CreateBiomeMaterial(BiomeType biome)
        {
            Material mat = new Material(Shader.Find("Standard"));
            
            // Create texture for the biome
            Texture2D texture = CreateBiomeTexture(biome);
            mat.mainTexture = texture;
            
            // Set material properties for Palia-style look
            mat.SetFloat("_Glossiness", 0.1f); // Low glossiness
            mat.SetFloat("_Metallic", 0.0f);    // No metallic
            
            return mat;
        }

        /// <summary>
        /// Creates a procedural texture for a biome type
        /// </summary>
        private Texture2D CreateBiomeTexture(BiomeType biome)
        {
            Texture2D texture = new Texture2D(textureSize, textureSize);
            Color baseColor = GetBiomeBaseColor(biome);
            Color accentColor = GetBiomeAccentColor(biome);

            for (int y = 0; y < textureSize; y++)
            {
                for (int x = 0; x < textureSize; x++)
                {
                    // Create organic texture using Perlin noise
                    float noise = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                    
                    // Add second octave for detail
                    float detailNoise = Mathf.PerlinNoise(x * noiseScale * 2f, y * noiseScale * 2f) * 0.3f;
                    noise = noise * 0.7f + detailNoise;

                    // Blend between base and accent color based on noise
                    Color pixelColor = Color.Lerp(baseColor, accentColor, noise);
                    texture.SetPixel(x, y, pixelColor);
                }
            }

            texture.Apply();
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Bilinear;

            return texture;
        }

        /// <summary>
        /// Gets the base color for a biome
        /// </summary>
        private Color GetBiomeBaseColor(BiomeType biome)
        {
            switch (biome)
            {
                case BiomeType.Forest:
                    return new Color(0.25f, 0.55f, 0.25f); // Rich green
                
                case BiomeType.Plains:
                    return new Color(0.55f, 0.70f, 0.35f); // Grassy green
                
                case BiomeType.Desert:
                    return new Color(0.88f, 0.75f, 0.50f); // Sandy yellow
                
                case BiomeType.Mountains:
                    return new Color(0.50f, 0.50f, 0.50f); // Stone gray
                
                default:
                    return Color.green;
            }
        }

        /// <summary>
        /// Gets the accent color for a biome (for texture variation)
        /// </summary>
        private Color GetBiomeAccentColor(BiomeType biome)
        {
            switch (biome)
            {
                case BiomeType.Forest:
                    return new Color(0.30f, 0.50f, 0.30f); // Darker green
                
                case BiomeType.Plains:
                    return new Color(0.60f, 0.75f, 0.40f); // Lighter grass
                
                case BiomeType.Desert:
                    return new Color(0.85f, 0.70f, 0.45f); // Darker sand
                
                case BiomeType.Mountains:
                    return new Color(0.60f, 0.60f, 0.60f); // Lighter stone
                
                default:
                    return Color.white;
            }
        }

        /// <summary>
        /// Creates a simple dirt/ground texture
        /// </summary>
        public Texture2D CreateGroundTexture()
        {
            Texture2D texture = new Texture2D(textureSize, textureSize);
            Color dirtColor = new Color(0.45f, 0.35f, 0.25f);
            Color dirtVariation = new Color(0.40f, 0.30f, 0.20f);

            for (int y = 0; y < textureSize; y++)
            {
                for (int x = 0; x < textureSize; x++)
                {
                    float noise = Mathf.PerlinNoise(x * noiseScale * 1.5f, y * noiseScale * 1.5f);
                    Color pixelColor = Color.Lerp(dirtColor, dirtVariation, noise);
                    texture.SetPixel(x, y, pixelColor);
                }
            }

            texture.Apply();
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Bilinear;

            return texture;
        }

        /// <summary>
        /// Creates a simple grass texture
        /// </summary>
        public Texture2D CreateGrassTexture()
        {
            Texture2D texture = new Texture2D(textureSize, textureSize);
            Color grassColor = new Color(0.40f, 0.65f, 0.30f);
            Color grassVariation = new Color(0.35f, 0.60f, 0.28f);

            for (int y = 0; y < textureSize; y++)
            {
                for (int x = 0; x < textureSize; x++)
                {
                    float noise = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                    
                    // Add grass blade detail
                    if (Random.value < GrassBladeChance)
                    {
                        noise = Mathf.Clamp01(noise + 0.15f);
                    }

                    Color pixelColor = Color.Lerp(grassColor, grassVariation, noise);
                    texture.SetPixel(x, y, pixelColor);
                }
            }

            texture.Apply();
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Bilinear;

            return texture;
        }
    }
}
