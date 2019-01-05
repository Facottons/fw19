using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {

    public enum PickupType { life, coin, egg, automaticGun, pistolGun}
    public PickupType pickupType;
    public ParticleSystem pickupDestroyParticle;
    public Sprite [] gunBarSprites;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colide");
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.tag.Equals("Player"))
        {
            CharacterCore player = collisionObject.GetComponent<CharacterCore>();
            Image gunBarImage = GameObject.FindGameObjectWithTag("gunBar").GetComponent<Image>();
            var main = pickupDestroyParticle.main;
            switch (pickupType)
            {
                
                case PickupType.coin:
                    player.eggCoin += 1;
                    player.eggCoinText.text = player.eggCoin.ToString();
                    Destroy(gameObject);
                    main.startColor = new Color(255, 255, 0);
                    Instantiate(pickupDestroyParticle,transform.position,Quaternion.identity);
                    break;
                case PickupType.egg:
                    player.egg += 1;
                    player.eggText.text = player.egg.ToString();
                    player.timeBetweenShots = 0.1f;
                    Destroy(gameObject);
                    main.startColor = new Color(255, 0, 0,255);
                    Instantiate(pickupDestroyParticle, transform.position, Quaternion.identity);
                    break;
                case PickupType.life:        
                    player.life = 100;
                    player.lifeText.text = player.life.ToString();
                    Destroy(gameObject);
                    main.startColor = new Color(255, 0, 0);
                    Instantiate(pickupDestroyParticle, transform.position, Quaternion.identity);
                    break;
                case PickupType.pistolGun:
                    gunBarImage.sprite = gunBarSprites[1];
                    player.gunType = player.getGunTypePistol();
                    player.timeBetweenShots = 5f;
                    Destroy(gameObject);
                    main.startColor = new Color(0, 0, 0);
                    Instantiate(pickupDestroyParticle, transform.position, Quaternion.identity);
                    break;
                case PickupType.automaticGun:
                 
                    gunBarImage.sprite = gunBarSprites[0];
                    player.gunType = player.getGunTypeAutomatic();
                    player.timeBetweenShots = 0.1f;
                    Destroy(gameObject);
                    main.startColor = new Color(0, 0, 0);
                    Instantiate(pickupDestroyParticle, transform.position, Quaternion.identity);
                    break;

            }
        }
    }

}
