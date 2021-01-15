using System.IO;
using UnityEngine;

[System.Serializable]
public class GameConfig
{
    public int LifeNumber;
    public int DelayShoot;
    public int DelayTeleport;
    public string ColorShotVirus;
    public string ColorShotKMS;
    public int NbContaminatedPlayerToVictory;
    public int RadiusExplosion;
    public int TimeToAreaContamination;
    public string DeviceUsed;

    private static GameConfig inst;

    public static GameConfig Inst
    {
        get
        {
            if (inst == null) inst = new GameConfig();
            return inst;
        }
    }

    public void UpdateValuesFromJSON(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            jsonString = Path.Combine(Application.streamingAssetsPath, "GameConfig.json");
        }
        JsonUtility.FromJsonOverwrite(File.ReadAllText(jsonString), Inst);     
    }
}