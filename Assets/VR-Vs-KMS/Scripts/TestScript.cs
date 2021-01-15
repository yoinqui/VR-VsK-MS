using UnityEngine;
using vr_vs_kms;

public class TestScript : MonoBehaviour
{
    public void Awake()
    {
        if (GameConfig.Inst.LifeNumber != 0) {
            Debug.LogError("JSON Import --> Done");
        } else
        {
            Debug.LogError("JSON Import --> Load Error");
        }
    }
}
