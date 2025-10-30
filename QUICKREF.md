# Quick Reference Guide

Quick reference for common tasks and information in Vintage Beef.

## 🚀 Getting Started (5 Minutes)

1. Clone: `git clone https://github.com/shifty81/Vintage-Beef.git`
2. Open Unity Hub → Add Project → Select folder
3. Open with Unity 2022.3.10f1+
4. Open MainMenu scene
5. Press Play!

## 🎮 Controls

| Action | Key |
|--------|-----|
| Move | WASD |
| Look | Mouse |
| Jump | Space |
| Sprint | Left Shift |
| Interact with Dungeon | E |
| Unlock Cursor | ESC |
| **Open Chat** | **Enter** |

## 👥 The 12 Professions

| # | Profession | Description |
|---|------------|-------------|
| 1 | Farmer | Grow crops and tend to animals |
| 2 | Blacksmith | Forge tools and weapons |
| 3 | Builder | Construct buildings and structures |
| 4 | Miner | Extract resources from the earth |
| 5 | Hunter | Track and hunt wildlife |
| 6 | Cook | Prepare food and meals |
| 7 | Tailor | Craft clothing and armor |
| 8 | Merchant | Trade goods and resources |
| 9 | Explorer | Discover new lands and dungeons |
| 10 | Engineer | Build machines and contraptions |
| 11 | Alchemist | Create potions and elixirs |
| 12 | Woodworker | Craft items from wood |

## 📁 Project Structure

```
Vintage-Beef/
├── Assets/
│   ├── Scenes/        # 3 scenes (MainMenu, Lobby, GameWorld)
│   ├── Scripts/       # 9 C# scripts
│   ├── Prefabs/       # Future prefabs
│   └── Materials/     # Future materials
├── ProjectSettings/   # Unity configuration
├── Packages/          # Unity package dependencies
└── Documentation/     # All .md files in root
```

## 🎯 Core Scripts

| Script | Purpose |
|--------|---------|
| **Core Systems** |
| `ProfessionManager.cs` | Manages all 12 professions |
| `PlayerData.cs` | Stores player's selected profession |
| `PlayerController.cs` | Handles movement and camera |
| `GameManager.cs` | Controls game state and spawning |
| **UI Systems** |
| `MainMenuUI.cs` | Main menu logic |
| `LobbyUI.cs` | Profession selection UI |
| `ConnectionUI.cs` | **v0.2.0** - Multiplayer lobby |
| `InventoryUI.cs` | **v0.3.0** - Inventory display |
| **World Generation** |
| `SimpleWorldGenerator.cs` | Basic world generation |
| `ProceduralWorldGenerator.cs` | **v0.3.0** - Advanced terrain & biomes |
| `DayNightCycle.cs` | **v0.3.0** - Time of day system |
| `WeatherSystem.cs` | **v0.3.0** - Weather effects |
| **Gameplay** |
| `DungeonEntrance.cs` | Dungeon interaction points |
| `ResourceNode.cs` | **v0.3.0** - Gatherable resources |
| `PlayerInventory.cs` | **v0.3.0** - Inventory system |
| **Networking** |
| `NetworkManager.cs` | **v0.2.0** - Multiplayer networking |
| `NetworkPlayer.cs` | **v0.2.0** - Network player sync |
| `ChatManager.cs` | **v0.2.0** - In-game chat |
| `Billboard.cs` | **v0.2.0** - Name tag rendering |

## 🔧 Common Unity Operations

### Opening a Scene
1. Project window → Assets/Scenes
2. Double-click desired scene

### Running the Game
1. Open MainMenu scene
2. Click Play button (or Ctrl+P)

### Building the Game
1. File → Build Settings
2. Select platform
3. Click Build

### Checking Console
1. Window → General → Console (or Ctrl+Shift+C)
2. View errors, warnings, and logs

## 📊 Scene Flow

```
MainMenu.unity
    ↓ (Click "Play")
Lobby.unity
    ↓ (Select Profession + Click "Start Game")
GameWorld.unity
    ↓ (Explore and interact)
```

## 🐛 Troubleshooting Quick Fixes

| Issue | Solution |
|-------|----------|
| Scripts won't compile | Assets → Reimport All |
| Scene not loading | File → Build Settings → Add scene |
| Player not spawning | Check GameManager exists in scene |
| UI not working | Check Canvas and EventSystem exist |
| Camera not moving | Check PlayerController is on Player |

## 📝 Documentation Map

| Document | When to Read |
|----------|--------------|
| **README.md** | First - Project overview |
| **QUICKREF.md** | Quick reference (this file!) |
| **SETUP.md** | Setting up development environment |
| **UNITY_SETUP.md** | **NEW v0.2.0** - Setting up multiplayer in Unity |
| **MULTIPLAYER.md** | **NEW v0.2.0** - Multiplayer guide |
| **TESTING.md** | Before testing features |
| **CONTRIBUTING.md** | Before making contributions |
| **ARCHITECTURE.md** | Understanding technical design |
| **ROADMAP.md** | Planning future work |

