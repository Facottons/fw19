using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float lifeTime;
	// Use this for initialization
	void Start () {
        lifeTime = 1.5f;
        Invoke("DestroyExplosion", lifeTime);
        CameraFollow camera = new CameraFollow();
        StartCoroutine(camera.Shake(0.5f, 0.2f));
    }


    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
