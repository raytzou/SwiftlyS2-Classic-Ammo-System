# Classic Ammo System

<a href="https://www.youtube.com/watch?v=p2h6ZizVcwk" target="_blank">
 <img src="http://img.youtube.com/vi/p2h6ZizVcwk/mqdefault.jpg" alt="Watch the video" width="560" height="315" border="10" />
</a>

Classic Ammo System is a SwiftlyS2 plugin for Counter-Strike 2 that restores classic-style ammo behavior after the game engine applies the newer reload rules.

## Requirements

- Counter-Strike 2 dedicated server
- SwiftlyS2 runtime installed on the server
- SwiftlyS2.CS2 `1.3.2` or a compatible runtime version
- .NET 10 SDK for local development or building from source

This project currently targets `net10.0`.

## Plugin Effect

After the game engine updates weapon ammo counts, the plugin waits until the weapon reload animation has finished and then rewrites the ammo values according to the classic reload rules.

## Example Flow

Example: a player is holding an AK-47 with `21/87` ammo.

1. The player starts reloading.
2. The game engine first applies the current reload rule and changes the ammo to `30/86`.
3. After the reload animation finishes, the plugin applies the classic ammo rule and changes the ammo to `30/78`.

## Usage

0. Build the plugin or download the plugin from [release](https://github.com/raytzou/SwiftlyS2-Classic-Ammo-System/releases/tag/1.0.0).
1. Install the plugin.
2. Start the server at least once after the plugin is installed.
3. After the first server run, the plugin generates `csgo/addons/swiftlys2/configs/plugins/cas/config.jsonc`.
4. Edit `config.jsonc` to configure clip ammo, reserve ammo, and reload animation time.

## Installation

1. Make sure SwiftlyS2 is already installed and working on your CS2 server.
2. Build the plugin or prepare a compiled release.
3. Copy the plugin assembly into the SwiftlyS2 plugin directory used by your server setup.
4. Start the server once so the plugin can load and generate `csgo/addons/swiftlys2/configs/plugins/cas/config.jsonc`.
5. Edit the generated configuration file and restart or reload the plugin as needed.

If you are building from this repository, the compiled plugin assembly is generated under `ClassicAmmoSystem/bin/<Configuration>/net10.0/`.

## Build

Build the solution from the repository root:

```bash
dotnet build ClassicAmmoSystem.slnx
```

For a release build:

```bash
dotnet build ClassicAmmoSystem.slnx -c Release
```

The main project file is `ClassicAmmoSystem/ClassicAmmoSystem.csproj`.

## Configuration Notes

- `Ammo` controls the number of bullets inside the magazine.
- `ReserveAmmo` controls the spare ammo amount.
- `ReloadTime` is measured in seconds and determines when the plugin applies the classic ammo rewrite after the reload animation.
- If `ReloadTime` is set too short, the plugin may update the ammo and then the game engine may subtract one bullet again afterward.

## Technical Notes

- The plugin hooks `weapon_reload` through SwiftlyS2 and uses a delayed rewrite after the configured reload animation time.
- Because `weapon_outofammo` is not currently available as a reliable SwiftlyS2 hook, the plugin uses `EventWeaponFire` and `EventItemEquip` together as a practical empty-mag reload fallback path.
- Reload completion is verified through `WaitForReloadCompletion(...)`, which uses a bounded retry window after the nominal reload time so the final ammo rewrite still happens if `+attack` delays the actual reload start.
- The reload logic is configuration-driven and stores per-weapon values for clip size, reserve ammo, and reload time.
- On weapon entity creation and on round start, the plugin reapplies configured ammo values to keep weapon state consistent.
- Reload sessions are tracked so stale delayed callbacks do not overwrite newer weapon state.
- Shotguns that reload shell-by-shell are intentionally excluded from the timed magazine-style reload rewrite path.

## Known Issue

~~Reloading after a weapon has completely run out of ammo does not use the same event path as `weapon_reload`.~~

~~At the moment, SwiftlyS2 does not provide a valid and usable abstraction for hooking `weapon_outofammo`, so the plugin cannot currently apply the correct classic ammo values for empty-mag reloads.~~

~~In that case, the weapon still follows the newer reload rule and may end up with the `reserve ammo -1` behavior.~~

The current implementation uses `EventWeaponFire` and `EventItemEquip` as the empty-mag workaround, and current testing shows that this path behaves correctly in practice.

## TODO

~~Find a reliable solution for the empty-mag reload path so weapons no longer fall back to the newer reload rule when the magazine has been fully depleted.~~

The current workaround is treated as the working solution unless SwiftlyS2 exposes a more direct and reliable `weapon_outofammo` hook in the future.