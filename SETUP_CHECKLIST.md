# Vintage Beef - Setup Checklist

This is a quick checklist to get Vintage Beef playable. Follow in order for best results.

## ✅ Checklist

### Essential Setup (Required to Play)

- [ ] **1. Open Unity Project**
  - Open Unity Hub
  - Add project from disk
  - Select Vintage-Beef folder
  - Open with Unity 2022.3.10f1 or later
  - Wait for initial import (2-3 minutes)

- [ ] **2. Quick Play Test**
  - Open `Assets/Scenes/GameWorld.unity`
  - Press Play button (▶)
  - Verify you can move around (WASD, Mouse, Space, Shift)
  - If working: **Game is playable!** Continue for more features.

### Scene Setup (For Full Experience)

- [ ] **3. Use Scene Setup Helper** (RECOMMENDED - Fast!)
  - In Unity menu: `Vintage Beef → Scene Setup Helper`
  - Click "Setup MainMenu Scene" (have MainMenu scene open)
  - Click "Setup Lobby Scene" (have Lobby scene open)
  - Click "Setup GameWorld Scene" (have GameWorld scene open)
  - Done! Skip to step 5.

**OR** Do manual setup (if helper doesn't work):

- [ ] **4. Manual Scene Setup** (Alternative to step 3)
  
  **MainMenu Scene:**
  - Create Canvas (Right-click Hierarchy → UI → Canvas)
  - Create title text: "VINTAGE BEEF"
  - Create "PLAY" button
  - Create "QUIT" button
  - Add MainMenuUI script and link buttons
  - Create GameManager, ProfessionManager, PlayerData objects
  
  **Lobby Scene:**
  - Create Canvas
  - Create title: "SELECT YOUR PROFESSION"
  - Create profession container
  - Create "START GAME" button (disabled)
  - Add LobbyUI script and link elements
  
  **GameWorld Scene:**
  - Create Environment with Sun and Moon lights
  - Add DayNightCycle component
  - Add WeatherSystem component
  - Create TerrainSystem with TerrainManager
  - Create Canvas with InventoryPanel
  - Add InventoryUI script

- [ ] **5. Configure Build Settings**
  - File → Build Settings
  - Add scenes in order: MainMenu, Lobby, GameWorld
  - Ensure all are checked
  - Close window

### Testing

- [ ] **6. Test Full Flow**
  - Open MainMenu scene
  - Press Play
  - Click PLAY → select profession → START GAME
  - Verify you spawn and can play
  - Press I for inventory
  - Walk around and explore

### Optional Enhancements

- [ ] **7. Choose Terrain Type** (In GameWorld TerrainManager)
  - Simple: Flat plane (fastest)
  - Procedural: Hills and biomes (pretty)
  - Voxel: Terraformable (full features)

- [ ] **8. Add Resources** (Optional)
  - See PLAY_NOW.md for resource node setup
  - Place trees, rocks, plants in GameWorld

- [ ] **9. Setup Multiplayer** (Optional)
  - See UNITY_SETUP.md for detailed multiplayer setup
  - Create NetworkPlayer prefab
  - Configure NetworkManager

## 🎮 Controls

Once playing, use these controls:

| Action | Key/Button |
|--------|-----------|
| Move | WASD |
| Look | Mouse |
| Jump | Space |
| Sprint | Left Shift |
| Unlock Cursor | ESC |
| Inventory | I |
| Interact | E |
| Remove Voxel | Left Mouse (if voxel terrain) |
| Place Voxel | Right Mouse (if voxel terrain) |

## 📋 Quick Status Check

**Is it playable?**
- ✅ Can open GameWorld and press Play → **YES, basic gameplay works**
- ✅ Can move around with WASD → **YES, core mechanics work**
- ⚠️ Want menu flow → Need scene setup (steps 3-5)
- ⚠️ Want inventory UI → Need GameWorld UI setup
- ⚠️ Want multiplayer → Need additional setup (see UNITY_SETUP.md)

## 🔧 Troubleshooting

**Problem:** Can't move
- Solution: Click in Game window, press ESC to lock cursor

**Problem:** No terrain
- Solution: Wait 2-3 seconds, check Console, select Simple terrain type

**Problem:** Scene won't load
- Solution: Add scenes to Build Settings (File → Build Settings)

**Problem:** Scripts won't compile
- Solution: Assets → Reimport All, restart Unity

**Problem:** Buttons don't work
- Solution: Ensure Canvas and EventSystem exist in scene

## 📚 Documentation

- **PLAY_NOW.md** - Detailed step-by-step setup guide
- **README.md** - Project overview and features
- **QUICKSTART.md** - Quick reference for getting started
- **UNITY_SETUP.md** - Multiplayer networking setup
- **VOXEL_QUICKSTART.md** - Voxel terrain setup and usage

## ⏱️ Time Estimates

- Minimal playable (GameWorld only): **2 minutes**
- Full scene setup with helper: **5 minutes**
- Full scene setup manually: **15 minutes**
- With all enhancements: **20 minutes**
- With multiplayer: **45 minutes**

## ✨ What Works Right Now

- ✅ Player movement and controls
- ✅ Camera system
- ✅ Terrain generation (3 types)
- ✅ Day/night cycle
- ✅ Weather system
- ✅ Inventory system
- ✅ Resource gathering
- ✅ Voxel terraforming
- ✅ Scene transitions
- ✅ Profession selection

## 🚀 Next Steps After Playing

1. Test all terrain types
2. Experiment with day/night and weather
3. Try voxel terraforming
4. Set up multiplayer for co-op play
5. Add custom resources and content

---

**Ready to play?** Open GameWorld scene and press Play! 🎮
