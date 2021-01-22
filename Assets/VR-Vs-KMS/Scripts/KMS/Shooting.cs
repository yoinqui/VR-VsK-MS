using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Rigidbody BulletPrefab;
    public float Speed;
    public int lifeTime = 5;

    public AudioSource shootSound;

    private float Timer = 0;
    private bool IsCanShoot = true;

    //private AudioSource[] audioSources;

    private void Start()
    {
        //ControllerInput.onGrabPinch += this.Shoot;
        //audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsCanShoot)
        {
            Timer += Time.deltaTime;
            if (Timer >= (GameConfig.Inst.DelayShoot / 1000))
            {
                IsCanShoot = true;
                Timer = 0;
            }
        }
    }

    public void Shoot(GameObject origin)
    {
        if (IsCanShoot)
        {            
            //AudioSource shootSound = audioSources[2];
            shootSound.Play();
            photonView.RPC("RpcShoot", RpcTarget.All, origin.transform.position);
            IsCanShoot = false;
        }
    }


    [PunRPC]
    void RpcShoot(Vector3 origin)
    {
        Rigidbody bulletClone = Instantiate(BulletPrefab, origin + new Vector3(0,0,0), this.transform.rotation);
        
        _ = new Color();
        Color newColorBullet;

        if (gameObject.GetComponent<IsScientistPlayer>() != null) {
            ColorUtility.TryParseHtmlString(GameConfig.Inst.ColorShotKMS, out newColorBullet);
        } else {
            ColorUtility.TryParseHtmlString(GameConfig.Inst.ColorShotVirus, out newColorBullet);
        }

        bulletClone.GetComponent<MeshRenderer>().material.color = newColorBullet;
        bulletClone.AddRelativeForce(Vector3.forward * 1000 * Time.deltaTime, ForceMode.Impulse);
        StartCoroutine(DestroyAfterTime(bulletClone));
    }

    public IEnumerator DestroyAfterTime(Rigidbody bullet)
    {
        yield return new WaitForSeconds(lifeTime);
        if (bullet != null)
        {
            Destroy(bullet.gameObject);
        }

    }
}
