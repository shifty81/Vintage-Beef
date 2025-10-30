# Quick Start Guide - Get Running in Unity

This guide will get you up and running in Unity in under 5 minutes so you can walk around and test the game.

## Prerequisites
- Unity 2022.3.10f1 or later installed
- This repository cloned

## Step 1: Open the Project (1 minute)

1. Open Unity Hub
2. Click "Add" → "Add project from disk"
3. Navigate to the cloned `Vintage-Beef` folder
4. Select the folder and click "Add Project"
5. Click on the project to open it
6. Wait for Unity to import everything (first time takes 2-3 minutes)

## Step 2: Quick Scene Setup (2 minutes)

### Option A: Test Single Player (Fastest - 30 seconds)

1. In Unity, open `Assets/Scenes/GameWorld.unity`
2. Press the **Play** button (▶) at the top
3. You should spawn in the world!

**Controls:**
- WASD - Move
- Mouse - Look around
- Space - Jump
- Shift - Sprint
- ESC - Unlock cursor

**What you'll see:**
- A basic flat world (using SimpleWorldGenerator)
- Some purple dungeon entrance cubes
- You can walk around freely

### Option B: Test Procedural World (2 minutes)

If you want to see the biomes and resources:

1. Open `Assets/Scenes/GameWorld.unity`
2. In the Hierarchy, find or create a GameObject called "Environment"
3. Select "Environment" and in Inspector, click "Add Component"
4. Type "ProceduralWorldGenerator" and add it
5. Find "SimpleWorldGenerator" component on any object and **disable it** (uncheck it)
6. Press Play!

**What you'll see:**
- Procedurally generated terrain with hills
- Different colored areas (biomes)
- Trees, rocks, and plants scattered around
- Press 'E' near resources to gather them
- Press 'I' to open inventory

## Step 3: Optional Enhancements (1 minute each)

### Add Day/Night Cycle
1. Select "Environment" GameObject
2. Add Component → "DayNightCycle"
3. Press Play
4. Watch the sun rise and set!

### Add Weather
1. Select "Environment" GameObject  
2. Add Component → "WeatherSystem"
3. Press Play
4. Weather will change every 60 seconds (rain, fog, clear)

### Add Inventory
1. The inventory system works automatically
2. Gather resources near trees/rocks/plants (press E)
3. Press 'I' to see your inventory
4. Items stack automatically

## Troubleshooting

### "No player spawns"
- GameManager should exist in MainMenu scene and persist
- If missing, create empty GameObject, add GameManager script
- It will auto-create a player

### "World doesn't generate"
- Make sure SimpleWorldGenerator is disabled if using ProceduralWorldGenerator
- Check Console (Ctrl+Shift+C) for errors
- Both generators work, but only one should be active

### "Can't move"
- Click in the Game window to focus it
- Press ESC to lock cursor
- Make sure PlayerController script is on the player

### "Performance is slow"
- Reduce ProceduralWorldGenerator world size (Inspector: World Size = 100)
- Reduce chunk size (Chunk Size = 10)
- Close other applications

## Testing Multiplayer (Optional - 5 minutes)

To test multiplayer, you need to build the game first:

1. File → Build Settings
2. Add Open Scenes (MainMenu, Lobby, GameWorld)
3. Click "Build"
4. Run the built executable as Host
5. In Unity Editor, click Play and join as Client

See UNITY_SETUP.md for detailed multiplayer setup.

## What's Currently Working

✅ **Player Movement**
- Walk, run, jump
- Camera controls
- Basic physics

✅ **World Generation**
- Simple flat world (SimpleWorldGenerator)
- Procedural terrain with biomes (ProceduralWorldGenerator)
- Resource placement

✅ **Resource Gathering**
- Press 'E' near trees, rocks, plants
- Resources respawn after 60 seconds
- Items go to inventory

✅ **Inventory**
- Press 'I' to open/close
- 30 slots with stacking
- Automatic management

✅ **Day/Night & Weather**
- Dynamic lighting
- Weather transitions
- Atmospheric effects

## What's Not Set Up Yet (Requires Manual Unity Setup)

❌ **Multiplayer UI**
- Connection screen in Lobby
- Needs UI elements created

❌ **Chat UI**
- Chat window in GameWorld
- Needs UI panels created

❌ **Network Player Prefab**
- Needs to be created and configured
- See UNITY_SETUP.md for details

❌ **Main Menu**
- Needs button linking
- Simple to set up

## Quick Tips

1. **Start Simple**: Just open GameWorld and press Play. You can walk around immediately.

2. **Test Features One by One**: 
   - First test player movement
   - Then add ProceduralWorldGenerator
   - Then add DayNight and Weather
   - Then test gathering and inventory

3. **Use Console**: 
   - Press Ctrl+Shift+C to open Console
   - Watch for debug messages
   - Errors show in red

4. **Inspector is Your Friend**:
   - Select any GameObject
   - See all components and settings
   - Adjust values in real-time while playing

5. **Game View Settings**:
   - Maximize Game view (Shift+Space)
   - Set to Free Aspect for full screen
   - Click "Maximize on Play" for best testing

## Performance Settings

If the game runs slow:

**In ProceduralWorldGenerator:**
- World Size: 100 (instead of 200)
- Chunk Size: 10 (instead of 20)
- Trees/Rocks/Plants Per Chunk: Lower the numbers

**In Unity:**
- Edit → Project Settings → Quality
- Select "Low" or "Medium" quality level
- Disable shadows if needed

**In WeatherSystem:**
- Max Rain Particles: 500 (instead of 1000)

## Next Steps

Once you're comfortable walking around:

1. **Test all features** - Gather resources, check inventory, watch day/night
2. **Adjust settings** - Modify world size, resource counts, day length
3. **Set up multiplayer** - Follow UNITY_SETUP.md for full setup
4. **Add custom content** - Start tweaking biome colors, resource types, etc.

## Getting Help

- **Console errors?** - Check the error message, usually tells you what's missing
- **Can't find a script?** - Search in Project window (Ctrl+F)
- **Game won't play?** - Check for compilation errors in Console
- **Need more detail?** - See UNITY_SETUP.md for comprehensive setup

---

**You should be walking around in the game within 5 minutes!**

Just open GameWorld.unity and press Play. Everything else is optional enhancements.
