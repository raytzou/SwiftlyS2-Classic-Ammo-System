using ClassicAmmoSystem.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Plugins;

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
            Core.Configuration.InitializeJsonWithModel<Config>("config.jsonc", "Main")
                .Configure(builder =>
                {
                    builder.AddJsonFile("config.jsonc", optional: false, reloadOnChange: true);
                });
        }

        public override void Unload()
        {
            _serviceProvider?.Dispose();
        }

        private void RegisterServices()
        {
            ServiceCollection services = new();

            services.AddSwiftly(Core).AddOptionsWithValidateOnStart<Config>().BindConfiguration("Main");
            _serviceProvider = services.BuildServiceProvider();
        }

        private void RegisterEvents()
        {
            Core.Event.OnEntityCreated += OnEntityCreated;
        }

        private void OnEntityCreated(IOnEntityCreatedEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
