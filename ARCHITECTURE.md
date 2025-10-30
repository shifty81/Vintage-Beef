# Architecture Overview

This document explains the technical architecture of Vintage Beef.

## High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                        Client (Unity)                        │
├─────────────────────────────────────────────────────────────┤
│  ┌───────────┐  ┌──────────┐  ┌──────────────┐            │
│  │ Main Menu │→ │  Lobby   │→ │  Game World  │            │
│  │   Scene   │  │  Scene   │  │    Scene     │            │
│  └───────────┘  └──────────┘  └──────────────┘            │
├─────────────────────────────────────────────────────────────┤
│                    Core Game Systems                         │
│  ┌──────────────┐  ┌───────────────┐  ┌──────────────┐    │
│  │   Player     │  │  Profession   │  │    World     │    │
│  │  Controller  │  │    Manager    │  │  Generator   │    │
│  └──────────────┘  └───────────────┘  └──────────────┘    │
│  ┌──────────────┐  ┌───────────────┐  ┌──────────────┐    │
│  │    Game      │  │    Dungeon    │  │   Network    │    │
│  │   Manager    │  │   Entrances   │  │   Manager    │    │
│  └──────────────┘  └───────────────┘  └──────────────┘    │
└─────────────────────────────────────────────────────────────┘
                            ↓
                   ┌────────────────┐
                   │ Unity Netcode  │
                   │ (Future: MP)   │
                   └────────────────┘
```

## Scene Flow

### 1. MainMenu Scene
- **Purpose:** Entry point for the game
- **Components:**
  - MainMenuUI: Handles menu interactions
  - Camera: Static camera for menu view
  - UI Canvas: Displays title and buttons

**Flow:**
```
Start Game
    ↓
MainMenuUI.OnPlayClicked()
    ↓
Load Lobby Scene
```

### 2. Lobby Scene
- **Purpose:** Profession selection and multiplayer lobby
- **Components:**
  - LobbyUI: Manages profession selection UI
  - ProfessionManager: Singleton containing all professions
  - PlayerData: Singleton storing selected profession

**Flow:**
```
Enter Lobby
    ↓
Display 12 Professions
    ↓
Player Selects Profession
    ↓
Store in PlayerData
    ↓
LobbyUI.OnStartGameClicked()
    ↓
Load GameWorld Scene
```

### 3. GameWorld Scene
- **Purpose:** Main gameplay area
- **Components:**
  - GameManager: Spawns player and manages game state
  - SimpleWorldGenerator: Creates terrain and dungeons
  - Player (spawned): Character with PlayerController
  - DungeonEntrances: Interactive portal objects

**Flow:**
```
Enter GameWorld
    ↓
GameManager.OnSceneLoaded()
    ↓
Spawn Player at position
    ↓
SimpleWorldGenerator creates world
    ↓
Player can explore and interact
```

## Core Systems

### Profession System

**Components:**
- `Profession`: Data class for profession info
- `ProfessionManager`: Singleton managing all professions
- `PlayerData`: Stores player's selected profession

**Design Pattern:** Singleton + Data Object

```csharp
ProfessionManager (Singleton)
    ├── availableProfessions: List<Profession>
    ├── GetAllProfessions(): List<Profession>
    └── GetProfession(index): Profession

Profession (Data Class)
    ├── professionName: string
    ├── description: string
    ├── icon: Sprite
    └── themeColor: Color

PlayerData (Singleton)
    ├── PlayerName: string
    ├── SelectedProfession: Profession
    └── SetProfession(index): void
```

### Player System

**Components:**
- `PlayerController`: Handles movement and camera
- `CharacterController`: Unity built-in physics

**Design Pattern:** Component-based

```csharp
PlayerController
    ├── Movement Parameters (speed, jump, etc.)
    ├── Camera Settings
    ├── HandleMovement(): Update loop
    └── HandleCamera(): Mouse look
```

**Features:**
- WASD movement with camera-relative direction
- Mouse look with vertical clamping
- Jump with gravity
- Sprint mode
- Cursor lock/unlock

### World Generation

**Components:**
- `SimpleWorldGenerator`: Creates game world
- Ground plane
- Dungeon entrances

**Design Pattern:** Procedural Generation (simplified)

```csharp
SimpleWorldGenerator
    ├── World Settings (size, tile size)
    ├── Dungeon Settings (count, prefabs)
    ├── GenerateWorld(): Create entire world
    ├── CreateGround(): Make terrain
    └── PlaceDungeonEntrances(): Spawn dungeons
```

### Dungeon System

**Components:**
- `DungeonEntrance`: Interactive portal

**Design Pattern:** Interaction System

```csharp
DungeonEntrance
    ├── dungeonName: string
    ├── recommendedLevel: int
    ├── maxPlayers: int
    ├── CheckPlayerProximity(): Detect player
    └── OnInteract(): Handle E key press
```

### Network System (Framework Only)

**Components:**
- `NetworkManager`: Multiplayer foundation

**Design Pattern:** Singleton (Unity Netcode ready)

```csharp
NetworkManager
    ├── maxPlayers: 12
    ├── StartHost(): Initialize server
    └── StartClient(ip): Connect to server
```

**Note:** Full implementation pending Unity Netcode integration

## Data Flow

### Scene Persistence
- Managers use `DontDestroyOnLoad()` pattern
- Three singletons persist across scenes:
  - `ProfessionManager`
  - `PlayerData`
  - `GameManager`

### Profession Selection Flow
```
User clicks profession button
    ↓
