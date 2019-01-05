using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupDestroy : MonoBehaviour {
    public float lifeTime;
    // Use this for initialization
    void Start()
    {
        lifeTime = 2f;
        Invoke("DestroyPickupParticle", lifeTime); 
    }


    public void DestroyPickupParticle()
    {
        Destroy(gameObject);
    }
}
