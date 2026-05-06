using ClassicAmmoSystem.Services;
using ClassicAmmoSystem.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace ClassicAmmoSystem
{
    [PluginMetadata(Author = "cynicat", Name = "Classic Ammo System", Description = "A simple plugin to bring classic ammo system back", Id = "cas", Version = "1.0.0")]
    public class Main : BasePlugin
    {
        private static ServiceProvider? _serviceProvider;

        public Main(ISwiftlyCore core) : base(core) { }

        public override void Load(bool hotReload)
        {
            RegisterServices();
            RegisterEvents();
            Core.Configuration.InitializeJsonWithModel<Config>("config.jsonc", "WeaponAmmo")
                .Configure(builder =>
                {
                    builder.AddJsonFile("config.jsonc", optional: false, reloadOnChange: true);
                });
            _serviceProvider?.GetRequiredService<IAmmoService>().Initialize();
        }

        public override void Unload()
        {
            if (_serviceProvider is null)
            {
                Core.Logger.LogWarning("Service Provider is null while unloading plugin.");
                return;
            }

            var ammoService = _serviceProvider.GetRequiredService<IAmmoService>();

            ammoService.ClearReloadSession();
            _serviceProvider?.Dispose();
        }

        private void RegisterServices()
        {
            ServiceCollection services = new();

            services.AddSwiftly(Core).AddOptionsWithValidateOnStart<Config>().BindConfiguration("WeaponAmmo");
            services.AddSingleton<IAmmoService, AmmoService>();
            _serviceProvider = services.BuildServiceProvider();
        }

        private void RegisterEvents()
        {
            Core.Event.OnEntityCreated += OnEntityCreated;
            Core.Event.OnMapUnload += OnMapUnload;
            Core.GameEvent.HookPost<EventWeaponReload>((@event) =>
            {
                if (_serviceProvider is null)
                    throw new InvalidOperationException("Service Provider is null.");

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

                var ammoService = _serviceProvider.GetRequiredService<IAmmoService>();
                var weaponBase = activeWeapon.As<CCSWeaponBase>();

                if (!ammoService.IsWeaponBaseValid(weaponBase))
                {
                    Core.Logger.LogWarning("Reload validation failed: active weapon could not be resolved as a valid CCSWeaponBase.");
                    return HookResult.Continue;
                }

                ammoService.ReloadWeapon(weaponBase, player);

                return HookResult.Continue;
            });
            Core.GameEvent.HookPost<EventRoundEnd>((@event) =>
            {
                if (_serviceProvider is null)
                    throw new InvalidOperationException("Service Provider is null.");

                var ammoService = _serviceProvider.GetRequiredService<IAmmoService>();

                ammoService.ClearReloadSession();

                return HookResult.Continue;
            });
            Core.GameEvent.HookPost<EventRoundStart>((@event) =>
            {
                if (_serviceProvider is null)
                    throw new InvalidOperationException("Service Provider is null.");

                var ammoService = _serviceProvider.GetRequiredService<IAmmoService>();

                Core.Scheduler.NextWorldUpdate(() =>
                {
                    ammoService.ResetAllWeaponAmmo();
                });

                return HookResult.Continue;
            });
            Core.GameEvent.HookPost<EventWeaponFire>((@event) =>
            {
                if (_serviceProvider is null)
                    throw new InvalidOperationException("Service Provider is null.");

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

                    var ammoService = _serviceProvider.GetRequiredService<IAmmoService>();
                    var weaponBase = activeWeapon.As<CCSWeaponBase>();
                    var extraDelay = 0f;

                    if (weaponBase.DesignerName == "weapon_deagle" && weaponBase.AttributeManager.Item.ItemDefinitionIndex == 64)
                        extraDelay = 0.5f;

                    ammoService.ReloadWeapon(weaponBase, player, extraDelay);
                });

                return HookResult.Continue;
            });
            Core.GameEvent.HookPost<EventItemEquip>((@event) =>
            {
                if (_serviceProvider is null)
                    throw new InvalidOperationException("Service Provider is null.");

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

                    var ammoService = _serviceProvider.GetRequiredService<IAmmoService>();
                    var weaponBase = activeWeapon.As<CCSWeaponBase>();
                    var extraDelay = 0f;

                    if (weaponBase.DesignerName == "weapon_elite")
                        extraDelay = 0.5f;

                    if (weaponBase.DesignerName == "weapon_deagle" && weaponBase.AttributeManager.Item.ItemDefinitionIndex == 64)
                        extraDelay = 1f;

                    ammoService.ReloadWeapon(weaponBase, player, extraDelay);
                });

                return HookResult.Continue;
            });
        }

        private void OnMapUnload(IOnMapUnloadEvent @event)
        {
            if (_serviceProvider is null)
                throw new InvalidOperationException("Service Provider is null.");

            var ammoService = _serviceProvider.GetRequiredService<IAmmoService>();

            ammoService.ClearReloadSession();
        }

        private void OnEntityCreated(IOnEntityCreatedEvent @event)
        {
            if (_serviceProvider is null)
                throw new InvalidOperationException("Service Provider is null.");

            var instance = @event.Entity;
            if (instance is null || !instance.IsValidEntity)
                return;

            var designName = instance.DesignerName;
            if (!designName.StartsWith("weapon_"))
                return;

            var weaponBase = instance.As<CCSWeaponBase>();

            Core.Scheduler.NextWorldUpdate(() =>
            {
                var ammoService = _serviceProvider.GetRequiredService<IAmmoService>();

                if (!ammoService.IsWeaponBaseValid(weaponBase))
                    return;

                var weaponEntityName = ammoService.GetWeaponEntityName(weaponBase);
                var ammoAmount = ammoService.GetAmmoAmount(weaponEntityName);
                var reserveAmmoAmount = ammoService.GetReserveAmmoAmount(weaponEntityName);

                if (ammoAmount != null)
                    ammoService.SetAmmoAmount(weaponBase, ammoAmount.Value);
                if (reserveAmmoAmount != null)
                    ammoService.SetReserveAmmoAmount(weaponBase, reserveAmmoAmount.Value);
            });
        }
    }
}
