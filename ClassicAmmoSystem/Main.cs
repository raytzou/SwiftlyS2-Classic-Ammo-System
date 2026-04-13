using ClassicAmmoSystem.Services;
using ClassicAmmoSystem.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
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
            _serviceProvider?.Dispose();
        }

        private void RegisterServices()
        {
            ServiceCollection services = new();

            services.AddSwiftly(Core).AddOptionsWithValidateOnStart<Config>().BindConfiguration("Main");
            services.AddSingleton<IAmmoService, AmmoService>();
            _serviceProvider = services.BuildServiceProvider();
        }

        private void RegisterEvents()
        {
            Core.Event.OnEntityCreated += OnEntityCreated;
        }

        private void OnEntityCreated(IOnEntityCreatedEvent @event)
        {
            if (_serviceProvider is null)
                throw new InvalidOperationException("Service Provider is null.");

            var instance = @event.Entity;
            var designName = instance.DesignerName;
            if (instance is null || !instance.IsValidEntity || !designName.StartsWith("weapon_")) return;

            Core.Scheduler.NextWorldUpdate(() =>
            {
                var weaponBase = instance.As<CCSWeaponBase>();
                var ammoService = _serviceProvider.GetRequiredService<IAmmoService>();
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
