using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConfigLoader : MonoBehaviour
{
    public string AppConfigFilePath;

    public void Awake()
    {
        Debug.LogError("InitScene Load !");
        GameConfig.Inst.UpdateValuesFromJSON(AppConfigFilePath);

        ///////////////////////////// LOAD OTHER SCENE /////////////////////////////

        // Load GameScene with the index 0 in Build Settings --> Scenes In Build
        SceneManager.LoadScene(0);
    }
}
