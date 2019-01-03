using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public enum PickupType { life, coin, egg, automaticGun, pistolGun}
    public PickupType pickupType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colide");
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.tag.Equals("Player"))
        {
            CharacterCore player = collisionObject.GetComponent<CharacterCore>();
            switch (pickupType)
            {
                case PickupType.coin:
                    player.eggCoin += 1;
                    player.eggCoinText.text = player.eggCoin.ToString();
                    Destroy(gameObject);
                    break;
                case PickupType.egg:
                    player.egg += 1;
                    player.eggText.text = player.egg.ToString();
                    player.timeBetweenShots = 0.1f;
                    Destroy(gameObject);
                    break;
                case PickupType.life:
                    player.life += 50;
                    player.lifeText.text = player.life.ToString();
                    player.timeBetweenShots = 0.1f;
                    Destroy(gameObject);
                    break;
                case PickupType.pistolGun:
                    player.gunType = player.getGunTypePistol();
                    player.timeBetweenShots = 5f;
                    Destroy(gameObject);
                    break;
                case PickupType.automaticGun:
                    player.gunType = player.getGunTypeAutomatic();
                    player.timeBetweenShots = 0.1f;
                    Destroy(gameObject);
                    break;

            }
        }
    }
   
}
