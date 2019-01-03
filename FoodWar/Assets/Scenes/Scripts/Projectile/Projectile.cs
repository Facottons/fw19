using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    public float lifeTime;
    public GameObject explosion;
    public int damage;
    public bool instanceDirection;


    // Use this for initialization
    void Start () {
        lifeTime = 1.5f;
        Invoke("DestroyProjectile", lifeTime);
        damage = 1;
        if (instanceDirection)
            transform.localScale = new Vector2(transform.localScale.x*-1, transform.localScale.y);
        else
            transform.localScale = new Vector2(transform.localScale.x * 1, transform.localScale.y);
    }
	
	// Update is called once per frame
	void Update () {
        if(instanceDirection)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void DestroyProjectile()
    {
        if(explosion != null)
        {
            Instantiate(explosion,transform.position,Quaternion.identity);
        }
        Destroy(gameObject);
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if(collision.tag.Equals("Enemy"))
        {
            Debug.Log("colide Enemy");
            collision.GetComponent<EnemyCore>().TakeDamage(damage);
            DestroyProjectile();
        }
        if (collision.tag.Equals("Player"))
        {
            Debug.Log("colide Player");
            collision.GetComponent<CharacterCore>().TakeDamage(damage);
            DestroyProjectile();
        }

    }
}
