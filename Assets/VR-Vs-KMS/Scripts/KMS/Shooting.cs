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
    public Camera cam;

    public AudioSource shootSound;

    //private AudioSource[] audioSources;

    private void Start()
    {
        //ControllerInput.onGrabPinch += this.Shoot;
        //audioSources = GetComponents<AudioSource>();
        cam = cam = Camera.main;
    }

    public void Shoot(GameObject origin)
    {
        //AudioSource shootSound = audioSources[2];
        shootSound.Play();
        photonView.RPC("RpcShoot", RpcTarget.All, origin.transform.position);
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
        bulletClone.AddRelativeForce(cam.transform.forward * 1000 * Time.deltaTime, ForceMode.Impulse);
        StartCoroutine(DestroyAfterTime(bulletClone));
    }

    // Update is called once per frame
    void Update()
    {

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
