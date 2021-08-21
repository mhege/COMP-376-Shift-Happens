using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 15f;
    public Rigidbody2D theRB;
    public GameObject impactEffect;

    public float damageToGive;
    public float dftlDmgToGive;

    private bool hasCollide = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.right * speed;
    }

    //when bullet collides with any other object
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (!other.CompareTag("Player") && !hasCollide)
        {
            

            if (other.CompareTag("small")) //if obj has small tag, bullets do damage
            {
                other.GetComponent<SmallEnemy>().DamageEnemy(damageToGive);
                bulletImpact();
            }

            if (other.CompareTag("medium")) //if obj has medium tag, bullets do damage
            {
                if(other.GetComponent<MediumEnemy>())
                    other.GetComponent<MediumEnemy>().DamageEnemy(damageToGive);
                bulletImpact();
            }

            if (other.CompareTag("large")) //if obj has large tag, bullets do damage
            {
                other.GetComponent<LargeEnemy>().DamageEnemy(damageToGive);
                bulletImpact();
            }

            if (other.CompareTag("boss")) //if obj has boss tag, bullets do damage
            {
                other.GetComponent<BossEnemy>().DamageEnemy(damageToGive);
                bulletImpact();
            }
            
            if (other.CompareTag("walls") || other.CompareTag("decorations")) //if obj has boss tag, bullets do damage
            {
                bulletImpact();
            }


        }

    }

    private void bulletImpact()
    {
        if (!hasCollide && gameObject.activeSelf)
        {
            Instantiate(impactEffect, transform.position, transform.rotation); //what, where
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(WaitToDie());
        }
        
    }

    private IEnumerator WaitToDie()
    {
        hasCollide = true;
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    private void OnBecameInvisible() // bullets get destoyred when no longer in scene
    {
        if (!hasCollide && gameObject.activeSelf)
        {
            StartCoroutine(WaitToDie());
        }
    }

}
