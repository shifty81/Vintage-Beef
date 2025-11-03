# Quick Setup Guide - Making the Game Playable with Resource Harvesting

## Overview
This guide shows you exactly how to set up Vintage Beef to be a fully playable, interactable game where you can walk around and harvest resources.

## Time Required: 5-10 minutes

---

## Step 1: Open the Project in Unity

1. Open **Unity Hub**
2. Add the Vintage-Beef project folder
3. Open with **Unity 2022.3.10f1** or later
4. Wait for Unity to import all assets (2-3 minutes)

---

## Step 2: Setup the GameWorld Scene

### 2.1 Open GameWorld Scene
1. In Project window → `Assets/Scenes/GameWorld.unity`
2. Double-click to open

### 2.2 Create or Find Player GameObject

**If Player doesn't exist:**
1. Hierarchy → Right-click → Create Empty
2. Name it "Player"
3. Set Position: (0, 5, 0)

**Required Components on Player:**

#### Add CharacterController
1. Select Player in Hierarchy
2. Add Component → Character Controller
3. Set Center: (0, 1, 0)
4. Set Height: 2
5. Set Radius: 0.5

#### Add PlayerController
1. Add Component → Scripts → Player Controller
2. Leave default settings or customize:
   - Move Speed: 5
   - Sprint Speed: 8
   - Jump Force: 5
   - Mouse Sensitivity: 2

#### Add PlayerInventory (Optional - auto-added)
1. Add Component → Scripts → Player Inventory
2. Leave defaults (30 slots, 99 stack size)
3. **Note:** PlayerInteraction will add this automatically if missing

#### Add PlayerInteraction ⭐ (NEW - REQUIRED)
1. Add Component → Scripts → Player Interaction
2. Settings:
   - Interaction Range: 3 (adjust as needed)
   - Interaction Key: E
   - Leave other fields empty (auto-created)

#### Set Player Tag ⚠️ CRITICAL
1. Select Player GameObject
2. At top of Inspector: Tag dropdown
3. Select "Player" (or create it if missing)
4. **This is required for resource detection!**

### 2.3 Setup Camera
1. Create Camera as child of Player
2. Set Position: (0, 1.6, 0) - eye height
3. Set Rotation: (0, 0, 0)
4. Tag as "MainCamera"

### 2.4 Setup Terrain System
1. Create Empty GameObject: "TerrainSystem"
2. Add Component → Scripts → Terrain Manager
3. In TerrainManager:
   - Terrain Type: Choose one
     - **Simple** - Flat plane (fast, testing)
     - **Procedural** - Hills and biomes (pretty)
     - **Voxel** - Fully terraformable (full features)
   - For Procedural (recommended for resources):
     - World Size: 200
     - Chunk Size: 20
     - Trees Per Chunk: 5
     - Rocks Per Chunk: 3
     - Plants Per Chunk: 8

---

## Step 3: Configure Build Settings (Optional)

For full game flow (menus, lobbies):

1. File → Build Settings
2. Add scenes in order:
   - MainMenu
   - Lobby
   - GameWorld
3. Close window

---

## Step 4: Setup Lighting (Recommended)

For better visuals:

1. Hierarchy → Right-click → Light → Directional Light
2. Name it "Sun"
3. Rotation: (50, -30, 0)
4. Intensity: 1
5. Color: Slightly warm white

---

## Step 5: Test the Game! 🎮

### 5.1 Start Play Mode
1. Click **Play** button (▶) at top of Unity Editor
2. Wait 2-3 seconds for terrain generation
3. Console should show:
   ```
   [TerrainManager] Terrain generation started...
   [Procedural World Generator] Generating world...
   [TerrainManager] Terrain ready for player spawning
   ```

### 5.2 Test Movement
- **WASD** - Move around
- **Mouse** - Look around  
- **Space** - Jump
- **Left Shift** - Sprint
- Click in Game window if movement doesn't work

### 5.3 Test Resource Gathering
1. **Walk around** to find resources
   - Look for trees (brown cylinder + green sphere)
   - Look for rocks (gray spheres)
   - Look for plants (green/brown objects)

2. **Approach a resource** (get within 3 units)
   - A UI prompt should appear at bottom center:
   - "Press E to gather Tree" (or Rock, Plant)

3. **Press E** to gather
   - Console shows: `[PlayerInteraction] Successfully gathered Tree`
   - Resource gets smaller
   - After 3 gathers, resource disappears (respawns in 60s)

4. **Open Inventory** (Press I)
   - Should see gathered resources
   - Example: "Wood x3", "Stone x2", "Herbs x1"
   - Press I again to close

