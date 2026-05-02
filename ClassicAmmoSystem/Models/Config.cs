public class Config
{
    public ReserveAmmoConfig ReserveAmmo { get; set; } = new();
    public AmmoConfig Ammo { get; set; } = new();
    public ReloadTimeConfig ReloadTime { get; set; } = new();
}

public class ReserveAmmoConfig
{
    public int Nova { get; set; } = 32;
    public int XM1014 { get; set; } = 32;
    public int MAG7 { get; set; } = 32;
    public int SawedOff { get; set; } = 32;
    public int M249 { get; set; } = 200;
    public int Negev { get; set; } = 200;

    public int AK47 { get; set; } = 90;
    public int M4A4 { get; set; } = 90;
    public int M4A1S { get; set; } = 90;
    public int AUG { get; set; } = 90;
    public int SG553 { get; set; } = 90;
    public int FAMAS { get; set; } = 90;
    public int GalilAR { get; set; } = 90;

    public int AWP { get; set; } = 30;
    public int SSG08 { get; set; } = 90;
    public int SCAR20 { get; set; } = 90;
    public int G3SG1 { get; set; } = 90;

    public int MAC10 { get; set; } = 120;
    public int MP9 { get; set; } = 120;
    public int MP7 { get; set; } = 120;
    public int MP5SD { get; set; } = 120;
    public int UMP45 { get; set; } = 100;
    public int P90 { get; set; } = 100;
    public int Bizon { get; set; } = 120;

    public int Glock18 { get; set; } = 120;
    public int P2000 { get; set; } = 52;
    public int USPS { get; set; } = 100;
    public int DualBerettas { get; set; } = 120;
    public int P250 { get; set; } = 52;
    public int FiveSeven { get; set; } = 100;
    public int Tec9 { get; set; } = 90;
    public int CZ75Auto { get; set; } = 36;
    public int DesertEagle { get; set; } = 35;
    public int Revolver { get; set; } = 24;
}

public class AmmoConfig
{
    public int Nova { get; set; } = 8;
    public int XM1014 { get; set; } = 7;
    public int MAG7 { get; set; } = 5;
    public int SawedOff { get; set; } = 7;
    public int M249 { get; set; } = 100;
    public int Negev { get; set; } = 150;

    public int AK47 { get; set; } = 30;
    public int M4A4 { get; set; } = 30;
    public int M4A1S { get; set; } = 30;
    public int AUG { get; set; } = 30;
    public int SG553 { get; set; } = 30;
    public int FAMAS { get; set; } = 25;
    public int GalilAR { get; set; } = 35;

    public int AWP { get; set; } = 10;
    public int SSG08 { get; set; } = 10;
    public int SCAR20 { get; set; } = 20;
    public int G3SG1 { get; set; } = 20;

    public int MAC10 { get; set; } = 30;
    public int MP9 { get; set; } = 30;
    public int MP7 { get; set; } = 30;
    public int MP5SD { get; set; } = 30;
    public int UMP45 { get; set; } = 25;
    public int P90 { get; set; } = 50;
    public int Bizon { get; set; } = 64;

    public int Glock18 { get; set; } = 20;
    public int P2000 { get; set; } = 13;
    public int USPS { get; set; } = 12;
    public int DualBerettas { get; set; } = 30;
    public int P250 { get; set; } = 13;
    public int FiveSeven { get; set; } = 20;
    public int Tec9 { get; set; } = 18;
    public int CZ75Auto { get; set; } = 12;
    public int DesertEagle { get; set; } = 7;
    public int Revolver { get; set; } = 8;
}

public class ReloadTimeConfig
{
    public float M249 { get; set; } = 5.7f;
    public float Negev { get; set; } = 5.7f;

    public float AK47 { get; set; } = 2.4f;
    public float M4A4 { get; set; } = 3.1f;
    public float M4A1S { get; set; } = 3.1f;
    public float AUG { get; set; } = 3.8f;
    public float SG553 { get; set; } = 2.8f;
    public float FAMAS { get; set; } = 3.3f;
    public float GalilAR { get; set; } = 3.0f;

    public float AWP { get; set; } = 3.7f;
    public float SSG08 { get; set; } = 3.7f;
    public float SCAR20 { get; set; } = 3.1f;
    public float G3SG1 { get; set; } = 4.7f;

    public float MAC10 { get; set; } = 2.6f;
    public float MP9 { get; set; } = 2.1f;
    public float MP7 { get; set; } = 3.1f;
    public float MP5SD { get; set; } = 3.1f;
    public float UMP45 { get; set; } = 3.5f;
    public float P90 { get; set; } = 3.3f;
    public float Bizon { get; set; } = 2.43f;

    public float Glock18 { get; set; } = 2.27f;
    public float P2000 { get; set; } = 2.2f;
    public float USPS { get; set; } = 2.2f;
    public float DualBerettas { get; set; } = 3.78f;
    public float P250 { get; set; } = 2.2f;
    public float FiveSeven { get; set; } = 2.2f;
    public float Tec9 { get; set; } = 2.5f;
    public float CZ75Auto { get; set; } = 2.73f;
    public float DesertEagle { get; set; } = 2.2f;
    public float Revolver { get; set; } = 2.3f;
}