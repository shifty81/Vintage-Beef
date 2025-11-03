# How to Play Vintage Beef - Complete Setup Guide

This guide will get you from a fresh Unity import to a fully playable game in **under 15 minutes**.

## Quick Start (5 Minutes - Minimal Setup)

### Prerequisites
- Unity 2022.3.10f1 or later
- Repository cloned and opened in Unity

### Option A: Direct GameWorld Test (Fastest - 2 minutes)

1. **Open GameWorld Scene**
   - In Unity Project window, navigate to `Assets/Scenes/`
   - Double-click `GameWorld.unity`

2. **Press Play**
   - Click the Play button (▶) at the top center of Unity Editor
   - You should spawn into the world!

3. **Controls**
   - **WASD** - Move around
   - **Mouse** - Look around
   - **Space** - Jump
   - **Left Shift** - Sprint
   - **ESC** - Unlock cursor
   - **I** - Open inventory
   - **E** - Interact with resources

**That's it!** You're now playing the game.

---

## Full Experience Setup (15 Minutes - Complete Flow)

To test the full game flow (Main Menu → Lobby → Game), follow these steps:

### Step 1: Setup Main Menu Scene (3 minutes)

1. **Open MainMenu Scene**
   - Double-click `Assets/Scenes/MainMenu.unity`

2. **Create Canvas** (if not exists)
   - Right-click in Hierarchy → UI → Canvas
   - This creates Canvas and EventSystem automatically

3. **Create Title Text**
   - Right-click Canvas → UI → Text - TextMeshPro
   - Name it "TitleText"
   - In Inspector:
     - Text: "VINTAGE BEEF"
     - Font Size: 72
     - Alignment: Center
     - Position: Top center of screen

4. **Create Play Button**
   - Right-click Canvas → UI → Button - TextMeshPro
   - Name it "PlayButton"
   - Position: Center of screen
   - Change button text to "PLAY"

5. **Create Quit Button**
   - Right-click Canvas → UI → Button - TextMeshPro
   - Name it "QuitButton"
   - Position: Below Play button
   - Change button text to "QUIT"

6. **Add MainMenuUI Script**
   - Create empty GameObject under Canvas: "MenuManager"
   - Add Component → Main Menu UI
   - Drag references:
     - Play Button → PlayButton
     - Quit Button → QuitButton
     - Title Text → TitleText

7. **Create GameManager**
   - Right-click in Hierarchy → Create Empty
   - Name it "GameManager"
   - Add Component → Game Manager (VintageBeef)

8. **Create ProfessionManager**
   - Right-click in Hierarchy → Create Empty
   - Name it "ProfessionManager"
   - Add Component → Profession Manager (VintageBeef)

9. **Create PlayerData**
   - Right-click in Hierarchy → Create Empty
   - Name it "PlayerData"
   - Add Component → Player Data (VintageBeef)

10. **Save Scene** (Ctrl+S)

### Step 2: Setup Lobby Scene (5 minutes)

1. **Open Lobby Scene**
   - Double-click `Assets/Scenes/Lobby.unity`

2. **Create Canvas** (if not exists)
   - Right-click Hierarchy → UI → Canvas
   - EventSystem created automatically

3. **Create Title**
   - Right-click Canvas → UI → Text - TextMeshPro
   - Name: "TitleText"
   - Text: "SELECT YOUR PROFESSION"
   - Position: Top center

4. **Create Profession Container**
   - Right-click Canvas → Create Empty
   - Name: "ProfessionContainer"
   - Add Component → Vertical Layout Group
   - Add Component → Content Size Fitter
     - Vertical Fit: Preferred Size
   - Position: Center of screen

5. **Create Selected Profession Text**
   - Right-click Canvas → UI → Text - TextMeshPro
   - Name: "SelectedText"
   - Text: "Select a profession..."
   - Position: Below container

6. **Create Start Game Button**
   - Right-click Canvas → UI → Button - TextMeshPro
   - Name: "StartGameButton"
   - Text: "START GAME"
   - Position: Bottom center
   - In Inspector: Uncheck "Interactable" (starts disabled)

7. **Add LobbyUI Script**
   - Create empty GameObject under Canvas: "LobbyManager"
   - Add Component → Lobby UI
   - Drag references:
     - Title Text → TitleText
     - Profession Button Container → ProfessionContainer
     - Start Game Button → StartGameButton
     - Selected Profession Text → SelectedText
     - Leave "Profession Button Prefab" empty (will create buttons automatically)

8. **Save Scene** (Ctrl+S)

### Step 3: Setup GameWorld Scene (7 minutes)

1. **Open GameWorld Scene**
   - Double-click `Assets/Scenes/GameWorld.unity`

2. **Create Environment GameObject**
   - Right-click Hierarchy → Create Empty
   - Name: "Environment"

3. **Setup Lighting**
   - Right-click Environment → Light → Directional Light
   - Name: "Sun"
   - Rotation: (50, -30, 0)
   - Color: Light yellow

   - Right-click Environment → Light → Directional Light
   - Name: "Moon"
   - Rotation: (-50, -30, 0)
   - Color: Light blue
   - Intensity: 0.3

4. **Create TerrainSystem**
   - Right-click Hierarchy → Create Empty
   - Name: "TerrainSystem"
   - Add Component → Terrain Manager
   - In Inspector:
     - Terrain Type: Choose one:
       - **Simple** - Flat plane (fastest, good for testing)
       - **Procedural** - Hills and biomes (pretty)
       - **Voxel** - Fully terraformable (most features)

5. **Add Day/Night Cycle** (Optional but recommended)
   - Select "Environment"
   - Add Component → Day Night Cycle
   - Drag references:
     - Sun → Sun
     - Moon → Moon
   - Set Day Duration: 1200 (20 min real time)

