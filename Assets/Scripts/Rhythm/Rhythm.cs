using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm : MonoBehaviour {
    public float currentSpeed;

    public virtual void OnTriggerEnter(Collider other)
    {
        //player tag
        if (other.gameObject.tag == "Player")
        {
            
        }
    }

}
