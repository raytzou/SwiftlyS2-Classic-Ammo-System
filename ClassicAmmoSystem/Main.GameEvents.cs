using ClassicAmmoSystem.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.SchemaDefinitions;
using static SwiftlyS2.Shared.GameEvents.IGameEventService;

namespace ClassicAmmoSystem
{
    public partial class Main
    {
        private GameEventHandler<EventWeaponReload> WeaponReloadHandler()
        {
            return (@event) =>
            {
                @event.DontBroadcast = true;

                var player = @event.UserIdPlayer;

                if (player is null || !player.IsValid)
                {
                    Core.Logger.LogWarning("Reload validation failed: player is null or invalid.");
                    return HookResult.Continue;
                }

                if (player.PlayerPawn is null || !player.PlayerPawn.IsValid)
                {
                    Core.Logger.LogWarning("Reload validation failed: player pawn is null or invalid.");
                    return HookResult.Continue;
                }

                if (player.PlayerPawn.WeaponServices is null || !player.PlayerPawn.WeaponServices.IsValid)
                {
                    Core.Logger.LogWarning("Reload validation failed: weapon services are null or invalid.");
                    return HookResult.Continue;
                }

                var activeWeapon = player.PlayerPawn.WeaponServices.ActiveWeapon.Value;

                if (activeWeapon is null || !activeWeapon.IsValid)
                {
                    Core.Logger.LogWarning("Reload validation failed: active weapon is null or invalid.");
                    return HookResult.Continue;
                }

                var ammoService = _serviceProvider!.GetRequiredService<IAmmoService>();
                var weaponBase = activeWeapon.As<CCSWeaponBase>();

                if (!ammoService.IsWeaponBaseValid(weaponBase))
                {
                    Core.Logger.LogWarning("Reload validation failed: active weapon could not be resolved as a valid CCSWeaponBase.");
                    return HookResult.Continue;
                }

                ammoService.ReloadWeapon(weaponBase, player);

                return HookResult.Continue;
            };
        }

        private static GameEventHandler<EventRoundEnd> RoundEndHandler()
        {
            return (@event) =>
            {
                var ammoService = _serviceProvider!.GetRequiredService<IAmmoService>();

                ammoService.ClearReloadSession();

                return HookResult.Continue;
            };
        }

        private GameEventHandler<EventRoundStart> RoundStartHandler()
        {
            return (@event) =>
            {
                var ammoService = _serviceProvider!.GetRequiredService<IAmmoService>();

                Core.Scheduler.NextWorldUpdate(() =>
                {
                    ammoService.ResetAllWeaponAmmo();
                });

                return HookResult.Continue;
            };
        }

        private SwiftlyS2.Shared.GameEvents.IGameEventService.GameEventHandler<EventWeaponFire> WeaponFireHandler()
        {
            return (@event) =>
            {
                var playerPawn = @event.UserIdPawn;

                if (playerPawn is null || !playerPawn.IsValid)
                {
                    Core.Logger.LogWarning("EventWeaponFire: player pawn is null or invalid.");
                    return HookResult.Continue;
                }

                var weaponService = playerPawn.WeaponServices;

                if (weaponService is null || !weaponService.IsValid)
                {
                    Core.Logger.LogWarning("EventWeaponFire: weapon service is null or invalid.");
                    return HookResult.Continue;
                }

                var activeWeapon = weaponService.ActiveWeapon.Value;

                if (activeWeapon is null || !activeWeapon.IsValid)
                {
                    Core.Logger.LogWarning("EventWeaponFire: active weapon is null or invalid");
                    return HookResult.Continue;
                }

                var ammoAmount = activeWeapon.Clip1;

                if (ammoAmount - 1 != 0)
                    return HookResult.Continue;

                var player = @event.UserIdPlayer;

                Core.Scheduler.NextWorldUpdate(() =>
                {
                    if (player is null || !player.IsValid)
                    {
                        Core.Logger.LogWarning("EventWeaponFire: player is null or invalid");
                        return;
                    }

                    var ammoService = _serviceProvider!.GetRequiredService<IAmmoService>();
                    var weaponBase = activeWeapon.As<CCSWeaponBase>();
                    var extraDelay = 0f;

                    if (weaponBase.DesignerName == "weapon_deagle" && weaponBase.AttributeManager.Item.ItemDefinitionIndex == 64)
                        extraDelay = 0.5f;

                    ammoService.ReloadWeapon(weaponBase, player, extraDelay);
                });

                return HookResult.Continue;
            };
        }

        private SwiftlyS2.Shared.GameEvents.IGameEventService.GameEventHandler<EventItemEquip> ItemEquipHandler()
        {
            return (@event) =>
            {
                var playerPawn = @event.UserIdPawn;

                if (playerPawn is null || !playerPawn.IsValid)
                {
                    Core.Logger.LogWarning("EventItemEquip: player pawn is null or invalid.");
                    return HookResult.Continue;
                }

                var weaponService = playerPawn.WeaponServices;

                if (weaponService is null || !weaponService.IsValid)
                {
                    Core.Logger.LogWarning("EventItemEquip: weapon service is null or invalid.");
                    return HookResult.Continue;
                }

                var activeWeapon = weaponService.ActiveWeapon.Value;

                if (activeWeapon is null || !activeWeapon.IsValid)
                {
                    Core.Logger.LogWarning("EventItemEquip: active weapon is null or invalid");
                    return HookResult.Continue;
                }

                var ammoAmount = activeWeapon.Clip1;

                if (ammoAmount != 0)
                    return HookResult.Continue;

                var player = @event.UserIdPlayer;

                Core.Scheduler.NextWorldUpdate(() =>
                {
                    if (player is null || !player.IsValid)
                    {
                        Core.Logger.LogWarning("EventItemEquip: player is null or invalid");
                        return;
                    }

                    var ammoService = _serviceProvider!.GetRequiredService<IAmmoService>();
                    var weaponBase = activeWeapon.As<CCSWeaponBase>();
                    var extraDelay = 0f;

                    if (weaponBase.DesignerName == "weapon_elite")
                        extraDelay = 0.5f;

                    if (weaponBase.DesignerName == "weapon_deagle" && weaponBase.AttributeManager.Item.ItemDefinitionIndex == 64)
                        extraDelay = 1f;

                    ammoService.ReloadWeapon(weaponBase, player, extraDelay);
                });

                return HookResult.Continue;
            };
        }
    }
}
