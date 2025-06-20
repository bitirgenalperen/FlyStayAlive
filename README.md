# Fly Stay Alive

![Gameplay Screenshot](Assets/Sprites/gameplay_screenshot.png) <!-- Add a screenshot here -->

## Game Overview
"Fly Stay Alive" is a modern take on the classic endless flyer game genre, where players control a bird navigating through dynamically moving obstacles. The game features various movement patterns for pipes, creating an engaging and challenging experience.

## Features

### Core Gameplay
- **Intuitive Controls**: Simple tap/click mechanics for easy pick-up-and-play
- **Dynamic Obstacles**: Pipes with various movement patterns to challenge players
- **Scoring System**: Earn points by successfully navigating through pipes
- **Visual & Audio Feedback**: Engaging sound effects and visual cues

### Advanced Obstacle System
- **Multiple Movement Patterns**:
  - Linear Movement: Classic straight movement
  - Vertical Movement: Up and down movement
  - Sine Wave: Smooth wave-like motion
  - Hover: Gentle floating up and down
  - Figure Eight: Complex looping pattern
  - Oval: Circular movement pattern
  - Random Jump: Unpredictable teleportation
  - ZigZag: Sharp angular movements

### Technical Features
- **Modular Design**: Easy to add new movement patterns
- **Responsive Controls**: Smooth and precise input handling
- **Optimized Performance**: Efficient collision detection and game loop

## Controls
- **PC**: Press SPACEBAR to make the bird flap
- **Mobile**: Tap anywhere on the screen to make the bird flap

## Game Architecture

### Main Components

#### 1. Bird Controller (`BirdScript.cs`)
- Handles player input and physics
- Manages collision detection
- Controls game over conditions

#### 2. Pipe System
- **Pipe Spawner**: Generates pipes at regular intervals
- **Movement System**: Implements various movement patterns through the `IMovementPattern` interface
- **Scoring Zone**: Detects when the player successfully passes through a pair of pipes

#### 3. Game Manager (`LogicScript.cs`)
- Manages game state (playing/game over)
- Handles scoring system
- Controls audio feedback
- Manages scene transitions

#### 4. Audio System
- Background music
- Sound effects for scoring and game over

### Movement Patterns

All movement patterns implement the `IMovementPattern` interface:

```csharp
public interface IMovementPattern
{
    Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, 
                             ref float distanceTraveled, 
                             Vector3 startPosition, float moveSpeed);
    void OnDrawGizmos(Vector3 startPosition, Transform transform);
}
```

## Getting Started

### Prerequisites
- Unity 2021.3 or later
- Basic knowledge of Unity and C#

### Installation
1. Clone the repository
2. Open the project in Unity Hub
3. Open the main scene
4. Press Play to start the game

## Screenshots
<!-- Add more screenshots as needed -->
![Main Menu](Assets/Sprites/menu_screenshot.png)
![Game Over](Assets/Sprites/game_over_screenshot.png)

## Future Enhancements
- Power-ups and collectibles
- Different bird characters with unique abilities
- Online leaderboards
- Daily challenges
- More visual effects and animations

## Credits
- **Developed by**: [Your Name/Team Name]
- **Art Assets**: [Source of art assets]
- **Sound Effects**: [Source of sound effects]
- **Music**: [Source of background music]

## License
[Specify your license here, e.g., MIT License]
