using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviourPunCallbacks
{
    public string SceneToLoadAfterAppConfigLoaded;

    public GameObject PlayAgainButton;
    public GameObject PlayAgainBackground;

    public GameObject TexteInfo;

    public void RoomSceneReturn()
    {
        SceneManager.LoadScene("RoomScene");
    }

    public void LobbySceneReturn()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("LobbyScene");
    }

    public void Awake()
    {
        PlayAgainButton.SetActive(PhotonNetwork.IsMasterClient);
        PlayAgainBackground.SetActive(PhotonNetwork.IsMasterClient);
        TexteInfo.SetActive(!PhotonNetwork.IsMasterClient);
    }
}