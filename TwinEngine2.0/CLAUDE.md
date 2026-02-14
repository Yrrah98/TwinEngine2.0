# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Despite the name "TwinEngine2.0", this is a **complete game project**, not just a game engine. Built with C# and the MonoGame framework targeting .NET 8.0, it features a 2D art style and will evolve into a full game experience.

## Architecture

### Entity Component System (ECS)

The codebase follows an **ECS (Entity Component System)** pattern:
- **Entities** are lightweight identifiers
- **Components** are pure data structures
- **Systems** contain logic and operate on entities with specific component combinations

### Design Principles

- **SOLID principles** - Follow single responsibility, open/closed, Liskov substitution, interface segregation, and dependency inversion
- **KISS (Keep It Simple, Stupid)** - Avoid overcomplicating solutions; prefer straightforward implementations
- **Interfaces over inheritance** - Use composition and interfaces rather than deep inheritance hierarchies
- **File Structure** - Ensure file structure is easily navigatable. Game Engine code should be in a folder inside the project, whilst the game code should be elsewhere.

### Planned Systems

1. **Movement System** - Keyboard input handling (mouse input not priority initially)
2. **Collision System** - QuadTree spatial partitioning for efficient collision detection
3. **Rendering System** - Uses MonoGame's built-in drawing capabilities

### Testing Approach

For initial development and testing:
- Player entity: white rectangle
- NPC entities: red rectangles
- Use basic shapes before implementing final art assets

## Development Commands

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

### Clean Build
```bash
dotnet clean
dotnet build
```

### Restore Packages
```bash
dotnet restore
```

## Project Structure

- `Game1.cs` - Main game class inheriting from MonoGame's `Game`
- `Program.cs` - Entry point
- `Content/` - Game assets (textures, sounds, etc.)

## MonoGame Lifecycle

The game loop follows MonoGame's standard lifecycle:
1. `Initialize()` - One-time setup before content loading
2. `LoadContent()` - Load game assets
3. `Update(GameTime)` - Game logic, runs each frame
4. `Draw(GameTime)` - Rendering, runs each frame
