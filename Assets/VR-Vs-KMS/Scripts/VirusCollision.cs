using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusCollision : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("test");
            if (collision.gameObject.GetComponent<LifeManager>())
            {
                collision.gameObject.GetComponent<LifeManager>().ReduceHealth(1);
            }
        }
        if (collision.gameObject.tag != "VRPlayer")
        {
            Destroy(this.gameObject);
        } else
        {
            Debug.Log("Test");
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VRPlayer")
        {
            Debug.Log("test");
        }
    }


}
