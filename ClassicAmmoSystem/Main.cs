using ClassicAmmoSystem.Services;
using ClassicAmmoSystem.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Plugins;

namespace ClassicAmmoSystem
{
    [PluginMetadata(Author = "cynicat", Name = "Classic Ammo System", Description = "A simple plugin to bring classic ammo system back", Id = "cas", Version = "1.0.0")]
    public partial class Main : BasePlugin
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
            Core.Event.OnEntityCreated -= OnEntityCreated;
            Core.Event.OnMapUnload -= OnMapUnload;
            Core.GameEvent.UnhookPost<EventWeaponReload>();
            Core.GameEvent.UnhookPost<EventRoundEnd>();
            Core.GameEvent.UnhookPost<EventRoundStart>();
            Core.GameEvent.UnhookPost<EventWeaponFire>();
            Core.GameEvent.UnhookPost<EventItemEquip>();

            if (_serviceProvider is null)
            {
                Core.Logger.LogWarning("Service Provider is null while unloading plugin.");
                return;
            }

            var ammoService = _serviceProvider.GetRequiredService<IAmmoService>();

            ammoService.ClearReloadSession();
            _serviceProvider?.Dispose();
            _serviceProvider = null;
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
            if (_serviceProvider is null)
                throw new InvalidOperationException("Service Provider is null.");

            Core.Event.OnEntityCreated += OnEntityCreated;
            Core.Event.OnMapUnload += OnMapUnload;
            Core.GameEvent.HookPost(WeaponReloadHandler());
            Core.GameEvent.HookPost(RoundEndHandler());
            Core.GameEvent.HookPost(RoundStartHandler());
            Core.GameEvent.HookPost(WeaponFireHandler());
            Core.GameEvent.HookPost(ItemEquipHandler());
        }
    }
}
