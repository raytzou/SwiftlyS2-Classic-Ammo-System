using ClassicAmmoSystem.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace ClassicAmmoSystem
{
    public partial class Main
    {
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
