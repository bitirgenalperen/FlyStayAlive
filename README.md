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

All movement patterns implement the `IMovementPattern` interface, which defines two key methods for calculating movement and optional editor visualization. Each pattern has specific parameters that control its behavior, with many parameters being randomized within defined ranges when the pattern is first used.

### Pattern Details

#### 1. Linear Movement
- **Behavior**: Moves in a straight horizontal line at constant speed
- **Parameters**:
  - `speedMultiplier` (1.0): Multiplies the base movement speed
- **Performance**: 
  - Lightweight with minimal calculations
  - No randomization or state tracking needed

#### 2. Vertical Movement
- **Behavior**: Moves up and down in a sine wave pattern while moving horizontally
- **Parameters**:
  - `height` (2.0f): Vertical movement range (random 1.5-2.5)
  - `speed` (1.5f): Oscillation speed (random 1.0-2.0)
  - `startMovingUp` (random): Initial direction (random true/false)
- **Randomization**:
  - All parameters randomized on first use
  - `timeOffset` ensures unique movement patterns
- **Performance**:
  - Uses efficient sine calculations
  - Single randomization at initialization

#### 3. Sine Wave Movement
- **Behavior**: Smooth wave-like motion along both axes
- **Parameters**:
  - `amplitude` (0.5-2.5): Wave height (randomized)
  - `frequency` (0.5-2.0): Oscillations per second (randomized)
  - `speed` (0.8-2.0): Horizontal movement speed (randomized)
- **Randomization**:
  - All ranges randomized within min/max bounds
  - Unique `timeOffset` for each instance
- **Performance**:
  - Single sine calculation per frame
  - Values cached after initialization

#### 4. Hover Movement
- **Behavior**: Moves horizontally with periodic pauses
- **Parameters**:
  - `hoverSpacing` (8.0f): Distance between hovers
  - `hoverDuration` (0.5f): Pause duration
  - `moveSpeedMultiplier` (2.5f): Movement speed
  - `verticalHover` (true): Enable vertical movement
  - `hoverHeight` (0.3f): Vertical movement range
- **Performance**:
  - Simple state machine for hover/move states
  - No per-frame calculations when hovering

#### 5. Figure Eight Movement
- **Behavior**: Smooth infinity (∞) shaped path
- **Parameters**:
  - `width` (3.0f ± 1.0f): Horizontal size (randomized)
  - `height` (2.0f ± 0.5f): Vertical size (randomized)
  - `speed` (1.0f ± 0.3f): Movement speed (randomized)
  - `randomizeDirection`: Random starting direction
- **Randomization**:
  - All dimensions randomized within variation ranges
  - Can reverse direction randomly
- **Performance**:
  - Combines two sine waves
  - Values pre-calculated on init

#### 6. Oval Movement
- **Behavior**: Circular or elliptical path
- **Parameters**:
  - `width` (1.0-2.5f): Horizontal radius (randomized)
  - `height` (1.0-2.5f): Vertical radius (randomized)
  - `speed` (0.6-1.8f): Rotation speed (randomized)
- **Randomization**:
  - Each axis randomized independently
  - Random starting angle
- **Performance**:
  - Uses efficient sine/cosine
  - Single calculation per axis

#### 7. Random Jump Movement
- **Behavior**: Teleports to random positions at intervals
- **Parameters**:
  - `jumpHeight` (1.0-2.5f): Vertical range (randomized)
  - `jumpInterval` (0.5-2.0s): Time between jumps (randomized)
  - `jumpType`: Instant or smooth transition
  - `jumpSpeed` (5.0-10.0f): For smooth transitions
- **Randomization**:
  - Timing and position randomized
  - Can randomize between jump types
- **Performance**:
  - Minimal calculations between jumps
  - Smooth transitions use lerp

#### 8. ZigZag Movement
- **Behavior**: Angular, sharp turns at regular intervals
- **Parameters**:
  - `width` (1.0-3.0f): Pattern width (randomized)
  - `frequency` (0.5-2.0): Turns per second (randomized)
  - `sharpness` (1-10): Corner angle (randomized)
  - `smoothCorners`: Round the turns
- **Randomization**:
  - All parameters randomized within ranges
  - Smoothness can be toggled
- **Performance**:
  - Uses modulo for pattern repetition
  - Smoothing adds minimal overhead

### Common Implementation Details
- **Initialization**: All patterns use lazy initialization for better performance
- **Randomization**: 
  - Occurs once on first use
  - `timeOffset` ensures unique patterns
- **Bounds**:
  - Y-position clamped to -6..6
  - Consistent horizontal speed
- **Optimizations**:
  - Minimal allocations
  - Cached calculations
  - Efficient math operations
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
