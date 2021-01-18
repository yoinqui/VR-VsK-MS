using UnityEngine;

public class LoadDataScene : MonoBehaviour
{
    public void Awake()
    {
        if (JSONLevel.Inst.SpawnCount != 0 && JSONLevel.Inst.ContaminationAreaCount != 0 
            && JSONLevel.Inst.ThrowablesCount != 0)
        {
            Debug.LogError("JSON Level Data Management --> Done");
        } else
        {
            Debug.LogError("JSON Level_RA Import --> Load Error");
        }
    }
}
