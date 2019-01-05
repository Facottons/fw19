using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour {

    public Transform playerTransform;
    public Vector3 offset;
    public float speed;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;


    // Use this for initialization
    void Start () {

        speed = 5;
        transform.position = playerTransform.position;


    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void LateUpdate()
    {
        if(playerTransform != null)
        {
            Vector3 desiredPositin = playerTransform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position,desiredPositin,speed*Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }



    public void CameraFollowV1()
    {
        if (playerTransform != null)
        {
            // float clampedX = Mathf.Clamp(playerTransform.position.x, minX,maxX);
            //   float clampedY = Mathf.Clamp(playerTransform.position.y, minY, maxY);
            transform.position = Vector2.Lerp(transform.position, new Vector2(playerTransform.position.x, playerTransform.position.y), speed);

        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Vector3 originalPosition = mainCamera.transform.localPosition;

        float elapsed = 0.0f;
        while(elapsed < duration)
        {
            float x = Random.Range(-1f,1f)*magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = new Vector3(x,y,originalPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }
        mainCamera.transform.localPosition = originalPosition;
    }
}
