# Unity Setup Guide for Multiplayer

This guide walks you through setting up the Unity scenes and prefabs for multiplayer functionality.

## Prerequisites

- Unity 2022.3.10f1 or later installed
- Project opened in Unity Editor
- All scripts compiled without errors

## Step 1: Create Network Player Prefab

1. **Create a new GameObject in the Hierarchy:**
   - Right-click in Hierarchy → Create Empty
   - Name it "NetworkPlayer"

2. **Add Required Components:**
   - Select NetworkPlayer
   - Add Component → Character Controller
     - Set Height: 2
     - Set Radius: 0.5
   - Add Component → Network Object (Unity.Netcode)
   - Add Component → Network Transform (Unity.Netcode.Components)
   - Add Component → Network Player (VintageBeef.Network)
   - Add Component → Player Controller (VintageBeef)

3. **Create Player Camera:**
   - Right-click NetworkPlayer → Create Empty
   - Name it "PlayerCamera"
   - Set Position: (0, 1.6, 0)
   - Add Component → Camera
   - Add Component → Audio Listener

4. **Configure Components:**
   - NetworkObject:
     - Check "Is Player Object"
   - NetworkTransform:
     - Check "Synchronize Position"
     - Check "Synchronize Rotation"
     - Interpolate: true
   - PlayerController:
     - Drag PlayerCamera into "Camera Transform" field

5. **Create Prefab:**
   - Drag NetworkPlayer from Hierarchy to Assets/Prefabs folder
   - Delete NetworkPlayer from Hierarchy (we only need the prefab)

## Step 2: Setup Network Manager GameObject

1. **Create Network Manager:**
   - In Hierarchy, create Empty GameObject
   - Name it "NetworkManager"
   - Add Component → Vintage Beef Network Manager (VintageBeef.Network)
   - Add Component → Unity Transport (Unity.Netcode.Transports.UTP)

2. **Configure Network Manager:**
   - The VintageBeefNetworkManager script will automatically add Unity's NetworkManager
   - In Unity NetworkManager component:
     - Set "Player Prefab" to NetworkPlayer prefab from Assets/Prefabs
     - Check "Enable Scene Management"

3. **Move to DontDestroyOnLoad:**
   - The script handles this automatically
   - NetworkManager will persist across scenes

## Step 3: Update Lobby Scene

1. **Open Lobby Scene:**
   - Double-click Assets/Scenes/Lobby.unity

2. **Create Connection UI Panel:**
   - Right-click in Hierarchy → UI → Canvas (if not exists)
   - Right-click Canvas → Create Empty
   - Name it "ConnectionPanel"

3. **Add Connection UI Elements:**
   
   **Username Input:**
   - Right-click ConnectionPanel → UI → Input Field - TextMeshPro
   - Name it "UsernameInput"
   - Position it at the top of screen
   - Set placeholder text: "Enter your name..."
   
   **IP Address Input:**
   - Right-click ConnectionPanel → UI → Input Field - TextMeshPro
   - Name it "IPAddressInput"
   - Position below username
   - Set placeholder text: "127.0.0.1"
   
   **Host Button:**
   - Right-click ConnectionPanel → UI → Button - TextMeshPro
   - Name it "HostButton"
   - Set button text: "Host Game"
   - Position below IP input
   
   **Join Button:**
   - Right-click ConnectionPanel → UI → Button - TextMeshPro
   - Name it "JoinButton"
   - Set button text: "Join Game"
   - Position next to Host button
   
   **Status Text:**
   - Right-click ConnectionPanel → UI → Text - TextMeshPro
   - Name it "StatusText"
   - Position at bottom
   - Set text: "Ready to connect"

4. **Create Profession Selection Panel:**
   - Right-click Canvas → Create Empty
   - Name it "ProfessionPanel"
   - This should already exist from v0.1.0
   - Make sure it's initially disabled in Inspector

5. **Add ConnectionUI Script:**
   - Create Empty GameObject under Canvas
   - Name it "ConnectionUIManager"
   - Add Component → Connection UI (VintageBeef.UI)
   - Drag UI elements into corresponding fields:
     - Username Input → UsernameInput
     - IP Address Input → IPAddressInput
     - Host Button → HostButton
     - Join Button → JoinButton
     - Status Text → StatusText
     - Connection Panel → ConnectionPanel
     - Profession Panel → ProfessionPanel

6. **Save Scene:**
   - File → Save or Ctrl+S

## Step 4: Update GameWorld Scene

1. **Open GameWorld Scene:**
   - Double-click Assets/Scenes/GameWorld.unity