6. **Add Weather System** (Optional but recommended)
   - Select "Environment"
   - Add Component → Weather System
   - Set Weather Check Interval: 60

7. **Create Canvas for UI**
   - Right-click Hierarchy → UI → Canvas
   - Set Canvas Scaler:
     - UI Scale Mode: Scale With Screen Size
     - Reference Resolution: 1920x1080

8. **Create Inventory UI Panel**
   - Right-click Canvas → UI → Panel
   - Name: "InventoryPanel"
   - Uncheck "Active" in Inspector (starts hidden)
   - Add Component → Grid Layout Group
   - Set Cell Size: 80x80
   - Set Spacing: 10x10

9. **Create Inventory Manager**
   - Create empty GameObject under Canvas: "InventoryUIManager"
   - Add Component → Inventory UI
   - Drag references:
     - Inventory Panel → InventoryPanel
     - Slots Per Row: 6

10. **Save Scene** (Ctrl+S)

### Step 4: Configure Build Settings

1. **Open Build Settings**
   - File → Build Settings

2. **Add Scenes in Order**
   - Click "Add Open Scenes" or drag scenes:
     1. MainMenu
     2. Lobby
     3. GameWorld
   - Ensure all three are checked

3. **Close Build Settings**

---

## Testing the Game

### Test 1: Quick Play (GameWorld Direct)

1. Open GameWorld scene
2. Press Play
3. You should be able to walk around
4. Press 'I' to test inventory (if inventory UI is set up)

**Expected:** You can move, look around, jump

### Test 2: Full Flow (Main Menu → Game)

1. Open MainMenu scene
2. Press Play
3. Click "PLAY" button
4. Select any profession (e.g., Explorer)
5. Click "START GAME"
6. You should spawn in GameWorld

**Expected:** Complete flow works, you spawn and can play

### Test 3: Resource Gathering (if resources exist)

1. Walk around in GameWorld
2. Look for trees, rocks, or plants
3. Get close and press 'E' to gather
4. Press 'I' to see inventory

**Expected:** Resources are gathered and appear in inventory

---

## Current Features Available

### ✅ Working Features
- Player movement (WASD, mouse look, jump, sprint)
- Camera controls
- Scene transitions (Menu → Lobby → Game)
- Profession selection (12 professions)
- Terrain generation (3 types available)
- Day/night cycle (if set up)
- Weather system (if set up)
- Inventory system (if UI set up)
- Resource gathering (if resources spawned)

### ⚠️ Partially Working (Requires Setup)
- Multiplayer (requires NetworkManager setup)
- Chat system (requires UI setup)
- Resource nodes (require prefabs and placement)
- Dungeon entrances (require prefabs)

### ❌ Not Yet Implemented
- Crafting system
- Profession abilities
- Building system
- Combat system
- Quests and NPCs

---

## Troubleshooting

### "Can't move the player"
- Click in Game window to focus it
- Press ESC to lock cursor
- Check PlayerController is on player GameObject

### "No terrain appears"
- Check TerrainManager has a terrain type selected
- Wait 2-3 seconds for generation (watch Console)
- Try "Simple" terrain type first

### "Buttons don't work"
- Ensure EventSystem exists in scene
- Check button OnClick events are connected
- Try clicking directly in Game view (not Scene view)

### "Scene won't load"
- File → Build Settings → Add all scenes
- Check scene names match exactly (case-sensitive)
- Check Console for error messages

### "Scripts won't compile"
- Check all using statements are correct
- Assets → Reimport All
- Try restarting Unity

### "Player spawns in wrong place"
- TerrainManager may not be ready
- Check Console for terrain generation messages
- Try increasing spawn height in GameManager

---

## Performance Tips

### If game runs slow:

**In TerrainManager:**
- Use "Simple" terrain type for testing
- Reduce world size for Procedural terrain

**In Unity:**
- Edit → Project Settings → Quality
- Select "Low" or "Medium"
- Reduce shadow distance

**In Game view:**
- Lower resolution
- Disable VSync
- Use "Fast" quality

---

## What's Actually Required

### Absolute Minimum to Play:
1. GameWorld scene open
2. Press Play
3. Walk around

### For Full Experience:
1. All three scenes set up with UI
2. TerrainManager in GameWorld
3. GameManager, ProfessionManager, PlayerData in MainMenu

### Optional Enhancements:
- Day/night cycle
- Weather system
- Resource nodes
- Inventory UI
- Multiplayer networking

---

## Next Steps After Basic Setup

1. **Test movement** - Walk around, jump, sprint
2. **Add resources** - Place tree/rock/plant objects
3. **Add voxel terrain** - For terraforming
4. **Setup multiplayer** - Follow UNITY_SETUP.md
5. **Create prefabs** - For reusable objects

---

## Quick Reference

### Essential Controls
- **WASD** - Move
- **Mouse** - Look
- **Space** - Jump
- **Shift** - Sprint
- **ESC** - Unlock cursor
- **I** - Inventory

### Essential Scenes
- **MainMenu** - Title screen
- **Lobby** - Profession selection
- **GameWorld** - Main gameplay

### Essential Scripts
- **GameManager** - Core game logic
- **PlayerController** - Movement
- **TerrainManager** - World generation
- **ProfessionManager** - Profession system
- **PlayerData** - Save player choices

---

## Time Estimates

- **Minimal playable**: 2 minutes (just open GameWorld)
- **Basic flow**: 10 minutes (all scenes with basic UI)
- **Full features**: 15 minutes (with day/night, weather, etc.)
- **Multiplayer**: +30 minutes (see UNITY_SETUP.md)

---

**You should be playing within 2-15 minutes depending on desired features!**

Just open GameWorld and press Play for immediate results, or follow the full setup for complete experience.
