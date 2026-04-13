using SwiftlyS2.Shared.SchemaDefinitions;

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
    }
}
