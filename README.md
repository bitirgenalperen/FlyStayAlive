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

## Project Structure

### Scripts Directory Organization

```
Assets/
└── Scripts/
    ├── Audio/                 # Audio-related scripts
    │   ├── BackgroundMusic.cs    # Manages background music
    │   ├── GameOverSound.cs      # Handles game over sound effects
    │   └── ScoreSound.cs         # Manages scoring sound effects
    │
    ├── Gameplay/             # Core gameplay scripts
    │   └── BirdScript.cs         # Player character controller
    │
    ├── Managers/              # Game management scripts
    │   └── LogicScript.cs        # Main game logic and state management
    │
    ├── UI/                    # User interface scripts
    │   └── MainMenu.cs           # Main menu controller
    │
    └── Utils/                 # Utility scripts and systems
        ├── Cloud/                # Cloud spawning and movement
        │   ├── CloudMoveScript.cs
        │   └── CloudSpawnScript.cs
        │
        ├── Coin/                 # Coin collection system
        │   └── CoinMoveScript.cs
        │
        ├── Ground/               # Ground management
        │   ├── GroundMoveScript.cs
        │   └── GroundSpawnScript.cs
        │
        └── Pipe/                  # Pipe obstacle system
            ├── PipeMoveScript.cs       # Main pipe movement controller
            ├── PipeSpawnScript.cs      # Pipe spawning logic
            ├── PipeMiddleScript.cs     # Handles scoring zones
            │
            └── Movement Patterns/    # Various pipe movement behaviors
                ├── IMovementPattern.cs # Interface for all movement patterns
                ├── LinearMovement.cs   # Basic straight movement
                ├── VerticalMovement.cs # Up/down movement
                ├── SineWaveMovement.cs # Smooth wave pattern
                ├── HoverMovement.cs    # Gentle floating
                ├── FigureEightMovement.cs # Infinity symbol pattern
                ├── OvalMovement.cs      # Circular movement
                ├── RandomJumpMovement.cs # Teleporting movement
                └── ZigZagMovement.cs    # Angular movement
```

## Movement Patterns

All movement patterns implement the `IMovementPattern` interface, which defines two key methods:

```csharp
public interface IMovementPattern
{
    // Calculates the next position based on the current state
    Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, 
                             ref float distanceTraveled, 
                             Vector3 startPosition, float moveSpeed);
    
    // Optional method for drawing gizmos in the Unity editor
    void OnDrawGizmos(Vector3 startPosition, Transform transform);
}
```

### Available Movement Patterns

#### 1. Linear Movement
- **Behavior**: Moves in a straight horizontal line
- **Use Case**: Classic pipe movement, good for beginners
- **Parameters**: 
  - `moveSpeed`: Speed of movement
```csharp
public Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, 
                               ref float distanceTraveled, 
                               Vector3 startPosition, float moveSpeed)
{
    distanceTraveled += moveSpeed * deltaTime;
    return startPosition + Vector3.left * distanceTraveled;
}
```

#### 2. Vertical Movement
- **Behavior**: Moves up and down while moving horizontally
- **Use Case**: Adds vertical challenge to the basic movement
- **Parameters**:
  - `amplitude`: Height of the vertical movement
  - `frequency`: Speed of the up/down cycle

#### 3. Sine Wave Movement
- **Behavior**: Smooth wave-like motion
- **Use Case**: Creates natural-looking, flowing obstacles
- **Parameters**:
  - `amplitude`: Height of the wave
  - `frequency`: How many complete waves per second
  - `speed`: Horizontal movement speed

#### 4. Hover Movement
- **Behavior**: Gentle floating up and down
- **Use Case**: Subtle movement that's not too challenging
- **Visualization**:
  ```
     ___     ___
  __/   \___/   \__
  /                 \
  ```

#### 5. Figure Eight Movement
- **Behavior**: Moves in a continuous figure-eight (∞) pattern
- **Use Case**: Complex movement that requires precise timing
- **Implementation**: Combines two sine waves at 90° phase shift

#### 6. Oval Movement
- **Behavior**: Circular or elliptical path
- **Use Case**: Predictable but challenging circular patterns
- **Parameters**:
  - `radiusX`: Horizontal radius
  - `radiusY`: Vertical radius
  - `rotationSpeed`: How fast to complete a full circle

#### 7. Random Jump Movement
- **Behavior**: Teleports to random positions at intervals
- **Use Case**: Unpredictable, challenging movement
- **Parameters**:
  - `jumpInterval`: Time between jumps (in seconds)
  - `maxVerticalDistance`: Maximum vertical jump distance

#### 8. ZigZag Movement
- **Behavior**: Angular, sharp turns at regular intervals
- **Use Case**: Fast, unpredictable movement
- **Visualization**:
  ```
  /\    /\    /\
 /  \  /  \  /  \
/    \/    \/    \
  ```

### Creating Custom Movement Patterns

To create a new movement pattern:

1. Create a new C# script in the `Movement Patterns` folder
2. Implement the `IMovementPattern` interface
3. Add `[System.Serializable]` attribute to make it configurable in the Unity Inspector
4. Add your custom movement logic in `CalculateMovement`

Example template:

```csharp
using UnityEngine;

[System.Serializable]
public class CustomMovement : IMovementPattern
{
    [Header("Custom Parameters")]
    public float customParameter1 = 1f;
    public float customParameter2 = 2f;

    public Vector3 CalculateMovement(Vector3 currentPosition, float deltaTime, 
                                   ref float distanceTraveled,
                                   Vector3 startPosition, float moveSpeed)
    {
        // Your movement logic here
        distanceTraveled += moveSpeed * deltaTime;
        return new Vector3(
            startPosition.x - distanceTraveled,
            startPosition.y + Mathf.Sin(distanceTraveled * customParameter1) * customParameter2,
            startPosition.z
        );
    }

    public void OnDrawGizmos(Vector3 startPosition, Transform transform)
    {
        // Optional: Draw gizmos to visualize the movement in the Unity editor
    }
}
```

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