### 5.4 Expected Console Output
```
[PlayerInteraction] Created interaction prompt UI
[PlayerInventory] Inventory initialized with 30 slots
[PlayerInteraction] Successfully gathered Tree
[ResourceNode] Gathered Tree from Forest biome. HP: 2/3
[ResourceNode] Added 3x Wood to inventory
[PlayerInventory] Added 3 Wood to new slot
```

---

## Troubleshooting

### Player Can't Move
- ✅ Click in Game window to focus
- ✅ Check Player has CharacterController
- ✅ Check PlayerController is enabled
- ✅ Press ESC to lock cursor

### No Interaction Prompt
- ✅ Verify Player has tag "Player" (most common issue!)
- ✅ Check PlayerInteraction component exists on Player
- ✅ Verify you're within 3 units of a resource
- ✅ Check Console for warnings

### Can't Gather Resources
- ✅ Make sure prompt appears first ("Press E to gather...")
- ✅ Check resource has a Collider (should have automatically)
- ✅ Verify PlayerInventory component exists
- ✅ Look for errors in Console window

### Resources Don't Spawn
- ✅ Wait 2-3 seconds after pressing Play
- ✅ Check TerrainManager is set to "Procedural" (not Simple)
- ✅ Increase Trees/Rocks/Plants Per Chunk if needed
- ✅ Look in Hierarchy → TerrainSystem → Chunks → Resources

### Inventory Doesn't Open
- ✅ Press I key (not i, use uppercase mapping)
- ✅ Check if InventoryUI exists in scene (optional, but helpful)
- ✅ Try PrintInventory in Console with: `player.GetComponent<PlayerInventory>().PrintInventory()`

### Scene Looks Dark
- ✅ Add Directional Light (see Step 4)
- ✅ Window → Rendering → Lighting → Generate Lighting

---

## Verification Checklist

Before saying "it works":

- [ ] Player moves with WASD
- [ ] Camera rotates with mouse
- [ ] Player can jump (Space) and sprint (Shift)
- [ ] Terrain generates and is visible
- [ ] Resources are visible in world (trees, rocks, plants)
- [ ] Walking near resource shows "Press E to gather" prompt
- [ ] Pressing E gathers the resource
- [ ] Console shows gathering messages
- [ ] Pressing I opens inventory
- [ ] Gathered resources appear in inventory
- [ ] Resource gets smaller with each gather
- [ ] Resource disappears after 3 gathers
- [ ] Resource respawns after 60 seconds

---

## Advanced: Creating Custom Resources

If you want to create prefabs instead of procedural resources:

1. Create GameObject (e.g., Sphere for rock)
2. Scale and position as desired
3. Add Component → Scripts → Resource Node
4. Configure:
   - Resource Type: Rock (or Tree, Plant)
   - Biome: Forest (or Plains, Desert, Mountains)
   - Hit Points: 3
   - Gather Radius: 3
5. **Critical:** Ensure it has a Collider
6. Place in world at ground level
7. Done! It will now be gatherable

---

## Performance Tips

### For Better FPS:
- Use Simple terrain for quick testing
- Reduce Trees/Rocks/Plants Per Chunk
- Lower World Size (e.g., 100 instead of 200)
- Close Unity Inspector during Play mode

### For Better Visuals:
- Use Procedural or Voxel terrain
- Add Directional Light (Sun)
- Enable shadows on light
- Add fog for atmosphere

---

## What's Next?

Once gathering works:

1. **Create Crafting System** - Use gathered resources
2. **Add Tools** - Require axe for trees, pickaxe for rocks
3. **Implement Professions** - Miner gets bonus ore, etc.
4. **Build Structures** - Use wood and stone
5. **Add Multiplayer** - Gather with friends!
6. **Create Quests** - "Gather 10 wood"

---

## Summary

You now have a **fully playable, interactable game** where players can:
- ✅ Walk around freely in a procedurally generated world
- ✅ Find resources scattered throughout different biomes
- ✅ See visual prompts when near harvestable resources
- ✅ Gather resources with the E key
- ✅ Store resources in a persistent inventory
- ✅ Watch resources respawn over time

**Total Setup Time:** ~5-10 minutes  
**Code Required:** 0 lines (all implemented!)  
**Unity Setup:** Basic GameObject configuration  

**The game is now ready to play!** 🎉

---

## Getting Help

If you encounter issues:

1. Check Console window for errors
2. Review Troubleshooting section above
3. Read INTERACTION_SYSTEM.md for technical details
4. Review CURRENT_STATE.md for feature status
5. Check GitHub Issues for similar problems

---

Last Updated: Version 0.3.2 - Resource Interaction System
