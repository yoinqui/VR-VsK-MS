using UnityEngine;

public class LoadDataScene : MonoBehaviour
{
    public void Awake()
    {
        if (JSONLevel.Inst.SpawnCount != 0 && JSONLevel.Inst.ContaminationAreaCount != 0 
            && JSONLevel.Inst.ThrowablesCount != 0)
        {
            Debug.LogError("JSON Level Data Management --> Done");

            Debug.LogError("SpawnCount: " + JSONLevel.Inst.SpawnCount);
            Debug.LogError("ContaminationAreaCount: " + JSONLevel.Inst.ContaminationAreaCount);
            Debug.LogError("ThrowablesCount: " + JSONLevel.Inst.ThrowablesCount);
        } else
        {
            Debug.LogError("JSON Level_RA Import --> Load Error");
        }
    }
}
