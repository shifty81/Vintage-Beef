# Contributing to Vintage Beef

Thank you for your interest in contributing to Vintage Beef! This document provides guidelines and information for contributors.

## Project Vision

Vintage Beef aims to create a multiplayer cooperative game that:
- Combines Vintage Story's deep crafting and survival mechanics
- Adds dungeon exploration as a core gameplay element
- Supports 12 players with unique professions
- Uses Palia-inspired stylized graphics for broad hardware compatibility
- Provides an accessible entry point for players with older hardware

## Getting Started

1. Read the [README.md](README.md) for project overview
2. Follow the [SETUP.md](SETUP.md) guide to set up your development environment
3. Familiarize yourself with the codebase structure
4. Check the Issues page for tasks labeled "good first issue"

## Development Guidelines

### Code Style

- **Naming Conventions:**
  - Classes: PascalCase (e.g., `PlayerController`)
  - Methods: PascalCase (e.g., `HandleMovement`)
  - Private fields: camelCase with underscore prefix (e.g., `_playerData`) or just camelCase
  - Public fields/properties: PascalCase (e.g., `PlayerName`)
  - Constants: UPPER_SNAKE_CASE (e.g., `MAX_PLAYERS`)

- **Comments:**
  - Use XML documentation comments for public methods and classes
  - Add inline comments for complex logic
  - Explain "why" not "what" in comments

- **Organization:**
  - Keep scripts organized in appropriate folders under Assets/Scripts/
  - Group related functionality together
  - Use namespaces (e.g., `namespace VintageBeef.UI`)

### Unity Best Practices

- **Performance:**
  - Cache component references in Awake() or Start()
  - Avoid using Find() in Update() loops
  - Use object pooling for frequently instantiated objects
  - Keep draw calls low (target: <500 for low-end hardware)

- **Scenes:**
  - Keep scenes lightweight
  - Use prefabs for reusable objects
  - Test in all three scenes before committing

- **Assets:**
  - Optimize textures (use compression)
  - Keep polygon counts reasonable
  - Use LOD (Level of Detail) where appropriate

### Git Workflow

1. **Branch Naming:**
   - Feature: `feature/description`
   - Bug fix: `fix/description`
   - Hotfix: `hotfix/description`

2. **Commits:**
   - Use clear, descriptive commit messages
   - Keep commits focused on a single change
   - Reference issues in commit messages (e.g., "Fixes #123")

3. **Pull Requests:**
   - Provide a clear description of changes
   - Link to related issues
   - Include screenshots for visual changes
   - Ensure all tests pass before requesting review

## Priority Areas

Current development priorities (in order):

1. **Multiplayer Networking**
   - Implement Unity Netcode integration
   - Add player synchronization
   - Create lobby connection system

2. **Profession System**
   - Implement profession-specific abilities
   - Create skill trees
   - Add profession progression

3. **World Generation**
   - Improve terrain generation
   - Add biomes
   - Create points of interest

4. **Dungeon System**
   - Implement dungeon generation
   - Add dungeon instances
   - Create enemies and loot

5. **Crafting & Resources**
   - Resource gathering mechanics
   - Crafting recipes
   - Inventory system

## Testing

### Before Submitting

- Test your changes in Unity Editor
- Build and test standalone if making significant changes
- Check all three scenes work correctly
- Verify no console errors or warnings
- Test on lower-end hardware if possible (or Low quality settings)

### Testing Checklist

- [ ] Code compiles without errors
- [ ] No new console warnings
- [ ] Tested in MainMenu scene
- [ ] Tested in Lobby scene
- [ ] Tested in GameWorld scene
- [ ] Player movement works correctly
- [ ] UI is responsive
- [ ] Performance is acceptable (60+ FPS on medium settings)

## Graphics and Art Guidelines

Following Palia's approach:

- **Style:** Stylized, not photorealistic
- **Colors:** Warm, inviting palette
- **Performance:** Target 60 FPS on GTX 1050 / equivalent
- **Textures:** Use texture atlasing where possible
- **Models:** Keep under 5k triangles for characters, 2k for props
- **Shaders:** Use standard Unity shaders or simple custom shaders

## Documentation

When adding new features:

1. Update README.md if it's a user-facing feature
2. Add XML documentation to public APIs
3. Create or update relevant documentation files
4. Add examples to the setup guide if needed

## Questions and Support

- **Technical Questions:** Open a Discussion on GitHub
- **Bug Reports:** Create an Issue with details
- **Feature Requests:** Open an Issue with the "enhancement" label
- **General Chat:** [Community link TBD]

## Code Review Process

1. Submit a pull request
2. Automated checks run (if configured)
3. Code review by maintainers
4. Address feedback
5. Approval and merge

## License

By contributing, you agree that your contributions will be licensed under the same license as the project [TBD].

## Recognition

Contributors will be recognized in:
- README.md Contributors section
- In-game credits (when implemented)
- Release notes for significant contributions

## Code of Conduct

### Our Standards

- Be respectful and inclusive
- Welcome newcomers
- Give and receive constructive feedback gracefully
- Focus on what's best for the project and community

### Unacceptable Behavior

- Harassment or discrimination
- Trolling or personal attacks
- Publishing private information
- Spam or off-topic content

## Thank You!

Your contributions help make Vintage Beef better for everyone. We appreciate your time and effort!
