using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConfigLoader : MonoBehaviour
{
    public string AppConfigFilePath;
    public string SceneToLoadAfterAppConfigLoaded;

    public void Awake()
    {
        GameConfig.Inst.UpdateValuesFromJSON(AppConfigFilePath);

        ///////////////////////////// LOAD OTHER SCENE /////////////////////////////

        // Load SceneToLoadAfterAppConfigLoaded
        if (!string.IsNullOrEmpty(SceneToLoadAfterAppConfigLoaded))
        {
            SceneManager.LoadScene(SceneToLoadAfterAppConfigLoaded);
        }
    }
}
