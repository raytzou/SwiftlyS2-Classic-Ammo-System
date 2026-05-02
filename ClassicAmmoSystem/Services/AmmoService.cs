using ClassicAmmoSystem.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.SchemaDefinitions;
using System.Diagnostics.CodeAnalysis;

namespace ClassicAmmoSystem.Services
{
    public class AmmoService : IAmmoService
    {
        private readonly ISwiftlyCore _core;
        private readonly IOptionsMonitor<Config> _config;
        private readonly ILogger<AmmoService> _logger;

        private Dictionary<string, int> _weaponReserveAmmo = [];
        private Dictionary<string, int> _weaponAmmo = [];
        private Dictionary<string, float> _weaponReloadTime = [];

        public AmmoService(ISwiftlyCore core, IOptionsMonitor<Config> config, ILogger<AmmoService> logger)
        {
            _core = core;
            _config = config;
            _logger = logger;
        }

        public void Initialize()
        {
            _weaponReserveAmmo = new()
            {
                ["weapon_nova"] = _config.CurrentValue.ReserveAmmo.Nova,
                ["weapon_xm1014"] = _config.CurrentValue.ReserveAmmo.XM1014,
                ["weapon_mag7"] = _config.CurrentValue.ReserveAmmo.MAG7,
                ["weapon_sawedoff"] = _config.CurrentValue.ReserveAmmo.SawedOff,
                ["weapon_m249"] = _config.CurrentValue.ReserveAmmo.M249,
                ["weapon_negev"] = _config.CurrentValue.ReserveAmmo.Negev,

                ["weapon_ak47"] = _config.CurrentValue.ReserveAmmo.AK47,
                ["weapon_m4a1"] = _config.CurrentValue.ReserveAmmo.M4A4,
                ["weapon_m4a1_silencer"] = _config.CurrentValue.ReserveAmmo.M4A1S,
                ["weapon_aug"] = _config.CurrentValue.ReserveAmmo.AUG,
                ["weapon_sg556"] = _config.CurrentValue.ReserveAmmo.SG553,
                ["weapon_famas"] = _config.CurrentValue.ReserveAmmo.FAMAS,
                ["weapon_galilar"] = _config.CurrentValue.ReserveAmmo.GalilAR,

                ["weapon_awp"] = _config.CurrentValue.ReserveAmmo.AWP,
                ["weapon_ssg08"] = _config.CurrentValue.ReserveAmmo.SSG08,
                ["weapon_scar20"] = _config.CurrentValue.ReserveAmmo.SCAR20,
                ["weapon_g3sg1"] = _config.CurrentValue.ReserveAmmo.G3SG1,

                ["weapon_mac10"] = _config.CurrentValue.ReserveAmmo.MAC10,
                ["weapon_mp9"] = _config.CurrentValue.ReserveAmmo.MP9,
                ["weapon_mp7"] = _config.CurrentValue.ReserveAmmo.MP7,
                ["weapon_mp5sd"] = _config.CurrentValue.ReserveAmmo.MP5SD,
                ["weapon_ump45"] = _config.CurrentValue.ReserveAmmo.UMP45,
                ["weapon_p90"] = _config.CurrentValue.ReserveAmmo.P90,
                ["weapon_bizon"] = _config.CurrentValue.ReserveAmmo.Bizon,

                ["weapon_glock"] = _config.CurrentValue.ReserveAmmo.Glock18,
                ["weapon_hkp2000"] = _config.CurrentValue.ReserveAmmo.P2000,
                ["weapon_usp_silencer"] = _config.CurrentValue.ReserveAmmo.USPS,
                ["weapon_elite"] = _config.CurrentValue.ReserveAmmo.DualBerettas,
                ["weapon_p250"] = _config.CurrentValue.ReserveAmmo.P250,
                ["weapon_fiveseven"] = _config.CurrentValue.ReserveAmmo.FiveSeven,
                ["weapon_tec9"] = _config.CurrentValue.ReserveAmmo.Tec9,
                ["weapon_cz75a"] = _config.CurrentValue.ReserveAmmo.CZ75Auto,
                ["weapon_deagle"] = _config.CurrentValue.ReserveAmmo.DesertEagle,
                ["weapon_revolver"] = _config.CurrentValue.ReserveAmmo.Revolver
            };

            _weaponAmmo = new()
            {
                ["weapon_nova"] = _config.CurrentValue.Ammo.Nova,
                ["weapon_xm1014"] = _config.CurrentValue.Ammo.XM1014,
                ["weapon_mag7"] = _config.CurrentValue.Ammo.MAG7,
                ["weapon_sawedoff"] = _config.CurrentValue.Ammo.SawedOff,
                ["weapon_m249"] = _config.CurrentValue.Ammo.M249,
                ["weapon_negev"] = _config.CurrentValue.Ammo.Negev,

                ["weapon_ak47"] = _config.CurrentValue.Ammo.AK47,
                ["weapon_m4a1"] = _config.CurrentValue.Ammo.M4A4,
                ["weapon_m4a1_silencer"] = _config.CurrentValue.Ammo.M4A1S,
                ["weapon_aug"] = _config.CurrentValue.Ammo.AUG,
                ["weapon_sg556"] = _config.CurrentValue.Ammo.SG553,
                ["weapon_famas"] = _config.CurrentValue.Ammo.FAMAS,
                ["weapon_galilar"] = _config.CurrentValue.Ammo.GalilAR,

                ["weapon_awp"] = _config.CurrentValue.Ammo.AWP,
                ["weapon_ssg08"] = _config.CurrentValue.Ammo.SSG08,
                ["weapon_scar20"] = _config.CurrentValue.Ammo.SCAR20,
                ["weapon_g3sg1"] = _config.CurrentValue.Ammo.G3SG1,

                ["weapon_mac10"] = _config.CurrentValue.Ammo.MAC10,
                ["weapon_mp9"] = _config.CurrentValue.Ammo.MP9,
                ["weapon_mp7"] = _config.CurrentValue.Ammo.MP7,
                ["weapon_mp5sd"] = _config.CurrentValue.Ammo.MP5SD,
                ["weapon_ump45"] = _config.CurrentValue.Ammo.UMP45,
                ["weapon_p90"] = _config.CurrentValue.Ammo.P90,
                ["weapon_bizon"] = _config.CurrentValue.Ammo.Bizon,

                ["weapon_glock"] = _config.CurrentValue.Ammo.Glock18,
                ["weapon_hkp2000"] = _config.CurrentValue.Ammo.P2000,
                ["weapon_usp_silencer"] = _config.CurrentValue.Ammo.USPS,
                ["weapon_elite"] = _config.CurrentValue.Ammo.DualBerettas,
                ["weapon_p250"] = _config.CurrentValue.Ammo.P250,
                ["weapon_fiveseven"] = _config.CurrentValue.Ammo.FiveSeven,
                ["weapon_tec9"] = _config.CurrentValue.Ammo.Tec9,
                ["weapon_cz75a"] = _config.CurrentValue.Ammo.CZ75Auto,
                ["weapon_deagle"] = _config.CurrentValue.Ammo.DesertEagle,
                ["weapon_revolver"] = _config.CurrentValue.Ammo.Revolver
            };

            _weaponReloadTime = new()
            {
                ["weapon_m249"] = _config.CurrentValue.ReloadTime.M249,
                ["weapon_negev"] = _config.CurrentValue.ReloadTime.Negev,

                ["weapon_ak47"] = _config.CurrentValue.ReloadTime.AK47,
                ["weapon_m4a1"] = _config.CurrentValue.ReloadTime.M4A4,
                ["weapon_m4a1_silencer"] = _config.CurrentValue.ReloadTime.M4A1S,
                ["weapon_aug"] = _config.CurrentValue.ReloadTime.AUG,
                ["weapon_sg556"] = _config.CurrentValue.ReloadTime.SG553,
                ["weapon_famas"] = _config.CurrentValue.ReloadTime.FAMAS,
                ["weapon_galilar"] = _config.CurrentValue.ReloadTime.GalilAR,

                ["weapon_awp"] = _config.CurrentValue.ReloadTime.AWP,
                ["weapon_ssg08"] = _config.CurrentValue.ReloadTime.SSG08,
                ["weapon_scar20"] = _config.CurrentValue.ReloadTime.SCAR20,
                ["weapon_g3sg1"] = _config.CurrentValue.ReloadTime.G3SG1,

                ["weapon_mac10"] = _config.CurrentValue.ReloadTime.MAC10,
                ["weapon_mp9"] = _config.CurrentValue.ReloadTime.MP9,
                ["weapon_mp7"] = _config.CurrentValue.ReloadTime.MP7,
                ["weapon_mp5sd"] = _config.CurrentValue.ReloadTime.MP5SD,
                ["weapon_ump45"] = _config.CurrentValue.ReloadTime.UMP45,
                ["weapon_p90"] = _config.CurrentValue.ReloadTime.P90,
                ["weapon_bizon"] = _config.CurrentValue.ReloadTime.Bizon,

                ["weapon_glock"] = _config.CurrentValue.ReloadTime.Glock18,
                ["weapon_hkp2000"] = _config.CurrentValue.ReloadTime.P2000,
                ["weapon_usp_silencer"] = _config.CurrentValue.ReloadTime.USPS,
                ["weapon_elite"] = _config.CurrentValue.ReloadTime.DualBerettas,
                ["weapon_p250"] = _config.CurrentValue.ReloadTime.P250,
                ["weapon_fiveseven"] = _config.CurrentValue.ReloadTime.FiveSeven,
                ["weapon_tec9"] = _config.CurrentValue.ReloadTime.Tec9,
                ["weapon_cz75a"] = _config.CurrentValue.ReloadTime.CZ75Auto,
                ["weapon_deagle"] = _config.CurrentValue.ReloadTime.DesertEagle,
                ["weapon_revolver"] = _config.CurrentValue.ReloadTime.Revolver
            };
        }

