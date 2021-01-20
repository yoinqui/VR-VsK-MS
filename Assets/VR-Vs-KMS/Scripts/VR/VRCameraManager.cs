using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRCameraManager : MonoBehaviourPunCallbacks
{
    public GameObject blackScreen;
    public int time = 2;
    public GameObject wallGO;

 

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            blackScreen.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<wallCollider>())
        {
            wallGO = other.gameObject;
            StartCoroutine(FadeImage(false));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<wallCollider>())
        {
            wallGO = null;
            StartCoroutine(FadeImage(true));

        }
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                if (wallGO != null)
                {
                    break;
                }
                // set color with i as alpha
                blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Clamp(i, 0.0f, 0.95f));
                yield return null;
            }
            yield return null;
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= time; i += Time.deltaTime)
            {
                if (wallGO == null)
                {
                    break;
                }
                // set color with i as alpha
                blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Clamp(i, 0.0f, 0.95f));
                yield return null;
            }
            yield return null;
        }
    }
}
