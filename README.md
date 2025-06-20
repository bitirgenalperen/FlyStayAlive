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

All movement patterns implement the `IMovementPattern` interface, which defines two key methods for calculating movement and optional editor visualization.

### Movement Pattern Parameters and Randomization

Each pattern has specific parameters that control its behavior. Many parameters are randomized within defined ranges when the pattern is first used. Here's a detailed breakdown of each pattern:

#### 1. Linear Movement
- **Parameters**:
  - `speedMultiplier` (1.0 by default): Multiplies the base movement speed
- **Randomization**: None
- **Behavior**: Moves in a straight horizontal line at constant speed

#### 2. Vertical Movement
- **Parameters**:
  - `height` (2.0f): Maximum vertical movement range
  - `speed` (1.5f): Speed of vertical oscillation
  - `startMovingUp` (random): Initial direction of movement
- **Randomization on first use**:
  - `height`: Random between 1.5 and 2.5
  - `speed`: Random between 1.0 and 2.0
  - `startMovingUp`: Randomly true or false
  - `timeOffset`: Random phase offset for the movement

#### 3. Sine Wave Movement
- **Parameters**:
  - `minAmplitude`/`maxAmplitude` (0.5f/2.5f): Wave height range
  - `minFrequency`/`maxFrequency` (0.5f/2.0f): Oscillation speed range
  - `minSpeed`/`maxSpeed` (0.8f/2.0f): Horizontal speed range
- **Randomization on first use**:
  - `amplitude`: Random between minAmplitude and maxAmplitude
  - `frequency`: Random between minFrequency and maxFrequency
  - `speed`: Random between minSpeed and maxSpeed
  - `timeOffset`: Random phase offset

#### 4. Hover Movement
- **Parameters**:
  - `hoverSpacing` (8.0f): Distance between hover points
  - `hoverDuration` (0.5f): Time spent at each hover point
  - `moveSpeedMultiplier` (2.5f): Speed when moving between points
  - `verticalHover` (true): If true, adds vertical movement while hovering
  - `hoverHeight` (0.3f): Height of vertical hover movement
- **Randomization**: None
- **Behavior**: Moves horizontally, pausing at regular intervals

#### 5. Figure Eight Movement
- **Parameters**:
  - `width` (3.0f): Base width of the figure-eight
  - `widthVariation` (1.0f): Random variation in width
  - `height` (2.0f): Base height of the figure-eight
  - `heightVariation` (0.5f): Random variation in height
  - `speed` (1.0f): Base movement speed
  - `speedVariation` (0.3f): Random speed variation
  - `randomizeDirection` (true): If true, direction is random
- **Randomization on first use**:
  - `randomWidth`: width ± widthVariation
  - `randomHeight`: height ± heightVariation
  - `randomSpeedMultiplier`: 1.0 ± speedVariation
  - `directionMultiplier`: Random 1 or -1 if randomizeDirection is true

#### 6. Oval Movement
- **Parameters**:
  - `minWidth`/`maxWidth` (1.0f/2.5f): Width range of the oval
  - `minHeight`/`maxHeight` (1.0f/2.5f): Height range of the oval
  - `minSpeed`/`maxSpeed` (0.6f/1.8f): Movement speed range
- **Randomization on first use**:
  - `width`: Random between minWidth and maxWidth
  - `height`: Random between minHeight and maxHeight
  - `speed`: Random between minSpeed and maxSpeed
  - `timeOffset`: Random phase offset

#### 7. Random Jump Movement
- **Parameters**:
  - `minJumpHeight`/`maxJumpHeight` (1.0f/2.5f): Jump height range
  - `minJumpInterval`/`maxJumpInterval` (0.5f/2.0f): Time between jumps
  - `randomizeJumpType` (true): If true, randomizes between instant and smooth jumps
  - `minJumpSpeed`/`maxJumpSpeed` (5.0f/10.0f): Speed of smooth jumps
- **Randomization**:
  - Jump timing: Random between minJumpInterval and maxJumpInterval
  - Jump height: Random between minJumpHeight and maxJumpHeight
  - Jump type: Random if randomizeJumpType is true
  - Jump speed: Random between minJumpSpeed and maxJumpSpeed for smooth jumps

#### 8. ZigZag Movement
- **Parameters**:
  - `minWidth`/`maxWidth` (1.0f/3.0f): Width of the zigzag pattern
  - `minFrequency`/`maxFrequency` (0.5f/2.0f): Number of zigzags per second
  - `minSharpness`/`maxSharpness` (1/5, 1/10): Corner sharpness (1-10)
  - `smoothCorners` (false): If true, rounds the corners
- **Randomization on first use**:
  - `width`: Random between minWidth and maxWidth
  - `frequency`: Random between minFrequency and maxFrequency
  - `sharpness`: Random between minSharpness and maxSharpness

### Common Parameters
All patterns share these common behaviors:
- `distanceTraveled`: Tracks horizontal movement for consistent speed
- `startPosition`: Original spawn position of the obstacle
- `moveSpeed`: Base movement speed from the game controller
- Y-position is clamped between -6 and 6 to keep obstacles on screen

### Creating Consistent Randomness
Each pattern uses `timeOffset` to ensure consistent movement:
- Randomized once when the pattern is first used
- Ensures identical objects don't move in sync
- Maintains smooth, predictable movement patterns

### Performance Considerations
- Patterns are initialized on first use (lazy initialization)
- Random values are cached to prevent per-frame calculations
- Complex patterns (like Figure Eight) use efficient math operations

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