        public void SetAmmoAmount(CCSWeaponBase weaponBase, int amount)
        {
            if (!IsWeaponBaseValid(weaponBase))
                throw new InvalidOperationException($"Weapon Base is invalid, {weaponBase.DesignerName}");

            weaponBase.Clip1 = amount;
            weaponBase.WeaponBaseVData.MaxClip1 = amount;
            weaponBase.Clip1Updated();
        }

        public void SetReserveAmmoAmount(CCSWeaponBase weaponBase, int amount)
        {
            if (!IsWeaponBaseValid(weaponBase))
                throw new InvalidOperationException($"Weapon Base is invalid, {weaponBase.DesignerName}");

            weaponBase.ReserveAmmo[0] = amount;
            weaponBase.WeaponBaseVData.PrimaryReserveAmmoMax = amount;
            weaponBase.ReserveAmmoUpdated();
        }

        public int? GetAmmoAmount(string weaponEntityName) => _weaponAmmo.TryGetValue(weaponEntityName, out var amount) ? amount : null;

        public int? GetReserveAmmoAmount(string weaponEntityName) => _weaponReserveAmmo.TryGetValue(weaponEntityName, out var amount) ? amount : null;

        public string GetWeaponEntityName(CCSWeaponBase weaponBase)
        {
            var designName = weaponBase.DesignerName;
            var itemIndex = weaponBase.AttributeManager.Item.ItemDefinitionIndex;

            return (designName, itemIndex) switch
            {
                ("weapon_m4a1", 60) => "weapon_m4a1_silencer",
                ("weapon_hkp2000", 61) => "weapon_usp_silencer",
                ("weapon_mp7", 23) => "weapon_mp5sd",
                ("weapon_deagle", 64) => "weapon_revolver",
                _ => designName
            };
        }


