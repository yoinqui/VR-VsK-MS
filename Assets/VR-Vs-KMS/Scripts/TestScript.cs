using UnityEngine;

public class TestScript : MonoBehaviour
{
    public void Awake()
    {
        Debug.LogError("GameScene Load !");

        if (GameConfig.Inst.LifeNumber != 0) {
            Debug.LogError("JSON Data Management --> Done");
        }
    }
}
