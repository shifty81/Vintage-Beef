# Vintage Beef - Setup Guide

## First Time Setup

### Prerequisites
- Unity Hub installed
- Unity 2022.3.10f1 or later
- Git (for cloning the repository)

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/shifty81/Vintage-Beef.git
   cd Vintage-Beef
   ```

2. **Open in Unity Hub**
   - Launch Unity Hub
   - Click "Add" (or "Open")
   - Navigate to the cloned repository folder
   - Select the folder and click "Open"

3. **Wait for Unity to Import**
   - Unity will import all assets and compile scripts
   - This may take a few minutes on first import
   - You'll see a progress bar in the Unity Editor

4. **Verify Setup**
   - Open the Scenes folder in the Project window
   - Double-click MainMenu.unity
   - Press the Play button in Unity Editor
   - You should see the main menu

## Testing the Game

### Single Player Test
1. Press Play in Unity Editor
2. Click "Play" in the main menu
3. Select any profession (e.g., "Explorer")
4. Click "Start Game"
5. You should spawn in the game world
6. Test controls:
   - WASD: Movement
   - Mouse: Look around
   - Space: Jump
   - Left Shift: Sprint
   - ESC: Release cursor

### Finding Dungeons
- Purple cube markers are dungeon entrances
- Walk near a purple cube
- Press 'E' to interact with the dungeon entrance
- A message will appear in the console

## Known Limitations (v0.1.0)

- Multiplayer networking is framework only (not fully functional)
- Dungeons show interaction prompt but don't load instances yet
- World is a simple flat plane with random dungeon placement
- No crafting or resource gathering yet
- No profession-specific abilities yet
- UI is basic and functional (not stylized)

## Building for Testing

### Windows Build
1. File > Build Settings
2. Select "Windows, Mac, Linux"
3. Choose "Windows" as target platform
4. Click "Build"
5. Choose a folder for the build
6. Run the .exe file from the build folder

### WebGL Build (for sharing)
1. File > Build Settings
2. Select "WebGL"
3. Click "Switch Platform" (if not already selected)
4. Click "Build"
5. Host the build folder on a web server

## Troubleshooting

### "Scripts have compile errors"
- Check the Console window for errors
- Ensure all .cs files are in Assets/Scripts/
- Try: Assets > Reimport All

### "Scene not found" error
- Check File > Build Settings
- Ensure all three scenes are listed and checked:
  - MainMenu
  - Lobby
  - GameWorld

### Camera/Movement not working
- Ensure you're in the GameWorld scene
- Check that the Player object has PlayerController script attached
- Verify the camera is a child of the Player object

### UI not appearing
- Check that Canvas objects exist in the scene
- Verify EventSystem exists in the scene
- Check that UI scripts are attached to appropriate GameObjects

## Next Steps

After verifying the basic setup works:
1. Review the code in Assets/Scripts/
2. Familiarize yourself with the profession system
3. Test each scene individually
4. Review the README.md for the development roadmap

## Getting Help

- Check existing issues on GitHub
- Review the code comments in scripts
- Join the community [link TBD]

## Development Workflow

1. Always work on a feature branch
2. Test changes in Unity Editor before committing
3. Keep commits focused and descriptive
4. Update documentation when adding features

## Performance Tips

For better editor performance:
- Use Quality Settings: Low or Medium during development
- Reduce shadow distance in Quality Settings
- Disable unnecessary post-processing
- Use occlusion culling for large worlds (coming soon)

## Future Development Areas

Priority areas for contribution:
1. Multiplayer networking implementation
2. Profession-specific abilities
3. Crafting system
4. Advanced world generation
5. Dungeon content and instances
6. Art asset creation (Palia-style)
