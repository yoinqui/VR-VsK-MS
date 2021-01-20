using Photon.Pun;
using UnityEngine;

public class Spawner : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        LifeManager.onDeath += SpawnAfterDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnAfterDeath(GameObject player, int viewID, int randomNumber)
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        photonView.RPC("RpcSpawnAfterDeath", RpcTarget.AllBuffered, viewID, randomNumber);
    }

    [PunRPC]
    private void RpcSpawnAfterDeath(int viewID, int randomNumber)
    {
        GameObject player = PhotonView.Find(viewID).gameObject;
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");

        if (gameObject.GetComponent<PhotonView>().ViewID == spawns[randomNumber % spawns.Length].GetComponent<PhotonView>().ViewID)
        {
            if (player.tag == "Player")
            {
                player.SetActive(false);
                player.transform.position = transform.position;
                player.SetActive(true);
            }
            else
            {
                GameObject cameraRig = player.transform.parent.gameObject;
                GameObject camera = player;
                Vector3 positionDifference = camera.transform.position - cameraRig.transform.position;
                cameraRig.transform.position = transform.position - new Vector3(positionDifference.x, 0, positionDifference.z);
            }
        }
    }
}
