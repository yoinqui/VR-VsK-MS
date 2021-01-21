using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviourPunCallbacks
{
    public GameObject blackScreenKMS;
    public GameObject blackScreenHTC;
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
            StartCoroutine(WaitBeforRespawn(player));
        }
    }

    private IEnumerator WaitBeforRespawn(GameObject player)
    {
        if (player.tag == "Player")
        {
            //GameObject blackScreenGO = null;

            if (photonView.IsMine)
            {
                GameObject panel = player.GetComponent<Canvas>().transform.Find("Panel").gameObject;
                panel.SetActive(true);
                player.transform.position = new Vector3( 0, 20, 0);

                yield return new WaitForSeconds(2);

                player.transform.position = transform.position;

                panel.SetActive(false);
                GetComponent<AudioSource>().Play();
            }
            
        }
        else
        {
            GameObject blackScreenGO = null;

            if (photonView.IsMine)
            {
                blackScreenGO = Instantiate(blackScreenHTC);
                blackScreenGO.GetComponent<Canvas>().worldCamera = player.GetComponent<Camera>();
            }
            GameObject cameraRig = player.transform.parent.gameObject;
            GameObject camera = player;
            Vector3 positionDifference = camera.transform.position - cameraRig.transform.position;

            cameraRig.SetActive(false);
            cameraRig.transform.position = transform.position - new Vector3(positionDifference.x, 0, positionDifference.z);
            yield return new WaitForSeconds(2);
            if (blackScreenGO != null)
            {
                Destroy(blackScreenGO);
            }
            cameraRig.SetActive(true);
        }
    }

    //void FadeImage(bool fadeAway, GameObject blackScreen)
    //{
    //    // fade from opaque to transparent
    //    if (fadeAway)
    //    {
    //        // loop over 1 second backwards
    //        for (float i = 1; i >= 0; i -= Time.deltaTime)
    //        {
    //            // set color with i as alpha
    //            blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Clamp(i, 0.0f, 1f));
    //        }
    //    }
    //    // fade from transparent to opaque
    //    else
    //    {
    //        // loop over 1 second
    //        for (float i = 0; i <= 1; i += Time.deltaTime)
    //        {
    //            // set color with i as alpha
    //            blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Clamp(i, 0.0f, 1f));
    //        }
    //    }
    //}
}
