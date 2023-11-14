# Mythril2D: Changelog

## Version 1.2
### Features
- Added dash ability, unlocked by the archer upon reaching level 3.
- Implemented a new AI navigation system using context steering:
  - AI can now avoid simple obstacles (complex shapes, such as mazes, are not supported).
  - AI can no longer see behind objects, allowing the player to hide effectively.
  - When the AI loses sight of the player, it will move to the last known position and reset after a while if it cannot reestablish visual contact.

### Fixes
- Rectified projectile hitbox (preventing instances where they would hit a target behind the caster).
- Resolved a potential path normalization issue on UNIX platforms.
- Fixed player stats updates:
  - Increasing max stats (equip item, apply points...) will now properly adjust current stats by the same amount instead of resetting them to their maximum value.
  - Leveling up now restores the player's stats.
- Rectified typos in the triple shot and corrosion ability sheets.
- Removed unused code.

## Version 1.1
### Features
- Added the ability to execute scriptable actions when a dialogue starts.
- Added a permanent death setting for monsters.
- Added a new scriptable action to either heal or damage the player.

### Fixes
- Fixed a bug where the `booleanChanged` event (renamed to `gameFlagChanged`) wasn't triggering properly, resulting in some broken interactions with conditional activators.
- Fixed a bug where monster projectiles would sometimes remain attached to their caster.
- Fixed a bug causing teleporters without activation settings to teleport the player multiple times.

## Version 1.0
This is the first version of Mythril2D!
