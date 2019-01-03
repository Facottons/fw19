using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarrotCrazy : EnemyCore {

    public GameObject explosion;
 
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            gunPositionEnd.transform.position = new Vector2(player.transform.position.x, gunPositionEnd.transform.position.y);
            Debug.DrawLine(gunPositionStart.position, gunPositionEnd.position, Color.red);
            Flip();
        }
        else
        {
            animator.SetBool("noFoundPlayer", true);

        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObjectCollider = collision.gameObject;
        if (gameObjectCollider.tag.Equals("Player"))
        {
   
            Instantiate(explosion,gameObject.transform.position,gameObject.transform.rotation);
            Destroy(gameObjectCollider);
            Destroy(gameObject);
         

        }
    }

    public void destroyObjectWithTime( GameObject gameObject, float time)
    {
        Destroy(gameObject,time);   

    }


    public override void TakeDamage(int damageAmount)
    {
        life -= damageAmount;
        Debug.Log("dano:" + life);
        if (life <= 0 && characterState != EnemyStatus.dying)
        {

            int randomNumber = Random.Range(1, 101);
            if (randomNumber < pickupChance)
            {
                GameObject randomPickup = pickups[Random.Range(0, pickups.Length)];
                Instantiate(randomPickup, transform.position, transform.rotation);
                Debug.Log("Instantiate pickup");
            }
            characterState = EnemyStatus.dying;
            animator.SetBool("isDying", true);
            StartCoroutine("DestroyEnemyAfterSeconds", 0f);
        }
        else
        {
            animator.SetBool("isDying", false);
        }
    }

    public override IEnumerator DestroyEnemyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
        Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
       // destroyObjectWithTime(explosion, 3f);
    }



}