LobbyUI.OnProfessionSelected(index)
    ↓
PlayerData.SetProfession(index)
    ↓
ProfessionManager.GetProfession(index)
    ↓
Store Profession in PlayerData
    ↓
Enable Start Game button
```

### Player Spawn Flow
```
GameWorld scene loads
    ↓
GameManager.OnSceneLoaded()
    ↓
GameManager.SpawnPlayer()
    ↓
Create Player GameObject
    ↓
Add CharacterController
    ↓
Add PlayerController
    ↓
Create Camera as child
    ↓
Player ready for input
```

## Design Patterns Used

### 1. Singleton Pattern
- **Where:** GameManager, ProfessionManager, PlayerData, NetworkManager
- **Why:** Single instance needed across scenes
- **Thread-Safe:** Yes (Unity main thread only)

### 2. Component Pattern
- **Where:** PlayerController, DungeonEntrance
- **Why:** Unity's component-based architecture
- **Benefits:** Reusable, modular, inspector-editable

### 3. Observer Pattern (Unity Events)
- **Where:** UI button clicks, scene loading
- **Why:** Decouple UI from logic
- **Implementation:** Unity's UnityEvent system

### 4. Factory Pattern (Implicit)
- **Where:** GameManager spawning player, WorldGenerator creating dungeons
- **Why:** Centralized object creation
- **Benefits:** Easy to modify spawn logic

## Performance Considerations

### Optimization Strategies

1. **Object Pooling (Future)**
   - For frequently spawned objects (projectiles, particles)
   - Reduce GC pressure

2. **LOD System (Future)**
   - Multiple detail levels for models
   - Switch based on camera distance

3. **Occlusion Culling**
   - Don't render what camera can't see
   - Important for dungeons

4. **Texture Atlasing**
   - Combine multiple textures
   - Reduce draw calls

5. **Simple Shaders**
   - Stylized art = simpler shaders
   - Better performance on low-end hardware

### Current Performance Profile
- Draw calls: < 50 (very low, mostly primitives)
- Triangles: < 100k (simple geometry)
- Batching: Dynamic batching enabled
- Target: 60 FPS on GTX 1050 / equivalent

## Scalability

### Network Architecture (Planned)

```
Host-Client Model (Unity Netcode)

Host (Server + Client)
    ├── Authoritative game state
    ├── Handles all 12 players
    ├── Manages dungeon instances
    └── Synchronizes world state

Clients (11 other players)
    ├── Send input to host
    ├── Receive state updates
    ├── Client-side prediction
    └── Interpolation for smooth movement
```

### World Scaling
- Chunk-based loading (future)
- Dynamic entity streaming
- Server-authoritative dungeons
- Separate dungeon instances per group

## Extension Points

Areas designed for future expansion:

1. **Profession System**
   - Add abilities to Profession class
   - Implement skill trees
   - Add progression system

2. **Crafting System**
   - Recipe database
   - Item system
   - Resource nodes

3. **Dungeon System**
   - Procedural generation
   - Enemy AI
   - Loot tables

4. **Networking**
   - Full Netcode integration
   - Lobby browser
   - Matchmaking

## File Organization

```
Assets/
├── Scenes/              # Unity scene files
├── Scripts/             # C# game logic
│   ├── UI/             # (Future) UI-specific
│   ├── Network/        # (Future) Networking
│   ├── Gameplay/       # (Future) Game mechanics
│   └── Core/           # (Future) Core systems
├── Prefabs/            # Reusable game objects
├── Materials/          # Visual materials
└── Resources/          # Runtime-loaded assets

ProjectSettings/        # Unity project config
Packages/              # Package dependencies
```

## Testing Strategy

### Unit Testing (Future)
- Test profession logic
- Test world generation algorithms
- Test networking synchronization

### Integration Testing
- Scene transitions
- Player spawn flow
- UI interactions

### Performance Testing
- FPS benchmarks
- Memory profiling
- Network latency tests

## Future Architecture Changes

### Phase 2: Networking
- Integrate Unity Netcode for GameObjects
- Add NetworkBehaviour to core components
- Implement client-server synchronization
- Add lobby connection system

### Phase 3: Gameplay Systems
- Inventory system with grid-based UI
- Crafting system with recipes
- Resource nodes with respawn timers
- Quest system with objectives

### Phase 4: Content Pipeline
- Addressables for asset streaming
- Modding support (potential)
- Asset bundles for DLC
- Save system for persistence

## Security Considerations

### Current (Localhost)
- No security needed (single player testing)

### Future (Multiplayer)
- Server-authoritative gameplay
- Input validation
- Anti-cheat measures
- Secure player authentication

## Dependencies

### Unity Packages
- **Unity Netcode for GameObjects** (1.7.0): Multiplayer
- **TextMeshPro** (3.0.6): UI text rendering
- **Timeline** (1.7.5): Cutscenes (future)
- **Visual Scripting** (1.9.0): Designer tools (optional)

### External Dependencies
- None currently

## Conclusion

The current architecture provides a solid foundation for:
- Scalable multiplayer (12 players)
- Modular profession system
- Extensible dungeon system
- Performance-optimized rendering

Next steps focus on implementing full networking and expanding gameplay systems while maintaining the Palia-inspired performance profile.
