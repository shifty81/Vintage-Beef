# Creating Basic Prefabs for Vintage Beef

This guide shows how to create the basic prefabs needed for full gameplay.

## Why Prefabs?

Prefabs are reusable GameObject templates. While the game is playable without them, prefabs enable:
- Resource nodes in the world (trees, rocks, plants)
- Multiplayer player spawning
- Dungeon entrances
- Consistent object behavior

## Priority Prefabs

### Essential (For Full Experience)
1. **NetworkPlayer** - Required for multiplayer
2. **ResourceNode (Tree, Rock, Plant)** - For resource gathering

### Optional (Future Content)
3. **DungeonEntrance** - For dungeon system
4. **BuildingPieces** - For building system

---

## 1. Creating NetworkPlayer Prefab

**Required for:** Multiplayer gameplay

### Steps:

1. **Create Player GameObject**
   - Hierarchy → Create Empty
   - Name: "NetworkPlayer"

2. **Add Components**
   - CharacterController
     - Height: 2
     - Radius: 0.5
   - NetworkObject (Unity.Netcode)
     - Check "Is Player Object"
   - NetworkTransform (Unity.Netcode.Components)
     - Check "Synchronize Position"
     - Check "Synchronize Rotation"
   - NetworkPlayer (VintageBeef.Network)
   - PlayerController (VintageBeef)

3. **Create Camera Child**
   - Right-click NetworkPlayer → Create Empty
   - Name: "PlayerCamera"
   - Position: (0, 1.6, 0)
   - Add Camera component
   - Add AudioListener component

4. **Link References**
   - Select NetworkPlayer
   - In PlayerController component:
     - Drag PlayerCamera into "Camera Transform" field

5. **Create Prefab**
   - Drag NetworkPlayer from Hierarchy to Assets/Prefabs folder
   - Delete NetworkPlayer from Hierarchy (only keep prefab)

6. **Configure in NetworkManager**
   - See UNITY_SETUP.md for NetworkManager configuration

---

## 2. Creating Resource Node Prefabs

**Required for:** Resource gathering gameplay

### Tree Prefab

1. **Create Tree Object**
   - Hierarchy → 3D Object → Cylinder (temporary visual)
   - Name: "TreeNode"
   - Scale: (1, 3, 1) - make it tall like a tree

2. **Set Material**
   - Create Material: Assets/Materials/TreeMaterial
   - Color: Brown/Dark green
   - Apply to cylinder

3. **Add Components**
   - ResourceNode script
     - Resource Type: Tree
     - Resource Amount: 3
     - Respawn Time: 60

4. **Add Collider**
   - The cylinder already has a collider
   - Or add BoxCollider if needed
   - Check "Is Trigger" if using trigger-based interaction

5. **Create Prefab**
   - Drag TreeNode to Assets/Prefabs/
   - Can now delete from Hierarchy

### Rock Prefab

1. **Create Rock Object**
   - Hierarchy → 3D Object → Cube
   - Name: "RockNode"
   - Scale: (1.5, 1, 1.5) - make it look rocky
   - Rotate: (15, 25, 10) - angle it for variety

2. **Set Material**
   - Create Material: Assets/Materials/RockMaterial
   - Color: Gray/Dark gray
   - Apply to cube

3. **Add Components**
   - ResourceNode script
     - Resource Type: Rock
     - Resource Amount: 3
     - Respawn Time: 60

4. **Create Prefab**
   - Drag RockNode to Assets/Prefabs/
   - Delete from Hierarchy

### Plant Prefab

1. **Create Plant Object**
   - Hierarchy → 3D Object → Sphere
   - Name: "PlantNode"
   - Scale: (0.5, 0.5, 0.5) - small plant

2. **Set Material**
   - Create Material: Assets/Materials/PlantMaterial
   - Color: Bright green
   - Apply to sphere

3. **Add Components**
   - ResourceNode script
     - Resource Type: Plant
     - Resource Amount: 2
     - Respawn Time: 60

4. **Create Prefab**
   - Drag PlantNode to Assets/Prefabs/
   - Delete from Hierarchy

---

## 3. Placing Resource Nodes in GameWorld

Once prefabs are created, place them in the world:

### Manual Placement

1. Open GameWorld scene
2. Drag prefabs from Assets/Prefabs/ into Hierarchy
3. Position them around the world (Y=0 for ground level)
4. Create multiple instances for variety

### Organized Placement

1. Create empty GameObject: "Resources"
2. Create children: "Trees", "Rocks", "Plants"
3. Drag resource instances under appropriate parent
4. Spread them around the world

Example layout:
```
Resources
├── Trees
│   ├── TreeNode (0)
│   ├── TreeNode (1)
│   └── TreeNode (2)
├── Rocks
│   ├── RockNode (0)
│   └── RockNode (1)
└── Plants
    ├── PlantNode (0)
    ├── PlantNode (1)
    └── PlantNode (2)
```

### Quick Distribution

