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
    public GameObject lifeBarControllerRight;
    public GameObject lifeBarControllerLeft;
    public Material material;

    public delegate void OnDeath(GameObject player, int viewID, int randomNumber);
    public AudioSource hitSound;

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
        material = lifeBarControllerRight.GetComponent<Renderer>().material;
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

                DataGame.Inst.UpdateNbContaminatedPlayer(this.gameObject);
            }
            healthPoints = baseHealthPoints;
            photonView.RPC("RpcLifeBarUpdate", RpcTarget.All, healthPoints);
        }
    }

    public void ReduceHealth(float damage)
    {
        if (photonView.IsMine)
        {
            healthPoints -= damage;
            photonView.RPC("RpcLifeBarUpdate", RpcTarget.All, healthPoints);
        }

        hitSound.Play();
    }

    [PunRPC]
    public void RpcLifeBarUpdate(float healthPoints)
    {
        lifeBar.GetComponent<Image>().fillAmount = healthPoints / 10;
        material.SetFloat("_Cutoff", 1f - healthPoints / 10);
    }
}