using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LifeManager : MonoBehaviourPunCallbacks
{
    public float baseHealthPoints = 10.0f;
    private float healthPoints;
    public GameObject lifeBar;

    public delegate void OnDeath(GameObject player, int viewID, int randomNumber);

    /// <summary>
    /// The Death event.
    /// Use like this :
    /// "LifeManager.onDeath += this.myMethode;"
    /// </summary>
    public static event OnDeath onDeath;

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = baseHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthPoints <= 0 && photonView.IsMine)
        {
            if (onDeath != null)
            {

                int randomNumber = Random.Range(0, 20);
                onDeath(gameObject, gameObject.GetComponent<PhotonView>().ViewID, randomNumber);
            }
            healthPoints = baseHealthPoints;
            photonView.RPC("RpcLifeBarUpdate", RpcTarget.AllBuffered, healthPoints);
        }
    }

    public void ReduceHealth(float damage)
    {
        if (photonView.IsMine)
        {
            healthPoints -= damage;
            Debug.Log(healthPoints);
            photonView.RPC("RpcLifeBarUpdate", RpcTarget.AllBuffered, healthPoints);
        }

    }

    [PunRPC]
    public void RpcLifeBarUpdate(float healthPoints)
    {
        lifeBar.GetComponent<Image>().fillAmount = healthPoints / 10;
    }
}