For quick testing, place resources in a circle around spawn:
- Spawn at (0, 0, 0)
- Trees at: (10, 0, 0), (-10, 0, 0), (0, 0, 10), (0, 0, -10)
- Rocks at: (15, 0, 15), (-15, 0, 15), (15, 0, -15), (-15, 0, -15)
- Plants scattered between

---

## 4. Creating Dungeon Entrance Prefab

**Required for:** Dungeon system (future)

1. **Create Entrance Marker**
   - Hierarchy → 3D Object → Cube
   - Name: "DungeonEntrance"
   - Scale: (2, 3, 2) - make it noticeable
   - Position slightly above ground (Y = 1.5)

2. **Set Material**
   - Create Material: Assets/Materials/DungeonMaterial
   - Color: Purple/Magenta (makes it visible)
   - Emission: Check "Emission", set color to purple
   - Apply to cube

3. **Add Components**
   - DungeonEntrance script
   - BoxCollider (if not present)
   - Set collider "Is Trigger" if using triggers

4. **Create Prefab**
   - Drag to Assets/Prefabs/
   - Delete from Hierarchy

5. **Place in World**
   - Drag prefab into GameWorld scene
   - Position at various locations
   - Recommended: 5-10 entrances spread across map

---

## 5. Testing Prefabs

### Test Resource Nodes

1. Place a few resource node prefabs in GameWorld
2. Press Play
3. Walk to a resource node
4. Press 'E' when close
5. Verify:
   - Resource scales down
   - Item added to inventory (press 'I' to check)
   - Resource respawns after 60 seconds

### Test NetworkPlayer

1. Follow UNITY_SETUP.md for full multiplayer setup
2. Build game executable
3. Run as host
4. Join with Unity Editor
5. Verify both players see each other

### Test Dungeon Entrance

1. Place entrance prefab in GameWorld
2. Press Play
3. Walk to entrance
4. Press 'E' when close
5. Check Console for interaction message

---

## Material Creation Quick Reference

For all prefabs, you'll want basic materials:

### Creating a Material

1. Right-click in Assets/Materials/
2. Create → Material
3. Name it appropriately
4. Set properties:
   - Rendering Mode: Opaque (default)
   - Albedo: Base color
   - Metallic: 0 (for natural objects)
   - Smoothness: 0.2-0.5 (for natural objects)

### Material Color Suggestions

- **Tree**: #3d2817 (dark brown) for trunk, #2d5016 (dark green) for leaves
- **Rock**: #4a4a4a (gray) to #2a2a2a (dark gray)
- **Plant**: #3fc73f (bright green)
- **Dungeon Entrance**: #8b2fc7 (purple) with emission

---

## Advanced: ProceduralWorldGenerator Integration

The ProceduralWorldGenerator can spawn resources automatically!

### Enable Auto-Spawning

1. Find ProceduralWorldGenerator in GameWorld scene
2. In Inspector, locate resource spawning settings:
   - Trees Per Chunk: 5
   - Rocks Per Chunk: 3
   - Plants Per Chunk: 8
3. Assign your prefabs:
   - Tree Prefab: Drag TreeNode prefab
   - Rock Prefab: Drag RockNode prefab
   - Plant Prefab: Drag PlantNode prefab

4. Press Play - resources spawn automatically across biomes!

**Note:** This requires the ProceduralWorldGenerator script to have public fields for prefabs. If not present, prefabs can be added via code modification.

---

## Prefab Checklist

- [ ] NetworkPlayer prefab created
- [ ] NetworkPlayer has all required components
- [ ] Camera child configured
- [ ] TreeNode prefab created
- [ ] RockNode prefab created
- [ ] PlantNode prefab created
- [ ] All resource nodes have ResourceNode script
- [ ] Materials created and applied
- [ ] DungeonEntrance prefab created (optional)
- [ ] Prefabs tested in play mode
- [ ] Resources placed in GameWorld (or auto-spawn configured)

---

## Troubleshooting

**"Resource node doesn't respond to 'E' key"**
- Check ResourceNode script is attached
- Verify player is within gather radius (3 meters default)
- Check Console for errors

**"NetworkPlayer won't spawn in multiplayer"**
- Ensure NetworkObject component present
- Check "Is Player Object" is enabled
- Verify prefab registered in NetworkManager

**"Resources don't respawn"**
- Check Respawn Time is set (60 seconds default)
- Watch Console for respawn messages
- Verify resource was fully depleted (3 hits)

**"Can't find ResourceNode script"**
- Make sure you're in correct namespace
- Check script compiled without errors
- Try Assets → Reimport All

---

## Quick Start

**Minimum for basic gameplay:**
1. Create 1 TreeNode, 1 RockNode, 1 PlantNode prefab (10 minutes)
2. Place 3-5 of each in GameWorld (5 minutes)
3. Test gathering with 'E' key

**For multiplayer:**
1. Create NetworkPlayer prefab (15 minutes)
2. Configure NetworkManager (see UNITY_SETUP.md)
3. Build and test (30 minutes)

---

**All prefabs are optional but enhance the experience!** The game is playable without them for basic movement testing.