        public void ReloadWeapon(CCSWeaponBase weaponBase)
        {
            if (!IsWeaponBaseValid(weaponBase))
                throw new InvalidOperationException($"Weapon Base is invalid, {weaponBase.DesignerName}");

            var weaponEntityName = GetWeaponEntityName(weaponBase);

            if (IsShotgunWithoutMagzine(weaponEntityName))
                return;

            var currentAmmoAmount = weaponBase.Clip1;
            var currentReserveAmmoAmount = weaponBase.ReserveAmmo[0];
            var clipMaxAmount = GetAmmoAmount(weaponEntityName);
            var reloadTime = GetReloadTime(weaponEntityName);

            if (clipMaxAmount is null)
                return;

            var totalAmmo = currentAmmoAmount + currentReserveAmmoAmount;
            var finalClipAmount = Math.Min(totalAmmo, clipMaxAmount.Value);
            var finalReserveAmount = Math.Max(0, totalAmmo - clipMaxAmount.Value);

            _core.Scheduler.DelayBySeconds(reloadTime, () =>
            {
                _core.Scheduler.NextWorldUpdate(() =>
                {
                    SetAmmoAmount(weaponBase, finalClipAmount);
                    SetReserveAmmoAmount(weaponBase, finalReserveAmount);
                });
            });
        }

        private float GetReloadTime(string weaponEntityName) =>
            _weaponReloadTime.TryGetValue(weaponEntityName, out var reloadTime) ? reloadTime : 2f;

        private bool IsWeaponBaseValid([NotNullWhen(true)] CCSWeaponBase weaponBase) =>
            weaponBase is not null && weaponBase.IsValidEntity && weaponBase.IsValid;

        private bool IsShotgunWithoutMagzine(string weaponEntityName)
        {
            var shotguns = new HashSet<string> { "weapon_nova", "weapon_xm1014", "weapon_sawedoff" };

            return shotguns.Contains(weaponEntityName);
        }
    }
}
