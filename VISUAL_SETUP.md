# Visual Improvements for Terrain

## Recommended Unity Settings for Best Visual Quality

### Lighting Settings (GameWorld Scene)

1. **Open Lighting Settings**
   - Window → Rendering → Lighting
   
2. **Environment**
   - **Skybox Material:** Use "Default-Skybox" or create custom
   - **Sun Source:** Create Directional Light if not present
   - **Environment Lighting:**
     - Source: Skybox
     - Intensity Multiplier: 1.0
     - Ambient Mode: Skybox
   
3. **Realtime Lighting**
   - Realtime Global Illumination: Checked (for dynamic lighting)
   
4. **Baked Lighting** (Optional for better performance)
   - Baked Global Illumination: Checked
   - Lightmap Resolution: 20-40 (lower for better performance)

### Directional Light Settings (Sun)

Create or modify the Directional Light:
- **Position:** (0, 50, 0)
- **Rotation:** (50, -30, 0) for pleasant angle
- **Color:** Slightly warm white (255, 250, 240)
- **Intensity:** 1.0-1.5
- **Shadow Type:** Soft Shadows
- **Shadow Resolution:** Medium Shadows

### Camera Settings

On the Main Camera (Player Camera):
- **Clear Flags:** Skybox
- **Background:** If using Solid Color, use light blue (135, 206, 235)
- **Culling Mask:** Everything
- **Depth:** 0
- **Field of View:** 60-75 (70 recommended)
- **Clipping Planes:**
  - Near: 0.3
  - Far: 1000

### Post-Processing (Optional Enhancement)

For even better visuals:

1. Install Post Processing package (if not already installed)
2. Create Post-Processing Profile
3. Add effects:
   - **Ambient Occlusion:** Subtle depth
   - **Bloom:** Very subtle for bright areas
   - **Color Grading:** Warm tone adjustment
   - **Vignette:** Subtle edge darkening

### Quality Settings

Edit → Project Settings → Quality

For target hardware (Palia-style):
- **Quality Level:** Medium or High
- **Pixel Light Count:** 2-4
- **Texture Quality:** Full Res
- **Anisotropic Textures:** Per Texture
- **Anti Aliasing:** 2x Multi Sampling (or FXAA in camera)
- **Soft Particles:** Checked
- **Shadows:**
  - Shadow Type: Hard and Soft Shadows
  - Shadow Resolution: Medium Resolution
  - Shadow Distance: 50-100 (adjust for performance)
  - Shadow Cascades: Two Cascades

## Quick Visual Setup Script

Add this to your GameWorld scene setup:

```csharp
// In TerrainManager or a new LightingSetup script
void SetupSceneLighting()
{
    // Find or create directional light
    Light sunLight = GameObject.FindObjectOfType<Light>();
    if (sunLight == null)
    {
        GameObject lightObj = new GameObject("Directional Light");
        sunLight = lightObj.AddComponent<Light>();
        sunLight.type = LightType.Directional;
    }
    
    // Configure sun
    sunLight.transform.rotation = Quaternion.Euler(50, -30, 0);
    sunLight.color = new Color(1f, 0.98f, 0.94f); // Warm white
    sunLight.intensity = 1.2f;
    sunLight.shadows = LightShadows.Soft;
    
    // Set ambient lighting
    RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
    RenderSettings.ambientIntensity = 1.0f;
}
```

## Fog Settings (Optional Atmospheric Effect)

Window → Rendering → Lighting → Environment

- **Fog:** Checked
- **Color:** Light blue-gray (180, 200, 220)
- **Mode:** Exponential Squared
- **Density:** 0.001-0.005 (very subtle)

This creates atmospheric depth without overwhelming the scene.

## Terrain Visual Enhancements

### In ProceduralWorldGenerator:

Adjust these values for different looks:
- **Height Multiplier:** 15-20 for more dramatic terrain
- **Noise Scale:** 0.03 for larger features, 0.08 for more detail
- **Biome Scale:** 0.015 for larger biomes, 0.03 for smaller patches

### Texture Quality:

In TerrainTextureGenerator:
- **Texture Size:** 512 for higher quality (at cost of memory)
- **Noise Scale:** 0.05 for finer details, 0.2 for coarser

## Recommended Scene Hierarchy

```
GameWorld Scene
├── TerrainSystem (TerrainManager)
├── Environment
│   ├── Directional Light (Sun)
│   └── Sky (optional skybox GameObject)
├── GameManager
├── NetworkManager
└── UI
    ├── Canvas (HUD)
    └── EventSystem
```

## Testing Visuals

1. **Press Play** in Unity Editor
2. **Maximize Game View** (Shift + Space)
3. **Test lighting at different times:**
   - Add DayNightCycle component to see dynamic lighting
   - Add WeatherSystem for atmospheric variety
4. **Adjust settings** in real-time while playing
5. **Save changes** to the scene when satisfied

## Performance vs Quality Trade-offs

| Setting | Performance | Quality | Recommendation |
|---------|-------------|---------|----------------|
| Shadow Distance | High impact | Medium | 50-75 units |
| Shadow Resolution | Medium impact | High | Medium |
| Texture Size | Medium impact | High | 256x256 |
| Anti-Aliasing | High impact | High | 2x MSAA |
| Post Processing | Medium impact | High | Use selectively |
| World Size | Very high impact | Low | 100-200 units |

## Final Tips

1. **Keep it stylized:** Don't aim for realism - Palia-style means artistic and optimized
2. **Test on target hardware:** What looks good in editor might perform differently
3. **Use simple shaders:** Standard shader with low glossiness works great
4. **Ambient light is key:** Proper ambient lighting makes terrain look much better
5. **Don't overdo effects:** Subtle is better for performance and visual clarity

## Example Scene Setup (Copy-Paste Ready)

Create a new script `SceneVisualSetup.cs`:

```csharp
using UnityEngine;

public class SceneVisualSetup : MonoBehaviour
{
    [ContextMenu("Setup Scene Visuals")]
    void SetupVisuals()
    {
        // Setup lighting
        Light sun = FindObjectOfType<Light>();
        if (sun == null)
        {
            GameObject lightObj = new GameObject("Sun");
            sun = lightObj.AddComponent<Light>();
            sun.type = LightType.Directional;
        }
        sun.transform.rotation = Quaternion.Euler(50, -30, 0);
        sun.color = new Color(1f, 0.98f, 0.94f);
        sun.intensity = 1.2f;
        sun.shadows = LightShadows.Soft;
        
        // Setup ambient
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.4f, 0.4f, 0.5f);
        
        // Setup fog
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.7f, 0.78f, 0.86f);
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.002f;
        
        Debug.Log("Scene visuals configured!");
    }
}
```

Right-click the component in Inspector and choose "Setup Scene Visuals" to apply.
