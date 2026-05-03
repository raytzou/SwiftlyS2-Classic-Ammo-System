using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using System.Diagnostics.CodeAnalysis;

namespace ClassicAmmoSystem.Services.Interfaces
{
    public interface IAmmoService
    {
        public void Initialize();

        /// <summary>
        /// Sets the current ammunition amount for the specified weapon.
        /// </summary>
        /// <param name="weaponBase">The weapon instance for which to set the ammunition amount. Cannot be null.</param>
        /// <param name="amount">The new ammunition amount to assign to the weapon. Must be zero or greater.</param>
        public void SetAmmoAmount(CCSWeaponBase weaponBase, int amount);

        /// <summary>
        /// Sets the reserved ammunition amount for the specified weapon.
        /// </summary>
        /// <param name="weaponBase">The weapon for which to set the reserved ammunition amount. Cannot be null.</param>
        /// <param name="amount">The number of reserved ammunition units to assign. Must be zero or greater.</param>
        public void SetReserveAmmoAmount(CCSWeaponBase weaponBase, int amount);

        /// <summary>
        /// Gets the current amount of ammunition available for the specified weapon entity.
        /// </summary>
        /// <param name="weaponEntityName">The name of the weapon entity for which to retrieve the ammunition amount. Cannot be null or empty.</param>
        /// <returns>The number of ammunition units available for the specified weapon entity. Returns 0 if the weapon entity is
        /// not found or has no ammunition.</returns>
        public int? GetAmmoAmount(string weaponEntityName);

        /// <summary>
        /// Gets the current amount of reserve ammunition available for the specified weapon.
        /// </summary>
        /// <param name="weaponEntityName">The name of the weapon entity for which to retrieve the reserve ammunition amount. Cannot be null or empty.</param>
        /// <returns>The number of reserve ammunition units available for the specified weapon. Returns 0 if the weapon has no
        /// reserve ammunition or is not found.</returns>
        public int? GetReserveAmmoAmount(string weaponEntityName);

        /// <summary>
        /// Gets the entity name for the specified weapon, mapping certain weapon and item index combinations to their
        /// alternate entity names.
        /// </summary>
        /// <remarks>This method maps certain weapons to alternate entity names based on their designer
        /// name and item definition index. For all other weapons, the original designer name is returned.</remarks>
        /// <param name="weaponBase">The weapon instance for which to retrieve the entity name. Must not be null.</param>
        /// <returns>The entity name corresponding to the weapon. For specific weapon and item index combinations, returns an
        /// alternate entity name; otherwise, returns the weapon's designer name.</returns>
        public string GetWeaponEntityName(CCSWeaponBase weaponBase);

        /// <summary>
        /// Reloads the specified weapon for the given player, updating the weapon's clip and reserve ammunition after
        /// the appropriate reload time.
        /// </summary>
        /// <remarks>If the weapon is a shotgun without a magazine or the maximum clip amount cannot be
        /// determined, the reload operation is skipped. The reload is performed asynchronously after the weapon's
        /// reload time has elapsed.</remarks>
        /// <param name="weaponBase">The weapon instance to reload. Must represent a valid weapon base.</param>
        /// <param name="player">The player for whom the weapon is being reloaded. If null or invalid, the reload will not complete.</param>
        /// <exception cref="InvalidOperationException">Thrown if the specified weapon base is invalid.</exception>
        public void ReloadWeapon(CCSWeaponBase weaponBase, IPlayer player);

        /// <summary>
        /// Determines whether the specified weapon base instance is valid for use.
        /// </summary>
        /// <param name="weaponBase">The weapon base instance to validate. Can be null.</param>
        /// <returns>true if the weapon base is not null and both its entity and weapon state are valid; otherwise, false.</returns>
        bool IsWeaponBaseValid([NotNullWhen(true)] CCSWeaponBase? weaponBase);
    }
}
