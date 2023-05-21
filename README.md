# Run-n-Gun
A 2D Platformer made with the Unity Game Engine.

### [Play The Game Here!](https://arsh-panesar.itch.io/run-n-gun)

## How To Play

Click on the Link to Play the WebGL Build.

### Controls:
- Move Left  - A / Left Arrow
- Move Right - D / Right Arrow
- Jump       - W / Up Arrow
- Shoot      - Enter
- Pause      - Escape / P

Hold the **Jump** Key at Platforms to Climb them.

## Game Mechanics

### Player

The Player:

- Can Move Left and Right.
- Can Jump.
- Can Shoot Bullets.
- Can Die if they Fall Out of World.
- Can Take Damage/Die from Enemies and Enemy Bullets.
- Has a Health of 100 Points, the UI can be seen on the Top-Left Corner of the Screen.

### Coins

Coins are the Score of the Game.
- Collectible by Player.
- Have a UI on the Top-Right Corner of the Screen.

### Enemy

The Enemy is a Hostile Actor that will Harm the Player if they are in its vicinity.
Enemies have a specific Shooting Pattern, which is a pattern of bullets they can shoot at any instant.
The Shooting Patterns have different Cooldowns (when the enemy can't shoot), opening a window for the Player to Shoot back.

The Enemy:

- Waits for the Player to be Visible.
- Shoot the Player.
- Have Different Shooting Patterns.
- Can Take Damage/Die from Player's Bullets.

The game currently has different configuration of Enemies in terms of:

- Health
- Shooting Pattern

### Level Progression

Completion of Level (Getting to the End Flag) will make Player Complete a Level.

If Player Dies, they restart from beginning of the level.
