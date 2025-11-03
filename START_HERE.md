# 🎮 Vintage Beef - Complete Getting Started Guide

**Welcome!** This is your one-stop guide to get Vintage Beef up and running.

## 📖 Quick Navigation

Choose your path based on what you need:

### Just Want to Play? (2 minutes)
→ **[Quick Play Guide](#quick-play-2-minutes)** below

### Want Full Experience? (15 minutes)
→ **[PLAY_NOW.md](PLAY_NOW.md)** - Detailed setup guide
→ **[SETUP_CHECKLIST.md](SETUP_CHECKLIST.md)** - Step-by-step checklist

### Want to Understand the Project?
→ **[CURRENT_STATE.md](CURRENT_STATE.md)** - What's working and what's not
→ **[README.md](README.md)** - Full project overview

### Need Specific Help?
→ **[PREFAB_GUIDE.md](PREFAB_GUIDE.md)** - Creating game objects
→ **[UNITY_SETUP.md](UNITY_SETUP.md)** - Multiplayer setup
→ **[VOXEL_QUICKSTART.md](VOXEL_QUICKSTART.md)** - Voxel terrain guide

---

## ⚡ Quick Play (2 minutes)

**Get playing in under 2 minutes:**

1. **Open Unity**
   - Unity Hub → Add → Select Vintage-Beef folder
   - Open with Unity 2022.3.10f1 or later

2. **Open GameWorld Scene**
   - Project window → Assets/Scenes/GameWorld.unity
   - Double-click to open

3. **Press Play**
   - Click Play button (▶) at top of editor
   - You're now in the game!

4. **Play**
   - **WASD** - Move around
   - **Mouse** - Look around
   - **Space** - Jump
   - **Left Shift** - Sprint
   - **ESC** - Unlock cursor

**That's it!** You're playing Vintage Beef.

---

## 🎯 Full Setup (15 minutes)

For the complete experience with menus, UI, and all features:

### Step 1: Use the Scene Setup Helper (5 minutes)

1. **Open Unity** (if not already)
2. **Use the helper tool:**
   - Unity menu bar → `Vintage Beef` → `Scene Setup Helper`
   - For each scene:
     - Open the scene (MainMenu, Lobby, or GameWorld)
     - Click the corresponding setup button
     - Wait for "Setup complete!" message
3. **Done!** Scenes are configured.

### Step 2: Configure Build Settings (1 minute)

1. **File → Build Settings**
2. **Add scenes** (if not already added):
   - Click "Add Open Scenes" for each scene
   - Or drag scenes from Assets/Scenes/
   - Order: MainMenu, Lobby, GameWorld
3. **Ensure all scenes are checked**
4. **Close window**

### Step 3: Test the Game (2 minutes)

1. **Open MainMenu scene**
2. **Press Play**
3. **Test flow:**
   - Click "PLAY" button
   - Select a profession (e.g., Explorer)
   - Click "START GAME"
   - You spawn in game world!
4. **Test features:**
   - Walk around (WASD)
   - Press 'I' for inventory
   - Check day/night cycle
   - See weather changes

**Congratulations!** Full game is now playable.

---

## 🎨 Optional Enhancements

### Add Better Terrain (5 minutes)

1. **In GameWorld scene**, find or create "TerrainSystem"
2. **Add TerrainManager component** (if not present)
3. **Choose terrain type:**
   - **Simple** - Flat plane (fastest)
   - **Procedural** - Hills and biomes (pretty)
   - **Voxel** - Fully terraformable (most features)
4. **Press Play** - Terrain generates!

### Add Resources (15 minutes)

See **[PREFAB_GUIDE.md](PREFAB_GUIDE.md)** to create:
- Tree nodes (for wood)
- Rock nodes (for stone)
- Plant nodes (for herbs)

### Setup Multiplayer (30 minutes)

See **[UNITY_SETUP.md](UNITY_SETUP.md)** for complete multiplayer setup.

---

## 📊 What's Currently Working

### ✅ Core Gameplay
- Player movement (WASD, mouse look, jump, sprint)
- Camera controls (first-person)
- Physics-based character controller
- Three terrain systems to choose from
- Day/night cycle with dynamic lighting
- Weather system (clear, rain, fog)
- Inventory system (30 slots, stacking)
- Resource gathering framework
- Voxel terraforming (dig and build)
- 12 profession system
- Scene flow (Menu → Lobby → Game)

### ⚠️ Needs Scene Setup
- UI elements (buttons, text, panels)
- Lights in GameWorld
- Canvas setup for inventory
- Multiplayer prefabs

### ❌ Not Yet Implemented
- Crafting system
- Profession abilities
- Combat and enemies
- Dungeon content
- Building system
- Quest system

See **[CURRENT_STATE.md](CURRENT_STATE.md)** for detailed breakdown.

---

## 🎮 Controls Reference

| Action | Key/Button |
|--------|-----------|
| Move Forward | W |
| Move Back | S |
| Move Left | A |
| Move Right | D |
| Look Around | Mouse |
| Jump | Space |
| Sprint | Left Shift |
| Unlock Cursor | ESC |
| Open Inventory | I |
| Interact/Gather | E |
| Place Voxel | Right Mouse |
| Remove Voxel | Left Mouse |
| Change Block (Voxel) | 1, 2, 3, 4 |

---

## 🔧 Troubleshooting

### Can't move the player
- Click in Game window to focus
- Press ESC to lock cursor
- Ensure PlayerController script exists on player

### No terrain visible
- Wait 2-3 seconds for generation
- Check Console for errors
- Try "Simple" terrain type first
- Ensure TerrainManager component exists

### Buttons don't work
- Ensure Canvas exists in scene
- Check EventSystem exists
- Verify button OnClick events connected
- Run Scene Setup Helper

### Scene won't load
- Add scenes to Build Settings
- Check scene names match exactly (case-sensitive)
- Look for errors in Console

### Scripts won't compile
- Check all using statements correct
- Assets → Reimport All
- Restart Unity Editor

### Performance issues
- Use "Simple" terrain for testing
- Lower quality settings (Edit → Project Settings → Quality)
- Reduce world size in terrain settings

---

## 📚 Documentation Index

### Getting Started
- **[START_HERE.md](START_HERE.md)** ← You are here
- **[PLAY_NOW.md](PLAY_NOW.md)** - Detailed setup instructions
- **[SETUP_CHECKLIST.md](SETUP_CHECKLIST.md)** - Quick checklist
- **[QUICKSTART.md](QUICKSTART.md)** - Quick reference

### Understanding the Project
- **[README.md](README.md)** - Project overview
- **[CURRENT_STATE.md](CURRENT_STATE.md)** - What's implemented
- **[ROADMAP.md](ROADMAP.md)** - Development plan
- **[IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)** - Technical details

### Specific Features
- **[VOXEL_QUICKSTART.md](VOXEL_QUICKSTART.md)** - Voxel terrain
- **[VOXEL_SYSTEM.md](VOXEL_SYSTEM.md)** - Voxel technical docs
- **[TERRAIN_SYSTEM.md](TERRAIN_SYSTEM.md)** - Terrain overview
- **[MULTIPLAYER.md](MULTIPLAYER.md)** - Multiplayer info

### Setup Guides
- **[UNITY_SETUP.md](UNITY_SETUP.md)** - Detailed Unity setup
- **[PREFAB_GUIDE.md](PREFAB_GUIDE.md)** - Creating prefabs
- **[VISUAL_SETUP.md](VISUAL_SETUP.md)** - Visual/lighting setup
- **[SETUP.md](SETUP.md)** - General setup info

### Development
- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Code architecture
- **[CONTRIBUTING.md](CONTRIBUTING.md)** - How to contribute
- **[TESTING.md](TESTING.md)** - Testing procedures

---

## ⏱️ Time Investment

| Task | Time Required |
|------|---------------|
| Quick play (GameWorld only) | 2 minutes |
| Full setup with helper tool | 15 minutes |
| Full setup manually | 30 minutes |
| Add resources and prefabs | +15 minutes |
| Setup multiplayer | +30 minutes |
| Total for everything | ~90 minutes |

---

## ✨ What You Can Do Right Now

After basic setup, you can:

1. **Explore procedurally generated worlds**
   - Walk through different biomes
   - See hills, valleys, and varied terrain
   - Experience dynamic weather

2. **Experience dynamic environment**
   - Watch sun rise and set
   - See weather change (rain, fog, clear)
   - Dynamic lighting throughout day

3. **Terraform with voxels** (if voxel terrain selected)
   - Dig holes in the ground
   - Build structures
   - Mine through terrain
   - Create underground caves

4. **Gather resources** (if prefabs created)
   - Find trees, rocks, plants
   - Press 'E' to gather
   - See items in inventory

5. **Test profession system**
   - Select from 12 professions
   - Each with unique characteristics
   - Framework for abilities (coming soon)

6. **Play with friends** (after multiplayer setup)
   - Up to 12 players
   - Explore together
   - Chat system available
   - Synchronized world

---

## 🚀 Next Steps

### Today
1. ✅ Get the game running (2-15 minutes)
2. ✅ Walk around and explore
3. ✅ Test different terrain types
4. ✅ Try voxel terraforming

### This Week
1. Create resource prefabs
2. Place resources in world
3. Test gathering system
4. Set up multiplayer
5. Test with a friend

### Future
1. Wait for crafting system (v0.4.0)
2. Try profession abilities when implemented
3. Explore dungeons when content added
4. Build structures when building system ready

---

## 🎯 Success Criteria

You'll know it's working when:

- ✅ You can press Play and game starts
- ✅ You can move around with WASD
- ✅ Camera follows mouse movement
- ✅ You can jump and sprint
- ✅ Terrain appears and looks good
- ✅ Day/night cycle works
- ✅ Weather changes
- ✅ Inventory opens with 'I'
- ✅ Scene transitions work (Menu → Lobby → Game)

---

## 💡 Pro Tips

1. **Start simple** - Just open GameWorld and press Play first
2. **Use the helper** - Scene Setup Helper saves lots of time
3. **Test incrementally** - Add one feature at a time
4. **Check Console** - Watch for errors and debug messages
5. **Experiment** - Try all three terrain types
6. **Read docs** - Each .md file has specific detailed info
7. **Join community** - Share feedback and get help

---

## 📞 Getting Help

If you get stuck:

1. **Check troubleshooting section** above
2. **Look at Console** for error messages
3. **Review relevant .md file** for specific feature
4. **Check existing issues** on GitHub
5. **Create new issue** with details

---

## 🎉 You're Ready!

Everything you need to play Vintage Beef is now documented:

- ✅ Quick play instructions (2 min)
- ✅ Full setup guide (15 min)
- ✅ Helper tool for automation
- ✅ Troubleshooting solutions
- ✅ Complete documentation index
- ✅ Clear next steps

**Now go play!** Open GameWorld.unity and press Play. 🎮

---

**Last Updated:** v0.3.1
**Estimated Setup Time:** 2-15 minutes depending on desired features
**Current State:** Fully playable with scene setup
