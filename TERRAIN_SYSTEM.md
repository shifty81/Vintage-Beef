# Terrain System Documentation

## Overview

The Vintage Beef terrain system provides procedural world generation with support for future terraforming capabilities. The system is designed to be performant and compatible with Palia-style graphics.

## Components

### TerrainManager
**Location:** `Assets/Scripts/TerrainManager.cs`

The TerrainManager is the central coordinator for terrain generation and player spawning. It ensures that:
- Players spawn at the correct terrain height
- Terrain is fully generated before player spawning
- Provides an API for future terrain modification (terraforming)

**Key Features:**
- Singleton pattern for global access
- Coordinates with ProceduralWorldGenerator or SimpleWorldGenerator
- Calculates safe spawn positions based on terrain height
- Provides hooks for future voxel-based terrain editing

**Usage:**
```csharp
// Get terrain height at a position
float height = TerrainManager.Instance.GetTerrainHeight(x, z);

// Get a safe spawn position
Vector3 spawnPos = TerrainManager.Instance.GetSafeSpawnPosition();

// Get a random spawn position within radius
Vector3 randomSpawn = TerrainManager.Instance.GetRandomSpawnPosition(5f);
```

### TerrainTextureGenerator
**Location:** `Assets/Scripts/TerrainTextureGenerator.cs`

Generates procedural textures for terrain based on biome types. Creates stylized, Palia-like textures that are:
- Lightweight (256x256 by default)
- Procedurally generated using Perlin noise
- Biome-specific with appropriate colors
- Performance-optimized

**Biome Textures:**
- **Forest:** Rich green with darker variations
- **Plains:** Grassy green with lighter accents
- **Desert:** Sandy yellow with brown variations
- **Mountains:** Stone gray with lighter highlights

**Usage:**
The texture generator is automatically used by world generators when `useProceduralTextures` is enabled.

### ProceduralWorldGenerator (Enhanced)
**Location:** `Assets/Scripts/ProceduralWorldGenerator.cs`

Enhanced with:
- Procedural texture generation support
- Better integration with TerrainManager
- Biome-based texture assignment

**New Settings:**
- `useProceduralTextures`: Enable/disable procedural texture generation

### SimpleWorldGenerator (Enhanced)
**Location:** `Assets/Scripts/SimpleWorldGenerator.cs`

Enhanced with:
- Procedural grass texture generation
- Better visual quality for simple terrain

**New Settings:**
- `useProceduralTexture`: Enable/disable procedural texture generation

## Player Spawning System

The player spawning system has been improved to ensure players always spawn at the correct terrain height:

1. **GameWorld Scene Loads**
2. **TerrainManager initializes** and sets up the appropriate world generator
3. **World generation occurs** (ProceduralWorldGenerator or SimpleWorldGenerator)
4. **TerrainManager marks terrain as ready**
5. **GameManager waits for terrain ready signal**
6. **Player spawns at calculated safe position** based on actual terrain height

This prevents issues where players would spawn underground or floating in the air.

## Future Terraforming Support

The terrain system is designed with future terraforming capabilities in mind:

### Planned Features
- **Voxel-based terrain editing:** Add/remove terrain blocks
- **Sculpting tools:** Raise/lower terrain with brushes
- **Terrain modification API:** `ModifyTerrain()`, `AddTerrainBlock()`, `RemoveTerrainBlock()`
- **Undo/Redo system:** Track and revert terrain changes
- **Networked terraforming:** Synchronize terrain changes across multiplayer clients

### Best Practices (from Research)
Based on Unity terrain best practices research, the system is structured to support:

1. **Chunk-based architecture:** The procedural generator already uses chunks
2. **Efficient mesh regeneration:** Only rebuild modified chunks
3. **Heightmap or voxel choice:** Currently heightmap-based, can be extended to voxels
4. **Multithreading potential:** Use Unity Jobs System for terrain generation
5. **Collision updates:** Regenerate colliders only for modified areas

## Configuration

