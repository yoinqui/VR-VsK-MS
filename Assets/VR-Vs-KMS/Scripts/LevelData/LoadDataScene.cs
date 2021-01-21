using UnityEngine;

public class LoadDataScene : MonoBehaviour
{
    public Transform ContaminationAreaParent;
    public Transform SpawnPointParent;

    public void Awake()
    {
        if (JSONLevel.Inst.SpawnCount != 0 && JSONLevel.Inst.ContaminationAreaCount != 0 
            && JSONLevel.Inst.ThrowablesCount != 0)
        {
            DestroyObject(GameObject.FindGameObjectsWithTag("ContaminationArea"));
            DestroyObject(GameObject.FindGameObjectsWithTag("ThrowableObject"));
            DestroyObject(GameObject.FindGameObjectsWithTag("SpawnPoint"));

            CreateObjectsFromJSONLevel();
        }
        DataGame.Inst.ResetData();
    }

    public void DestroyObject(Object[] list)
    {
        for (int i = 0; i < list.Length; i += 1)
        {
            Destroy(list[i]);
        }
    }

    public void CreateObjectsFromJSONLevel()
    {
        if (JSONLevel.Inst.SpawnCount != 0)
        {
            for (int i = 0; i < JSONLevel.Inst.SpawnCount; i += 1)
            {
                Vector3 position = new Vector3(JSONLevel.Inst.SpawnList[i].coordinates.x - 15,
                    JSONLevel.Inst.SpawnList[i].coordinates.y, JSONLevel.Inst.SpawnList[i].coordinates.z);

                Quaternion rotation = Quaternion.identity;
                rotation *= Quaternion.Euler(90, 0, 0);

                Instantiate(Resources.Load("Prefabs/SpawnArea"), position, rotation, SpawnPointParent);
            }
        }

        if (JSONLevel.Inst.ContaminationAreaCount != 0)
        {
            for (int i = 0; i < JSONLevel.Inst.ContaminationAreaCount; i += 1)
            {
                Vector3 position = new Vector3(JSONLevel.Inst.ContaminationAreaList[i].coordinates.x - 15,
                    JSONLevel.Inst.ContaminationAreaList[i].coordinates.y,
                    JSONLevel.Inst.ContaminationAreaList[i].coordinates.z);

                Instantiate(Resources.Load("Prefabs/ContaminationArea"), position, Quaternion.identity, ContaminationAreaParent);
            }
        }

        if (JSONLevel.Inst.ThrowablesCount != 0)
        {
            for (int i = 0; i < JSONLevel.Inst.ThrowablesCount; i += 1)
            {
                Vector3 position = new Vector3(JSONLevel.Inst.ThrowablesList[i].coordinates.x - 15,
                    JSONLevel.Inst.ThrowablesList[i].coordinates.y, JSONLevel.Inst.ThrowablesList[i].coordinates.z);

                Instantiate(Resources.Load("Prefabs/Thowable"), position, Quaternion.identity);
            }
        }
    }
}
