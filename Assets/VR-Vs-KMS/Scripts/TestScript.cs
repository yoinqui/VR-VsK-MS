using UnityEngine;

public class TestScript : MonoBehaviour
{
    public void Awake()
    {
        Debug.LogError("GameScene Load !");

        if (GameConfig.Inst.LifeNumber != 0) {
            Debug.LogError("LifeNumber: " + GameConfig.Inst.LifeNumber);
            Debug.LogError("DelayShoot (ms): " + GameConfig.Inst.DelayShoot);
            Debug.LogError("DelayTeleport (ms): " + GameConfig.Inst.DelayTeleport);
            Debug.LogError("ColorShotVirus: " + GameConfig.Inst.ColorShotVirus);
            Debug.LogError("ColorShotKMS: " + GameConfig.Inst.ColorShotKMS);
            Debug.LogError("NbContaminatedPlayerToVictory: " + GameConfig.Inst.NbContaminatedPlayerToVictory);
            Debug.LogError("RadiusExplosion (m): " + GameConfig.Inst.RadiusExplosion);
            Debug.LogError("TimeToAreaContamination (s): " + GameConfig.Inst.TimeToAreaContamination);
        }
    }
}
