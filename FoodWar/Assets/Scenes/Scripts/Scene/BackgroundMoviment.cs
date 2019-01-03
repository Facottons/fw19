using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoviment : MonoBehaviour {

    public Transform playerTransform;
    public float speed;
    public Vector2 prevPlayerPosition;

    public enum BackgroundLevel {Level1,Level2,Level3,Level4,Level5 };
    public BackgroundLevel backgroundLevel;
    public Renderer backgroundQuad3D;

    // Use this for initialization
    void Start () {
		switch(backgroundLevel)
        {
            case BackgroundLevel.Level1:
                speed = 0.05f;
                break;
            case BackgroundLevel.Level2:
                speed = 0.03f;
                break;
            case BackgroundLevel.Level3:
                speed = 0.02f;
                break;
            case BackgroundLevel.Level4:
                speed = 0.01f;
                break;
            case BackgroundLevel.Level5:
                speed = 0.05f;
                break;
        }
  
        prevPlayerPosition = new Vector2(playerTransform.position.x,playerTransform.position.y);

    }
	
	// Update is called once per frame
	void Update () {

        if (playerTransform.position.x > prevPlayerPosition.x )
        {
            Vector2 offset = new Vector2(speed * Time.deltaTime,0);
            backgroundQuad3D.material.mainTextureOffset += offset;
            prevPlayerPosition = playerTransform.position;
        }
        else if(playerTransform.position.x < prevPlayerPosition.x)
        {
            
            Vector2 offset = new Vector2(speed * Time.deltaTime, 0);
            backgroundQuad3D.material.mainTextureOffset -= offset;
            prevPlayerPosition = playerTransform.position;
        }

	}
}
