using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeEnemy : MonoBehaviour
{
    private Rigidbody2D theRB;
    private Vector3 moveDirection;
    private Animator anim;

    //Fading Out after Death
    public float fadeDelay = 1f;
    public float alphaValue = 0;
    public bool destroyGameObject = false;

    //Wandering Enemy Type (Blue)
    [Header("Wandering")]
    public float wanderLength;
    public float pauseLength;
    private float wanderCounter;
    private float pauseCounter;
    private Vector3 wanderDirection;

    [Header("Variables")]
    public float maxhealth = 5;
    public float health = 5;
    public EnemyHealthBar healthBar;
    public float moveSpeed;

    [Header("Damage Counters")]
    public GameObject dmgParentPrefab;
    GameObject dmgTemp;

    //animation direction
    private SpriteRenderer sprite;
    bool isMovingVertUp = false;
    bool isMovingHoriz = false;
    bool isMovingVertDown = false;

    //stomp and charge inserts
    bool decideAttack = false;
    bool attacking = false;
    bool attackType = false;
    bool isStomping = false;
    bool endStomp = false;
    bool isCharging = false;
    bool endCharge = false;
    bool chargeTimer = false;
    bool chargeNow = false;
    bool hasCollided = false;
    float attackTiming = 2.0f;
    float stompLength = 1.5f;
    float chargeLength = 2.0f;
    float chargeSpeed = 6.0f;
    int pickAttack = 0;
    int chargeAmount = 0;
    Vector3 chargeLocation;
    Vector3 chargeVector;
    Vector3 tempLocation;
    Vector3 tempVector;
    public GameObject bigZonePrefab;
    public GameObject smallZonePrefab;
    public GameObject zoneDmgPrefab;
    GameObject bzGO;
    GameObject szGO;
    GameObject zdGO;
    public GameObject stompEffect;

    private IEnumerator chargeCoroutine;

    [Header("MiniBoss")]
    public bool isMiniBoss;
    public MiniBoss miniBoss;

    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        
        healthBar.SetHealth(health, maxhealth);

        StartCoroutine(attackCycle(attackTiming));
        chargeCoroutine = chargeAttack(0.5f);
    }

    // Update is called once per frame
    void Update()
    {

        if (sprite.isVisible) //Enemy is within 
        {
            moveDirection = Vector3.zero;


            if (wanderCounter > 0)
            {
                wanderCounter -= Time.deltaTime;

                //move the enemy
                moveDirection = wanderDirection;

                if (wanderCounter <= 0)
                {
                    pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                }
            }

            if (pauseCounter > 0)
            {

                pauseCounter -= Time.deltaTime;

                if (pauseCounter <= 0)
                {
                    wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                    wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                }
            }

            moveDirection.Normalize(); // no matter the direction

            if (hasCollided && isCharging)
            {
                
                chargeAmount = 0;
                isCharging = false;
                chargeNow = false;
                endCharge = false;
                chargeTimer = false;
                hasCollided = false;
                chargeVector = Vector3.zero;
                StopCoroutine(chargeCoroutine);
                StartCoroutine(attackCycle(attackTiming));
            }
            else if ( (isCharging && chargeTimer) )
            {


                isMovingVertUp = false;
                isMovingHoriz = false;
                isMovingVertDown = false;
                //chargeNow = false;
                isStomping = false;

                anim.SetBool("isMovingInX", isMovingHoriz);
                anim.SetBool("isMovingPosY", isMovingVertUp);
                anim.SetBool("isMovingNegY", isMovingVertDown);
                anim.SetBool("isNowStomping", isStomping);
                anim.SetBool("isNowCharging", chargeNow);

                moveDirection = Vector3.zero;
                transform.Translate(chargeVector.x * chargeSpeed * Time.deltaTime, chargeVector.y * chargeSpeed * Time.deltaTime, 0.0f);
                viewDirection(chargeVector);


                if (((transform.position.x + .1f >= chargeLocation.x) && (transform.position.x - .1f <= chargeLocation.x) && (transform.position.y + .1f >= chargeLocation.y) && (transform.position.y - .1f <= chargeLocation.y)))
                {
                        chargeTimer = false;
                        theRB.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                        chargeCoroutine = chargeAttack(0.5f);
                        StartCoroutine(chargeCoroutine);
                        chargeVector = Vector3.zero;
                }
                  
            }
            else if (attacking)
            {
                moveDirection = Vector3.zero;
                theRB.velocity = new Vector3(0.0f,0.0f,0.0f); 
                anim.SetBool("isWalking", false);
                viewDirection(moveDirection);
            }
            else
            {
                

                theRB.velocity = moveDirection * moveSpeed; 

                anim.SetBool("isWalking", true);
            }

        
            if (decideAttack)
            {
                decideAttack = false;

                pickAttack = Random.Range(0, 2);

                if (pickAttack == 0)
                    attackType = false;
                else
                    attackType = true;
                
                //do attack stuff
                if (!attackType)
                {
                    
                    isStomping = true;
                    szGO = Instantiate(smallZonePrefab, new Vector3(GameObject.FindWithTag("Player").transform.position.x, GameObject.FindWithTag("Player").transform.position.y-.6f, -2.0f), Quaternion.identity ) as GameObject;
                    bzGO = Instantiate(bigZonePrefab, new Vector3(GameObject.FindWithTag("Player").transform.position.x, GameObject.FindWithTag("Player").transform.position.y - .6f, -2.0f), Quaternion.identity) as GameObject;
                    StartCoroutine(zone(stompLength));
                    StartCoroutine(stompAttack(stompLength));
                    
                }
                else
                {
                    isCharging = true;
                    chargeAmount = 3;
                    chargeCoroutine = chargeAttack(0.5f);
                    StartCoroutine(chargeCoroutine);
                }

                
            }

            if (endStomp)
            {
                Destroy(szGO);
                Destroy(bzGO);
                Destroy(zdGO);
                StartCoroutine(attackCycle(attackTiming));
                endStomp = false;
            }

            if (endCharge)
            {
                StartCoroutine(attackCycle(attackTiming));
                endCharge = false;
            }

        }

        //animation trigger for enemy movement
        if ( (moveDirection != Vector3.zero) || (chargeVector != Vector3.zero))
        {

            
            viewDirection(moveDirection);
            

            anim.SetBool("isWalking", true);
            anim.SetBool("isNowStomping", isStomping);
            anim.SetBool("isNowCharging", chargeNow);
            anim.SetBool("isMovingInX", isMovingHoriz);
            anim.SetBool("isMovingPosY", isMovingVertUp);
            anim.SetBool("isMovingNegY", isMovingVertDown);
            
        }
        else //if (!(isCharging && chargeTimer))
        {
            isMovingVertUp = false;
            isMovingHoriz = false;
            isMovingVertDown = false;
            anim.SetBool("isWalking", false);
            anim.SetBool("isMovingInX", isMovingHoriz);
            anim.SetBool("isMovingPosY", isMovingVertUp);
            anim.SetBool("isMovingNegY", isMovingVertDown);
            anim.SetBool("isNowStomping", isStomping);
            anim.SetBool("isNowCharging", chargeNow);
        }


    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        //Debug.Log("Hi");
        
       
        hasCollided = true;
    }


    IEnumerator attackCycle(float wait)
    {
        attacking = false;
        yield return new WaitForSeconds(wait);
        decideAttack = true;
        attacking = true;
        hasCollided = false;
    }

    IEnumerator stompAttack(float wait)
    {
        yield return new WaitForSeconds(wait);
        zdGO = Instantiate(zoneDmgPrefab, new Vector3(GameObject.FindWithTag("smallZone").transform.position.x, GameObject.FindWithTag("smallZone").transform.position.y - .05f, -2.0f), Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(0.5f);//Set buffer till attack cycle commences
        Instantiate(stompEffect, new Vector3(GameObject.FindWithTag("smallZone").transform.position.x, GameObject.FindWithTag("smallZone").transform.position.y - .05f, -2.0f), Quaternion.identity);
        isStomping = false;
        endStomp = true;
    }

    IEnumerator chargeAttack(float wait)
    {
        
        if (chargeAmount == 0)
        {
            isCharging = false;
            endCharge = true;
        }
        else
        {
            chargeAmount--;
            tempLocation = new Vector3(GameObject.FindWithTag("Player").transform.position.x, GameObject.FindWithTag("Player").transform.position.y, 0.0f);
            tempVector = new Vector3(tempLocation.x - transform.position.x, tempLocation.y - transform.position.y, 0.0f);
            chargeNow = true;

           // Debug.Log("passed");
                if (tempVector.x > 0)
                {
                    sprite.flipX = true;
                }
                else
                {
                    sprite.flipX = false;
                }
            //anim.SetBool("isNowCharging", chargeNow);
            if (gameObject.GetComponent<AudioSource>())
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSeconds(wait);
           
            chargeLocation = new Vector3(GameObject.FindWithTag("Player").transform.position.x, GameObject.FindWithTag("Player").transform.position.y, 0.0f);
            chargeVector = new Vector3(chargeLocation.x - transform.position.x, chargeLocation.y - transform.position.y, 0.0f);
            chargeVector.Normalize();
            chargeTimer = true;
            chargeNow = false;

        }

    }

    IEnumerator zone(float appearTime)
    {
        
        for (float t = 0.0f; t < 1.0f; t += 1.0f / (30.0f))
        {
            float alpha = bzGO.GetComponent<SpriteRenderer>().material.color.a;
            Color newColor = new Color(1, 0, 0, Mathf.Lerp(alpha, 255.0f, t/(2*appearTime) ));
            bzGO.GetComponent<SpriteRenderer>().material.color = newColor;

            yield return new WaitForSeconds(1.0f/30.0f);
        }
        
        //Debug.Log("Done");
    }

    IEnumerator FadeTo(float aValue, float fadeTime)
    {
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        float alpha = sprite.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Lerp(alpha, aValue, t));
            sprite.color = newColor;
            yield return null;
        }
        
        if (isMiniBoss && miniBoss != null)
        {
            miniBoss.DropItem();
        }

        Destroy(gameObject);
        Destroy(szGO);
        Destroy(bzGO);
        Destroy(zdGO);
    }

    public void DamageEnemy(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health, maxhealth);

        if (health <= 0)
        {
            healthBar.gameObject.SetActive(false);
            StartCoroutine(FadeTo(alphaValue, fadeDelay));
            
        }

    }

    public void viewDirection(Vector3 moveDirection)
    {

        //for complete random walk
        //xDirection = Random.Range(-1.0f, 1.0f);
        //yDirection = Random.Range(-1.0f, 1.0f);

        //directionVector = new Vector3(xDirection, yDirection, 0.0f);
        //directionVector = directionVector.normalized;
        /////////////////////////

        //Debug.Log(moveDirection.x);
        //Debug.Log(moveDirection.y);

        if (attacking && isStomping)
        {
            isMovingVertUp = false;
            isMovingHoriz = false;
            isMovingVertDown = false;
        }
        else
        {
            //Debug.Log("test");
            if ((moveDirection.y > moveDirection.x) && (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x)))
            {
                isMovingVertUp = true;
                isMovingHoriz = false;
                isMovingVertDown = false;
            }
            else if ((moveDirection.y < moveDirection.x) && (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x)))
            {
                isMovingVertUp = false;
                isMovingHoriz = false;
                isMovingVertDown = true;
            }
            else if ((moveDirection.y < moveDirection.x) && (Mathf.Abs(moveDirection.y) < Mathf.Abs(moveDirection.x)))
            {
                isMovingVertUp = false;
                isMovingHoriz = true;
                isMovingVertDown = false;
                sprite.flipX = true;
            }
            else if ((moveDirection.y > moveDirection.x) && (Mathf.Abs(moveDirection.y) < Mathf.Abs(moveDirection.x)))
            {
                isMovingVertUp = false;
                isMovingHoriz = true;
                isMovingVertDown = false;
                sprite.flipX = false;
            }
        }


    }

}


