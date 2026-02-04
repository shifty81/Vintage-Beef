# NetworkPlayer Prefab Setup Guide

This guide explains how to create and configure the NetworkPlayer prefab for multiplayer functionality.

## Quick Setup (Recommended)

### Using the Prefab Creation Helper Tool

1. **Open Unity Editor**
   - Open the Vintage Beef project in Unity

2. **Run the Prefab Creation Helper**
   - Unity menu bar → `Vintage Beef` → `Prefab Creation Helper`
   - Click the "Create NetworkPlayer Prefab" button
   - Wait for confirmation dialog

3. **Configure the Prefab**
   - The prefab will be automatically selected in the Project window
   - In the Inspector, find the **NetworkObject** component
   - **IMPORTANT:** Check the box for **"Is Player Object"**
   - Save the prefab (Ctrl+S or Cmd+S)

4. **Add to NetworkManager**
   - See [UNITY_SETUP.md](UNITY_SETUP.md) for complete multiplayer setup
   - In brief: Add the NetworkPlayer prefab to the NetworkManager's player prefab list

## What the Tool Creates

The Prefab Creation Helper automatically creates a NetworkPlayer prefab with:

### Components
- **CharacterController** - Physics-based character movement
  - Height: 2m
  - Radius: 0.5m
  - Center: (0, 1, 0)

- **NetworkObject** - Unity Netcode network synchronization
  - Note: You must manually set "Is Player Object" to TRUE

- **NetworkTransform** - Position and rotation synchronization
  - Syncs X, Y, Z position
  - Syncs Y rotation (horizontal turning)

- **NetworkPlayer** - Custom script for player name and profession sync
  - Handles player name display
  - Syncs profession selection
  - Creates floating name label above player

- **PlayerController** - Movement and camera control
  - WASD movement
  - Mouse look
  - Jump and sprint
  - Linked to camera automatically

- **PlayerInventory** - Inventory system integration

- **PlayerInteraction** - Resource gathering and interaction

### Child Objects
- **PlayerCamera** - First-person camera
  - Position: (0, 1.6, 0) - eye height
  - Camera component (60° FOV)
  - AudioListener component
  - Automatically linked to PlayerController

- **PlayerVisual** - Temporary visual representation
  - Blue capsule mesh (placeholder)
  - Can be replaced with custom character model later

## Manual Setup (Alternative)

If you prefer to create the prefab manually, follow these detailed steps:

### 1. Create Base GameObject
```
Hierarchy → Right-click → Create Empty
Name: "NetworkPlayer"
```

### 2. Add Required Components
Add these components in order:
1. CharacterController
   - Height: 2
   - Radius: 0.5
   - Center: (0, 1, 0)

2. NetworkObject
   - Check "Is Player Object"

3. NetworkTransform
   - Check "Synchronize Position"
   - Check "Synchronize Rotation"

4. NetworkPlayer script

5. PlayerController script
   - Leave disabled (NetworkPlayer will enable it for owner)

6. PlayerInventory script

7. PlayerInteraction script

### 3. Create Camera Child
```
Right-click NetworkPlayer → Create Empty
Name: "PlayerCamera"
Position: (0, 1.6, 0)
Add: Camera component
Add: AudioListener component
```

### 4. Link References
- Select NetworkPlayer
- In PlayerController:
  - Drag PlayerCamera into "Camera Transform" field

### 5. Create Visual (Optional)
```
Right-click NetworkPlayer → 3D Object → Capsule
Name: "PlayerVisual"
Position: (0, 1, 0)
Remove: Capsule Collider (CharacterController handles collision)
```

### 6. Save as Prefab
```
Drag NetworkPlayer from Hierarchy to Assets/Prefabs/
Delete NetworkPlayer from Hierarchy (keep only prefab)
```

## Verification Checklist

After creation, verify your prefab has:

- [ ] CharacterController component
- [ ] NetworkObject component with "Is Player Object" = TRUE
- [ ] NetworkTransform component
- [ ] NetworkPlayer script
- [ ] PlayerController script
- [ ] PlayerInventory script
- [ ] PlayerInteraction script
- [ ] PlayerCamera child object at position (0, 1.6, 0)
- [ ] Camera component on PlayerCamera
- [ ] AudioListener on PlayerCamera
- [ ] Camera linked to PlayerController's "Camera Transform" field
- [ ] Visual representation (capsule or custom model)

## Next Steps

Once the NetworkPlayer prefab is created and configured:

1. **Configure NetworkManager**
   - See [UNITY_SETUP.md](UNITY_SETUP.md)
   - Add prefab to NetworkManager's player prefab list
   - Configure spawn settings

2. **Test Multiplayer**
   - Build the game
   - Run one instance as Host
   - Run another as Client
   - Both players should spawn and move

3. **Customize (Optional)**
   - Replace capsule visual with custom character model
   - Adjust camera height and FOV
   - Tweak movement speeds in PlayerController

## Troubleshooting

**"Is Player Object not showing in NetworkObject"**
- Make sure you're viewing the prefab asset, not a scene instance
- Ensure Unity Netcode package is installed

**"Camera not working"**
- Check PlayerCamera is a child of NetworkPlayer
- Verify Camera Transform is linked in PlayerController
- Ensure AudioListener is present on PlayerCamera

**"Player spawns but can't move"**
- Check PlayerController is enabled for local player
- NetworkPlayer script should enable it automatically for owner
- Verify CharacterController is present

**"Players don't see each other"**
- Ensure "Is Player Object" is checked on NetworkObject
- Verify prefab is added to NetworkManager
- Check both instances are connected to same session

**"Name doesn't display above player"**
- NetworkPlayer automatically creates name display
- Check Console for errors in NetworkPlayer.SetupNameDisplay()
- Ensure TextMeshPro package is installed

## Advanced Configuration

### Custom Character Model

To replace the default capsule:
1. Import your character model to Unity
2. Open NetworkPlayer prefab
3. Delete or hide the PlayerVisual capsule
4. Drag your character model under NetworkPlayer
5. Position it at (0, 0, 0)
6. Ensure no colliders on the model (CharacterController handles collision)
7. Save prefab

### Adjust Movement Settings

Open NetworkPlayer prefab and adjust PlayerController:
- **Move Speed**: Base walking speed (default: 5)
- **Sprint Speed**: Running speed (default: 8)
- **Jump Force**: Jump height (default: 5)
- **Mouse Sensitivity**: Camera turn speed (default: 2)

### Network Optimization

For better network performance, adjust NetworkTransform:
- **Interpolate**: Smooth position changes (recommended: enabled)
- **Sync Interval**: How often to sync (default: 0.1s)
- Consider disabling Y position sync if not needed (flat terrain)

## See Also

- [UNITY_SETUP.md](UNITY_SETUP.md) - Complete multiplayer setup guide
- [PREFAB_GUIDE.md](PREFAB_GUIDE.md) - General prefab creation guide
- [MULTIPLAYER.md](MULTIPLAYER.md) - Multiplayer system documentation
- [ARCHITECTURE.md](ARCHITECTURE.md) - Code structure and design

---

**Status:** NetworkPlayer prefab creation is automated via the Prefab Creation Helper tool. Manual creation is also supported for advanced users.

**Last Updated:** v0.2.0 - Multiplayer Setup Phase
