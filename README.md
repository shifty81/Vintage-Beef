# Vintage Beef

A Unity multiplayer game inspired by Vintage Story mechanics with added dungeons, featuring Palia-style graphics for broad hardware compatibility.

## Overview

Vintage Beef is a 12-player cooperative multiplayer game that combines:
- **Vintage Story-inspired mechanics**: Survival, crafting, and profession systems
- **Dungeon exploration**: Discover and enter dungeons scattered across the map
- **12 Professions**: Each profession specializes in different aspects of gameplay
- **Palia-style graphics**: Stylized visuals optimized for lower-end hardware

## Current Features (v0.3.0 - In Development)

### Implemented
- ✅ Main Menu system
- ✅ Lobby with profession selection
- ✅ 12 unique professions (Farmer, Blacksmith, Builder, Miner, Hunter, Cook, Tailor, Merchant, Explorer, Engineer, Alchemist, Woodworker)
- ✅ Player movement and camera controls
- ✅ **Procedural world generation with biomes**
- ✅ **Multiple biomes (Forest, Plains, Desert, Mountains)**
- ✅ **Resource nodes (Trees, Rocks, Plants)**
- ✅ **Gathering mechanics with respawn**
- ✅ **Inventory system with stacking**
- ✅ **Inventory UI**
- ✅ **Day/night cycle with dynamic lighting**
- ✅ **Weather system (Clear, Rain, Foggy)**
- ✅ Dungeon entrance system
- ✅ Multiplayer networking (Unity Netcode for GameObjects)
- ✅ Host/Join lobby system
- ✅ Player name synchronization
- ✅ Support for up to 12 players
- ✅ Chat system

### In Development
- 🔄 Profession-specific abilities and mechanics
- 🔄 Dungeon instances and content
- 🔄 Crafting system
- 🔄 Advanced inventory management
- 🔄 Network synchronization for world systems

## Requirements

- Unity 2022.3.10f1 or later
- Unity Netcode for GameObjects package (included in manifest)
- TextMeshPro (included in manifest)

## Getting Started

### Opening the Project

1. Clone this repository
2. Open Unity Hub
3. Click "Add" and navigate to the cloned repository folder
4. Open the project with Unity 2022.3.10f1 or later

### Building the Project

1. Open Unity Editor
2. Go to `File > Build Settings`
3. Ensure all three scenes are added and enabled:
   - MainMenu
   - Lobby
   - GameWorld
4. Select your target platform
5. Click "Build" or "Build and Run"

### Testing

#### Basic Flow Test
1. Press Play in Unity Editor
2. Click "Play" button in Main Menu
3. Select a profession from the 12 available options
4. Click "Start Game"
5. You will spawn in the game world
6. Use WASD to move, Mouse to look around, Space to jump, Shift to sprint
7. Find purple cube markers - these are dungeon entrances
8. Approach a dungeon entrance and press 'E' to interact

#### Controls
- **Movement**: WASD
- **Look**: Mouse
- **Jump**: Space
- **Sprint**: Left Shift
- **Interact/Gather**: E (when near resource or dungeon)
- **Open Inventory**: I
- **Open Chat**: Enter
- **Toggle Cursor**: ESC

## Multiplayer (New in v0.2.0!)

Vintage Beef now supports multiplayer for up to 12 players using Unity Netcode for GameObjects.

### Quick Start - Multiplayer

1. **Host a Game:**
   - Launch the game
   - Click "Play" in main menu
   - Enter your username
   - Click "Host" button
   - Select your profession
   - Click "Start Game"
   - Share your IP address with friends

2. **Join a Game:**
   - Launch the game
   - Click "Play" in main menu
   - Enter your username
   - Enter the host's IP address
   - Click "Join" button
   - Select your profession
   - Click "Start Game"

For detailed multiplayer setup and troubleshooting, see [MULTIPLAYER.md](MULTIPLAYER.md).

## Project Structure

```
Assets/
├── Scenes/
│   ├── MainMenu.unity      # Main menu scene
│   ├── Lobby.unity         # Profession selection & multiplayer lobby
│   └── GameWorld.unity     # Main game world
├── Scripts/
│   ├── ProfessionManager.cs           # Manages all professions
│   ├── PlayerData.cs                  # Player persistent data
│   ├── PlayerController.cs            # Player movement and camera
│   ├── GameManager.cs                 # Main game state manager
│   ├── MainMenuUI.cs                  # Main menu UI logic
│   ├── LobbyUI.cs                     # Lobby UI and profession selection
│   ├── ConnectionUI.cs                # Multiplayer connection UI (NEW v0.2.0)
│   ├── DungeonEntrance.cs             # Dungeon entrance interaction
│   ├── SimpleWorldGenerator.cs        # World generation
│   ├── NetworkManager.cs              # Multiplayer networking (UPDATED v0.2.0)
│   ├── NetworkPlayer.cs               # Network player component (NEW v0.2.0)
│   └── Billboard.cs                   # Name tag billboard (NEW v0.2.0)
├── Prefabs/                # Game object prefabs (to be added)
└── Materials/              # Materials and textures (to be added)
```

## The 12 Professions

1. **Farmer** - Grow crops and tend to animals
2. **Blacksmith** - Forge tools and weapons
3. **Builder** - Construct buildings and structures
4. **Miner** - Extract resources from the earth
5. **Hunter** - Track and hunt wildlife
6. **Cook** - Prepare food and meals
7. **Tailor** - Craft clothing and armor
8. **Merchant** - Trade goods and resources
9. **Explorer** - Discover new lands and dungeons
10. **Engineer** - Build machines and contraptions
11. **Alchemist** - Create potions and elixirs
12. **Woodworker** - Craft items from wood

## Development Roadmap

### Phase 1: Core Systems (In Progress - v0.2.0)
- [x] Project setup and structure
- [x] Basic player movement
- [x] Profession system foundation
- [x] Menu and lobby systems
- [x] Basic world generation
- [x] Multiplayer networking core (Host/Join, player sync)
- [ ] Complete multiplayer (prefabs, scenes, testing)
- [ ] Basic chat system

### Phase 2: Gameplay Mechanics
- [ ] Resource gathering system
- [ ] Crafting system
- [ ] Inventory management
- [ ] Profession-specific abilities
- [ ] Building system

### Phase 3: World Content
- [ ] Advanced world generation
- [ ] Dungeon generation and content
- [ ] NPC system
- [ ] Quest system
- [ ] Weather and day/night cycle

### Phase 4: Polish & Balance
- [ ] Palia-style art assets
- [ ] Sound effects and music
- [ ] UI/UX improvements
- [ ] Performance optimization
- [ ] Balancing and testing

## Contributing

This is an early-stage project. More contribution guidelines will be added as the project develops.

## Graphics Philosophy

Following Palia's approach, Vintage Beef aims for:
- Stylized, appealing visuals rather than photorealism
- Optimized performance for lower-end GPUs
- Clear, readable art style
- Warm, inviting color palette
- Efficient use of resources (textures, polygons, shaders)

## License

[License information to be added]

## Contact

[Contact information to be added]