## 🔍 Finding Things

### Find a Script
1. Project window search bar
2. Type script name
3. Double-click to open

### Find an Object in Scene
1. Hierarchy window search bar
2. Type object name
3. Click to select

### Find References
1. Right-click asset in Project
2. Select "Find References In Scene"

## ⚡ Performance Tips

| Setting | Impact | Where |
|---------|--------|-------|
| Quality Level | FPS | Edit → Project Settings → Quality |
| VSync | Input lag | Quality Settings |
| Shadow Distance | FPS | Quality Settings |
| Anti-Aliasing | Visual quality | Quality Settings |

**Quick Performance Check:**
- Game view → Stats button
- Look for Draw Calls (target: <100)
- Look for FPS (target: 60+)

## 🎨 Palia-Style Guidelines

✅ **Do:**
- Stylized, hand-painted look
- Warm, inviting colors
- Simple, readable shapes
- Optimize for performance

❌ **Don't:**
- Photorealistic textures
- High poly counts
- Complex shaders
- Heavy post-processing

## 🌐 Multiplayer (v0.2.0)

**Current State:** ✅ Core implementation complete!

**Features Implemented:**
- ✅ Host/Join system with UI
- ✅ 12 player support
- ✅ Player name synchronization
- ✅ Position synchronization
- ✅ Chat system with commands
- ✅ System messages

**How to Use:**
1. **Host:** Enter name → Click "Host" → Select profession
2. **Join:** Enter name → Enter IP → Click "Join" → Select profession
3. **Chat:** Press Enter to open, type message, press Enter to send

**Chat Commands:**
- `/help` - Show available commands
- `/players` - Show connected players count
- `/time` - Show server time

**Next Steps:**
- Follow UNITY_SETUP.md to configure scenes in Unity Editor
- See MULTIPLAYER.md for detailed guide

**Testing:**
- Build game once
- Run host in build, client in Unity Editor (or vice versa)
- Test on same machine with IP: 127.0.0.1

## 📦 Required Unity Packages

| Package | Version | Purpose |
|---------|---------|---------|
| Unity Netcode | 1.7.0 | Multiplayer |
| TextMeshPro | 3.0.6 | UI Text |
| Timeline | 1.7.5 | Cutscenes (future) |

## 🎓 Learning Resources

### Unity Basics
- Unity Learn: learn.unity.com
- Unity Docs: docs.unity3d.com

### Multiplayer
- Unity Netcode Docs
- MLAPI Community Contributions

### Game Design
- Vintage Story Wiki (inspiration)
- Palia Design Pillars (visual reference)

## 🤝 Community

- **Issues:** GitHub Issues page
- **Discussions:** GitHub Discussions
- **Contributing:** See CONTRIBUTING.md
- **Testing:** See TESTING.md

## 📈 Version History

| Version | Status | Features |
|---------|--------|----------|
| v0.1.0 | ✅ Complete | Foundation, basic systems |
| v0.2.0 | ✅ Complete | Multiplayer networking, chat |
| v0.3.0 | ✅ Complete | World generation, biomes, inventory, day/night, weather |
| v0.4.0 | 📋 Planned | Crafting & professions |

## 🔗 Quick Links

- [Full README](README.md)
- [Setup Guide](SETUP.md)
- [Unity Setup Guide](UNITY_SETUP.md) ⭐ NEW
- [Multiplayer Guide](MULTIPLAYER.md) ⭐ NEW
- [Testing Guide](TESTING.md)
- [Contribution Guide](CONTRIBUTING.md)
- [Architecture](ARCHITECTURE.md)
- [Roadmap](ROADMAP.md)

## 💡 Pro Tips

1. **Save Often:** Ctrl+S
2. **Use Prefabs:** For reusable objects
3. **Check Console:** Errors show in red
4. **Test Early:** Run game frequently
5. **Read Docs:** When stuck, check documentation
6. **Ask Questions:** Use GitHub Discussions

## 🎯 Next Steps

**For New Developers:**
1. ✅ Read README.md
2. ✅ Follow SETUP.md
3. ✅ Run TESTING.md tests
4. → Read ARCHITECTURE.md
5. → Pick a task from ROADMAP.md
6. → Follow CONTRIBUTING.md

**For Testers:**
1. ✅ Follow SETUP.md
2. ✅ Run TESTING.md
3. → Report bugs on GitHub
4. → Suggest features

**For Contributors:**
1. ✅ All of the above
2. → Check open issues
3. → Fork and create branch
4. → Make changes
5. → Submit PR

---

**Last Updated:** v0.2.0

**Quick Help:** For issues, check Console first, then see TROUBLESHOOTING section above, then create GitHub issue.
