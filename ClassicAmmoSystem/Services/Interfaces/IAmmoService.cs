using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using System.Diagnostics.CodeAnalysis;

namespace ClassicAmmoSystem.Services.Interfaces
{
    public interface IAmmoService
    {
        public void Initialize();

        /// <summary>
        /// Sets the current and maximum ammunition amount for the specified weapon.
        /// </summary>
        /// <remarks>This method updates both the current clip and the maximum clip size for the weapon.
        /// Use this method to synchronize ammunition counts after events such as reloading or picking up
        /// ammo.</remarks>
        /// <param name="weaponBase">The weapon instance whose ammunition amount will be updated. Must be a valid weapon base.</param>
        /// <param name="amount">The new ammunition amount to assign. This value is set as both the current and maximum clip size.</param>
        /// <exception cref="InvalidOperationException">Thrown if weaponBase is not valid.</exception>
        public void SetAmmoAmount(CCSWeaponBase weaponBase, int amount);

        /// <summary>
        /// Sets the reserve ammunition amount for the specified weapon.
        /// </summary>
        /// <remarks>This method updates both the current and maximum reserve ammunition for the weapon.
        /// Callers should ensure that the amount provided is appropriate for the weapon's intended use.</remarks>
        /// <param name="weaponBase">The weapon for which to set the reserve ammunition amount. Must be a valid instance.</param>
        /// <param name="amount">The new reserve ammunition amount to assign. This value determines both the current and maximum reserve ammo
        /// for the weapon.</param>
        /// <exception cref="InvalidOperationException">Thrown if weaponBase is not a valid weapon instance.</exception>
        public void SetReserveAmmoAmount(CCSWeaponBase weaponBase, int amount);

        /// <summary>
        /// Gets the current amount of ammunition available for the specified weapon entity.
        /// </summary>
        /// <param name="weaponEntityName">The name of the weapon entity for which to retrieve the ammunition amount. Cannot be null.</param>
        /// <returns>The number of available ammunition units for the specified weapon entity, or null if the weapon entity is
        /// not found.</returns>
        public int? GetAmmoAmount(string weaponEntityName);

        /// <summary>
        /// Gets the current reserve ammunition amount for the specified weapon entity, if available.
        /// </summary>
        /// <param name="weaponEntityName">The name of the weapon entity for which to retrieve the reserve ammunition amount. Cannot be null.</param>
        /// <returns>The reserve ammunition amount for the specified weapon entity, or null if the weapon entity is not found.</returns>
        public int? GetReserveAmmoAmount(string weaponEntityName);

        /// <summary>
        /// Gets the entity name for the specified weapon, applying special mappings for certain weapon types.
        /// </summary>
        /// <remarks>This method maps specific weapon and item index combinations to alternate entity
        /// names, reflecting special cases in weapon identification.</remarks>
        /// <param name="weaponBase">The weapon instance for which to retrieve the entity name. Cannot be null.</param>
        /// <returns>The entity name string corresponding to the weapon. For certain weapon types, returns a mapped entity name;
        /// otherwise, returns the designer name.</returns>
        public string GetWeaponEntityName(CCSWeaponBase weaponBase);

        /// <summary>
        /// Reloads the specified weapon for the given player, updating the weapon's clip and reserve ammunition after
        /// the reload delay.
        /// </summary>
        /// <remarks>The reload operation is delayed according to the weapon's reload time. If the weapon
        /// is a shotgun without a magazine or the weapon handle is invalid, the method returns without performing a
        /// reload. The method ensures that the correct weapon and player state are maintained before updating
        /// ammunition counts.</remarks>
        /// <param name="weaponBase">The weapon to reload. Must be a valid weapon instance.</param>
        /// <param name="player">The player who owns the weapon. If null or invalid, the reload will not complete.</param>
        /// <exception cref="InvalidOperationException">Thrown if weaponBase is not a valid weapon instance.</exception>
        public void ReloadWeapon(CCSWeaponBase weaponBase, IPlayer player);

        /// <summary>
        /// Determines whether the specified weapon base instance is valid for use.
        /// </summary>
        /// <param name="weaponBase">The weapon base instance to validate. Can be null.</param>
        /// <returns>true if the weapon base is not null and both its entity and weapon state are valid; otherwise, false.</returns>
        bool IsWeaponBaseValid([NotNullWhen(true)] CCSWeaponBase? weaponBase);

        /// <summary>
        /// Clears all active weapon reload sessions.
        /// </summary>
        /// <remarks>Call this method to reset the reload state for all weapons, typically when
        /// reinitializing or resetting the system. After calling this method, any ongoing reload operations will be
        /// discarded.</remarks>
        void ClearReloadSession();
    }
}
