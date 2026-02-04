# Prefabs Directory

This directory contains reusable Unity prefabs for Vintage Beef.

## Creating Prefabs

### Automated (Recommended)

Use the **Prefab Creation Helper** tool:
- Unity menu → `Vintage Beef → Prefab Creation Helper`
- Click the appropriate button for the prefab you want to create
- Follow the on-screen instructions

### Manual

See the [PREFAB_GUIDE.md](../PREFAB_GUIDE.md) for detailed manual instructions.

## Prefabs

### NetworkPlayer.prefab
**Status:** Ready to create using automated tool

**Purpose:** Multiplayer-enabled player prefab with networking

**Created by:** Prefab Creation Helper or manual setup

**Components:**
- CharacterController
- NetworkObject
- NetworkTransform
- NetworkPlayer
- PlayerController
- PlayerInventory
- PlayerInteraction
- PlayerCamera (child)
- PlayerVisual (child)

**Documentation:** See [NETWORK_PLAYER_SETUP.md](../NETWORK_PLAYER_SETUP.md)

### Resource Nodes (Trees, Rocks, Plants)
**Status:** Needs manual creation

**Purpose:** Gatherable resources in the game world

**Created by:** Manual setup in Unity Editor

**Documentation:** See [PREFAB_GUIDE.md](../PREFAB_GUIDE.md) Section 2

### Materials

This directory also contains materials used by prefabs:
- `PlayerVisualMaterial.mat` - Blue material for player capsule visual

## Usage

### For Developers
1. Use automated tools when available
2. Follow documentation for manual creation
3. Test prefabs in isolation before using in scenes
4. Update documentation if you modify prefabs

### For Artists
- Replace placeholder visuals (like PlayerVisual capsule) with proper models
- Maintain component structure when updating visuals
- Test that NetworkObject components still work after changes

## Notes

- Prefabs in this directory are assets and should be committed to version control
- Test prefabs thoroughly before using in production scenes
- Keep documentation updated when modifying prefabs
- Use Scene Setup Helper to integrate prefabs into scenes

## See Also

- [PREFAB_GUIDE.md](../PREFAB_GUIDE.md) - Complete prefab creation guide
- [NETWORK_PLAYER_SETUP.md](../NETWORK_PLAYER_SETUP.md) - NetworkPlayer specific guide
- [UNITY_SETUP.md](../UNITY_SETUP.md) - Multiplayer setup guide
- [PREFAB_TOOL_SUMMARY.md](../PREFAB_TOOL_SUMMARY.md) - Automated tool details