### TerrainManager Settings (in Unity Inspector)
- **Use Procedural Terrain:** Toggle between ProceduralWorldGenerator and SimpleWorldGenerator
- **Spawn Point:** Center point for player spawning (default: 0, 0, 0)

### ProceduralWorldGenerator Settings
- **World Size:** Total world dimensions (default: 200)
- **Chunk Size:** Size of each terrain chunk (default: 20)
- **Height Multiplier:** Terrain elevation scale (default: 10)
- **Noise Scale:** Detail level of terrain (default: 0.05)
- **Use Procedural Textures:** Enable texture generation (default: true)

### TextureGenerator Settings
- **Texture Size:** Resolution of generated textures (default: 256x256)
- **Noise Scale:** Detail level of texture variations (default: 0.1)

## Setup in Unity

### For GameWorld Scene:

1. Create an empty GameObject named "TerrainSystem"
2. Add the `TerrainManager` component
3. Configure settings in Inspector:
   - Check "Use Procedural Terrain" for varied terrain
   - Uncheck for flat world (SimpleWorldGenerator)
4. The TerrainManager will automatically add the appropriate generator
5. Textures will be generated automatically if enabled

### Testing:

1. Open the GameWorld scene
2. Press Play
3. The terrain will generate with textures
4. Player will spawn at the correct height
5. You can walk around and explore the world

## Performance Considerations

- **Texture Size:** 256x256 provides good quality with minimal memory
- **Chunk Size:** 20 units balances detail and performance
- **World Size:** 200 units provides ample space without overwhelming the system
- **Texture Generation:** Occurs once at startup, minimal runtime cost

## Integration with Existing Systems

The terrain system integrates seamlessly with:
- **Resource System:** Resources spawn on terrain at correct heights
- **Dungeon System:** Dungeon entrances positioned on terrain
- **Housing System:** Housing areas placed on flat terrain sections
- **Day/Night Cycle:** Lighting interacts properly with textured terrain
- **Weather System:** Weather effects render correctly on terrain

## Troubleshooting

### Player spawns underground
- Ensure TerrainManager is present in the scene
- Check that `IsTerrainReady()` returns true before spawning
- Verify spawn position is within world bounds

### Textures look incorrect
- Check that `useProceduralTextures` is enabled
- Verify TerrainTextureGenerator is attached (done automatically)
- Adjust `textureSize` and `noiseScale` for different looks

### Performance issues
- Reduce `worldSize` (e.g., 100 instead of 200)
- Reduce `textureSize` (e.g., 128 instead of 256)
- Reduce `chunkSize` for fewer chunks
- Disable procedural textures and use solid colors

## Example Code

### Checking if player is on terrain
```csharp
Vector3 playerPos = player.transform.position;
float terrainHeight = TerrainManager.Instance.GetTerrainHeight(playerPos.x, playerPos.z);

if (playerPos.y < terrainHeight)
{
    // Player is below terrain
    playerPos.y = terrainHeight + 0.5f;
    player.transform.position = playerPos;
}
```

### Future terraforming example
```csharp
// Raise terrain at position (future implementation)
void RaiseTerrain(Vector3 position, float radius, float strength)
{
    TerrainManager.Instance.ModifyTerrain(position, radius, strength);
}

// Place block (voxel-style, future implementation)
void PlaceBlock(Vector3 position)
{
    TerrainManager.Instance.AddTerrainBlock(position);
}
```

## References

This system was designed based on Unity terrain generation best practices:
- Chunk-based architecture for scalability
- Procedural generation using Perlin noise
- Efficient texture generation
- Performance optimization for Palia-style graphics
- Foundation for future voxel-based terraforming

## Version History

**v0.3.1 (Current)**
- Added TerrainManager for centralized terrain control
- Added TerrainTextureGenerator for procedural textures
- Enhanced ProceduralWorldGenerator with texture support
- Enhanced SimpleWorldGenerator with texture support
- Fixed player spawning to use correct terrain height
- Added API hooks for future terraforming features
