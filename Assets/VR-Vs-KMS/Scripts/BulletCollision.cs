using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "VRPlayer")
        {
            if (collision.gameObject.GetComponent<LifeManager>())
            {
                collision.gameObject.GetComponent<LifeManager>().ReduceHealth(1);
            }
            
        }

        if (collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VRPlayer")
        {
            if (other.gameObject.GetComponent<LifeManager>())
            {
                Debug.LogError("LifeManager : " + other.gameObject.GetComponent<LifeManager>().name);
                other.gameObject.GetComponent<LifeManager>().ReduceHealth(1);
            }
            Destroy(this.gameObject);
        }
    }
}
