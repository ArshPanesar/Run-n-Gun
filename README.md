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

Coins are the Score of the Game. They can be exchanged for Weapon Upgrades.
- Collectible by Player.
- Have a UI on the Top-Right Corner of the Screen.
- Can be exchanged for Weapon Upgrades.

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

### Weapon Upgrade

The Player's Weapon can be Upgraded to Shoot Stronger Bullets. The Player has to Spend Coins to Buy Upgrades.
- Use the **B** Button to Open the Upgrade Menu.
- Click Upgrade to get a New Upgrade on the Weapon.
- Coins will be deducted from Player if the Upgrade is Purchased.
  - Total Coins collected that are shown at the end of a level are not affected by this deduction.
  - If the Player does not have enough Coins, a Fail Message will be Displayed and No Coins will be Deducted.

### Health Pickups

Health Pickups increase the Player's Health when collected. 

### Level Progression

Completion of Level (Getting to the End Flag) will make Player Complete a Level.

If the Player Dies, they have to Restart from Beginning of the Level.
