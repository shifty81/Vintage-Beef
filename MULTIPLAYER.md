# Multiplayer Implementation Guide

This document describes the multiplayer implementation for Vintage Beef v0.2.0.

## Overview

The multiplayer system uses Unity Netcode for GameObjects to support up to 12 players in cooperative gameplay.

## Key Components

### VintageBeefNetworkManager
- Manages network connections (host, client, server modes)
- Configures Unity Transport with IP and port
- Handles connection lifecycle

### NetworkPlayer
- Network-enabled player component
- Synchronizes player name and profession across network
- Displays player names above characters
- Controls which player is locally controlled

### ConnectionUI
- Provides lobby UI for connecting to games
- Username input
- IP address input for joining
- Host/Join buttons

### Updated Components

#### GameManager
- Now supports both single-player and multiplayer modes
- Handles network player spawning when in multiplayer
- Manages connection approval for clients

#### PlayerController
- Supports network play (disabled for remote players)
- Maintains all existing single-player functionality

## How to Use

### Testing Multiplayer Locally

1. **Start the Host:**
   - Run the game
   - Click "Play" in main menu
   - Enter a username
   - Click "Host" button
   - Select your profession
   - Click "Start Game"

2. **Connect as Client:**
   - Run a second instance of the game (build it first)
   - Click "Play" in main menu
   - Enter a different username
   - Enter IP address (127.0.0.1 for local testing)
   - Click "Join" button
   - Select your profession
   - Click "Start Game"

### Testing Over Network

1. **Host side:**
   - Find your local IP address (ipconfig on Windows, ifconfig on Mac/Linux)
   - Share your IP with clients
   - Start as host (same as local testing)

2. **Client side:**
   - Enter the host's IP address
   - Click Join
   - Continue as normal

## Network Features

### Implemented
- ✅ Host/Client connection system
- ✅ Player name synchronization
- ✅ Profession synchronization
- ✅ Player position synchronization (via NetworkTransform)
- ✅ Support for up to 12 players
- ✅ Name display above players
- ✅ Connection UI in lobby

### To Be Implemented (Future)
- ❌ Voice chat
- ❌ Text chat system
- ❌ Player inventory synchronization
- ❌ Resource gathering synchronization
- ❌ Dungeon instancing
- ❌ Dedicated server support
- ❌ Reconnection handling
- ❌ Save/Load for multiplayer sessions

## Network Architecture

```
Host (Server + Client)
    ↓
NetworkManager (Unity Netcode)
    ↓
Unity Transport (UTP)
    ↓
Clients (1-11 additional players)
```

## Port Configuration

- Default Port: 7777 (TCP/UDP)
- Configurable in VintageBeefNetworkManager

## Troubleshooting

### "Failed to start host"
- Check if port 7777 is already in use
- Try restarting Unity
- Check firewall settings

### "Failed to connect"
- Verify IP address is correct
- Ensure host is running and waiting
- Check firewall settings allow Unity through
- Confirm both instances use same Unity Netcode version

### Players not spawning
- Ensure NetworkPlayer prefab is assigned in GameManager
- Check that NetworkObject component is on the player prefab
- Verify NetworkManager has the player prefab in its prefab list

### Position desync
- NetworkTransform component should be on player prefab
- Check that CharacterController is properly configured
- Ensure only owner controls their player

## Performance Considerations

- Tested with 12 players locally
- Recommended for LAN play initially
- Internet play will be improved in future versions with:
  - Client-side prediction
  - Lag compensation
  - Better interpolation

## Security Notes

- Current implementation is for development/testing only
- No authentication system yet
- No anti-cheat measures
- Server authority not fully enforced
- Plan to add these in future versions

## Next Steps (v0.3.0)

1. Implement basic chat system
2. Add player inventory synchronization
3. Synchronize resource gathering
4. Test with multiple computers over internet
5. Add reconnection handling
6. Improve connection error messages
