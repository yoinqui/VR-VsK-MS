using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LifeManager : MonoBehaviourPunCallbacks
{
    private float healthPoints;
    public GameObject lifeBar;
    private GameObject lifeBarScreen;
    public GameObject lifeBarControllerLeft;
    private Material material;

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
        healthPoints = GameConfig.Inst.LifeNumber;
        lifeBarScreen = GameObject.Find("LifeBarScreen");
        if (lifeBarControllerLeft)
        {
            material = lifeBarControllerLeft.GetComponent<Renderer>().material;
        }
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
            photonView.RPC("RpcUpdateNbContaminatedPlayer", RpcTarget.AllBuffered);
            healthPoints = GameConfig.Inst.LifeNumber;
            photonView.RPC("RpcLifeBarUpdate", RpcTarget.All, healthPoints);
            if (gameObject.GetComponent<IsScientistPlayer>() != null)
            {
                GameObject forground = lifeBarScreen.transform.Find("forground").gameObject;
                forground.GetComponent<Image>().fillAmount = healthPoints / 10;
            }
            else
            {
                material.SetFloat("_Cutoff", 1f - healthPoints / 10);
            }
        }

        
    }

    public void ReduceHealth(float damage)
    {
        if (photonView.IsMine)
        {
            healthPoints -= damage;
            photonView.RPC("RpcLifeBarUpdate", RpcTarget.All, healthPoints);
            if (gameObject.GetComponent<IsScientistPlayer>() != null)
            {
                GameObject forground = lifeBarScreen.transform.Find("forground").gameObject;
                forground.GetComponent<Image>().fillAmount = healthPoints / 10;
            }
            else
            {
                material.SetFloat("_Cutoff", 1f - healthPoints / 10);
            }
        }

        hitSound.Play();
    }

    [PunRPC]
    public void RpcLifeBarUpdate(float healthPoints)
    {
        lifeBar.GetComponent<Image>().fillAmount = healthPoints / 10;
    }

    [PunRPC]
    public void RpcUpdateNbContaminatedPlayer()
    {
        DataGame.Inst.UpdateNbContaminatedPlayer(this.gameObject);
    }
}