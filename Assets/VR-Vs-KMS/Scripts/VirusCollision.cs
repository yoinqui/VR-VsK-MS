using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("test");
        }
        if (collision.gameObject.tag != "VRPlayer")
        {
            Destroy(this.gameObject);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
