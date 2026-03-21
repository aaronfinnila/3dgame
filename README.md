# 3D Game

A first-person 3D exploration game built with MonoGame. Explore a terrain with a house/cottage structure, featuring player movement, jumping mechanics, and collision detection.

## Technologies

- .NET 9.0
- MonoGame Framework (DesktopGL) 3.8.x
- C#

## Features

- First-person camera with mouse look
- WASD movement relative to camera direction
- Jump mechanics with gravity physics
- Collision detection (bounding spheres and boxes)
- 3D model rendering with textures and lighting

## Controls

| Key       | Action        |
|-----------|---------------|
| W         | Move forward  |
| S         | Move backward |
| A         | Strafe left   |
| D         | Strafe right  |
| Space     | Jump          |
| Mouse     | Look around   |
| Escape    | Exit game     |

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd 3dgame
   ```

2. Restore .NET tools:
   ```bash
   dotnet tool restore
   ```

3. Build and run:
   ```bash
   dotnet run
   ```

## Building

```bash
# Build the project
dotnet build

# Build in release mode
dotnet build -c Release
```

## Project Structure

```
3dgame/
├── Content/
│   ├── Models/          # 3D models (.fbx, .x) and textures
│   ├── Textures/        # Additional textures
│   └── Content.mgcb     # MonoGame content pipeline config
├── Game1.cs             # Main game loop
├── Player.cs            # Player movement and physics
├── Camera.cs            # First-person camera system
├── GameObject.cs        # Base class for game objects
├── GameConstants.cs     # Configuration constants
└── Program.cs           # Entry point
```

## Platform Support

This game uses MonoGame.Framework.DesktopGL, providing cross-platform support for:

- Windows
- Linux
- macOS

## License

See LICENSE file for details.