2. **Add Chat UI:**
   
   **Create Chat Panel:**
   - Right-click Canvas → Create Empty
   - Name it "ChatPanel"
   - Set it to inactive (uncheck in Inspector)
   - Anchor to bottom-left
   
   **Chat Display:**
   - Right-click ChatPanel → UI → Text - TextMeshPro
   - Name it "ChatDisplay"
   - Make it scrollable:
     - Add Component → Scroll Rect
     - Create child: Viewport
     - Create child of Viewport: Content
     - Move ChatDisplay into Content
   
   **Message Input:**
   - Right-click ChatPanel → UI → Input Field - TextMeshPro
   - Name it "MessageInput"
   - Position at bottom of chat panel
   - Set placeholder: "Press Enter to chat..."

3. **Add Chat Manager:**
   - Create Empty GameObject
   - Name it "ChatManager"
   - Add Component → Network Object
   - Add Component → Chat Manager (VintageBeef.Network)
   - Drag UI elements:
     - Chat Panel → ChatPanel
     - Message Input → MessageInput
     - Chat Display → ChatDisplay
     - Scroll Rect → (the ScrollRect component)

4. **Save Scene:**
   - File → Save

## Step 5: Update GameManager

1. **Find or Create GameManager:**
   - Should exist in MainMenu scene
   - If not, create in MainMenu scene

2. **Configure GameManager:**
   - Select GameManager GameObject
   - In Game Manager component:
     - Player Prefab: Leave empty (for single-player fallback)
     - Network Player Prefab: NetworkPlayer (from Assets/Prefabs)
     - Spawn Position: (0, 2, 0)
     - Spawn Radius: 3

3. **Save Scene**

## Step 6: Configure Build Settings

1. **Open Build Settings:**
   - File → Build Settings

2. **Add Scenes in Order:**
   - MainMenu
   - Lobby
   - GameWorld
   - Ensure all are checked

3. **Platform Settings:**
   - Select your target platform (Windows/Mac/Linux)

## Step 7: Test Multiplayer

### Single Instance Test (Basic)

1. Press Play in Unity Editor
2. Click "Play" in main menu
3. Enter a username
4. Click "Host"
5. Select a profession
6. Click "Start Game"
7. Verify you spawn in world

### Two Instance Test (Full Multiplayer)

**First - Build the game:**
1. File → Build Settings
2. Click "Build"
3. Choose output folder
4. Wait for build to complete

**Then test:**
1. Run built executable
   - Click Play → enter name → click "Host"
   - Select profession → Start Game

2. In Unity Editor, press Play
   - Click Play → enter different name
   - Enter IP: 127.0.0.1
   - Click "Join"
   - Select profession → Start Game

3. Both players should see each other!

## Troubleshooting

### "NetworkManager not found"
- Ensure NetworkManager GameObject exists in MainMenu scene
- Check that VintageBeefNetworkManager component is attached

### "Player prefab not spawning"
- Verify NetworkPlayer prefab has NetworkObject component
- Check "Is Player Object" is enabled on NetworkObject
- Ensure prefab is assigned in Unity's NetworkManager

### "Can't see other players"
- Check NetworkTransform is on player prefab
- Verify both instances are connected (check console logs)
- Make sure NetworkPlayer script is attached

### "Chat not working"
- Ensure ChatManager has NetworkObject component
- Check UI elements are properly assigned
- Verify ChatManager exists in GameWorld scene

### Scripts won't compile
- Check all using statements are correct
- Verify Unity Netcode package is installed
- Clean and rebuild: Assets → Reimport All

## Next Steps

After setup is complete:
1. Test with 2 players
2. Test with more players (build multiple executables)
3. Test over network (not just localhost)
4. Report any issues
5. Continue to Phase 2 features!

## Visual Layout Reference

### Lobby Scene Hierarchy
```
Canvas
├── ConnectionPanel
│   ├── UsernameInput
│   ├── IPAddressInput
│   ├── HostButton
│   ├── JoinButton
│   └── StatusText
├── ProfessionPanel
│   ├── TitleText
│   ├── ProfessionButtons (container)
│   └── StartGameButton
└── ConnectionUIManager
```

### GameWorld Scene Hierarchy
```
Environment
├── Terrain
├── DungeonEntrances
└── Lighting

Canvas
└── ChatPanel
    ├── ChatDisplay (in ScrollRect)
    └── MessageInput

Managers
├── GameManager (from MainMenu scene)
├── NetworkManager (from MainMenu scene)
└── ChatManager
```

## Tips

- Always test basic functionality before multiplayer
- Keep console open to see network logs
- Use Development Build for better debugging
- Test with firewall temporarily disabled
- Start with localhost before network testing
- Back up your project before major changes

Happy multiplayer gaming!
