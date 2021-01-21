using Photon.Pun;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviourPunCallbacks
{
    public string SceneToLoadAfterAppConfigLoaded;

    public void RoomSceneReturn()
    {

        if (PhotonNetwork.IsMasterClient) { PhotonNetwork.DestroyAll(); }
        SceneManager.LoadScene("RoomScene");
    }

    public void LobbySceneReturn()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("LobbyScene");
    }
}