using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemy : MonoBehaviour
{
    public Rigidbody2D theRB;
    private Vector3 moveDirection;
    private Animator anim;

    //Fading Out after Death
    public float fadeDelay = 1f;
    public float alphaValue = 0;
    public bool destroyGameObject = false;

    [Header("Shooting")]
    public GameObject bullet;
    public Transform FPU;
    public Transform FPD;
    public Transform FPR;
    public Transform FPL;
    private Transform currentFP;
    public float fireRate;
    private float fireCounter;
    public float shootRange; //range in which enemy shoots at player

    //bullet rotation
    float bulletRotation;
    Vector3 initialBulletVec;

    [Header("Variables")]
    public float maxhealth = 5;
    public float health = 5;
    public EnemyHealthBar healthBar;
    public float moveSpeed;
    public float rangeToChasePlayer;
    public float runawayRange;

    [Header("Damage Counters")]
    public GameObject dmgParentPrefab;
    GameObject dmgTemp;

    //animation direction
    private SpriteRenderer sprite;
    bool isMovingVertUp = false;
    bool isMovingHoriz = false;
    bool isMovingVertDown = false;

    //timer
    float timeView;

    //temp bullet GO
    GameObject bulletTemp;

    //BigShot
    bool chargingBullet = false;
    bool fireBullet = false;
    bool refreshBullet = false;

    [Header("MiniBoss")]
    public bool isMiniBoss;
    public MiniBoss miniBoss;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(bigShotCycle(3.0f));
        currentFP = FPL;
        initialBulletVec = new Vector3(1.0f, 0.0f, 0.0f);

        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        healthBar.SetHealth(health, maxhealth);
    }

    // Update is called once per frame
    void Update()
    {

        if (sprite.isVisible) //Enemy is within 
        {
            moveDirection = Vector3.zero;

            if (chargingBullet)
            {

                if (fireBullet)
                {
                    bulletRotation = Vector3.Angle(initialBulletVec, new Vector3(Player.instance.transform.position.x - transform.position.x, Player.instance.transform.position.y - transform.position.y, Player.instance.transform.position.z - transform.position.z));

                    if ((Player.instance.transform.position.y - transform.position.y) < 0)
                        bulletRotation = 180 + (180 - bulletRotation);

                    bulletTemp = Instantiate(bullet, currentFP.position, Quaternion.Euler(0, 0, bulletRotation)) as GameObject;//place firepoint here if needed
                    bulletTemp.transform.localScale = new Vector3(1.5f, 1.5f, 2.0f);
                    
                    fireBullet = false;
                }

                if (refreshBullet)
                {
                    StartCoroutine(bigShotCycle(3.0f));
                    refreshBullet = false;
                    chargingBullet = false;
                }

            }
            else
            {
                if (Vector3.Distance(transform.position, Player.instance.transform.position) > (3.5)) //if within range
                {
                    timeView = 0.0f;
                    moveDirection = Player.instance.transform.position - transform.position;
                }


                else if (Vector3.Distance(transform.position, Player.instance.transform.position) < (2.5))
                {
                    timeView = 0.0f;
                    moveDirection = transform.position - Player.instance.transform.position;

                }

      

                moveDirection.Normalize(); // no matter the direction
                theRB.velocity = moveDirection * moveSpeed; //rb moves with enemy body

                if(theRB.velocity.magnitude > 0)
                {
                    anim.SetBool("isWalking", true);
                }
                else
                {
                    anim.SetBool("isWalking", false);

                }

                if (Vector3.Distance(transform.position, Player.instance.transform.position) < shootRange)  
                {
                    fireCounter -= Time.deltaTime;

                    bulletRotation = Vector3.Angle(initialBulletVec, new Vector3(Player.instance.transform.position.x - transform.position.x, Player.instance.transform.position.y - transform.position.y, Player.instance.transform.position.z - transform.position.z));

                    if ((Player.instance.transform.position.y - transform.position.y) < 0)
                        bulletRotation = 180 + (180 - bulletRotation);

                    //Debug.Log(bulletRotation);
                    if (fireCounter <= 0)
                    {
                        fireCounter = fireRate;
                        bulletTemp = Instantiate(bullet, currentFP.position, Quaternion.Euler(0, 0, bulletRotation)) as GameObject;//place firepoint here if needed
                        bulletTemp.transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
                    }
                }
            }


        }

        timeView += Time.deltaTime;
        
        //animation trigger for enemy movement
        if (moveDirection != Vector3.zero)
        {

            viewDirection();

            anim.SetBool("isMovingInX", isMovingHoriz);
            anim.SetBool("isMovingPosY", isMovingVertUp);
            anim.SetBool("isMovingNegY", isMovingVertDown);
            


        }
        else
        {
            isMovingVertUp = false;
            isMovingHoriz = false;
            isMovingVertDown = false;

            if(timeView > 1.0f)
            {
                motionlessDirection();//motionless view
            }
                
            anim.SetBool("isMovingInX", isMovingHoriz);
            anim.SetBool("isMovingPosY", isMovingVertUp);
            anim.SetBool("isMovingNegY", isMovingVertDown);
            

        }

    }

    IEnumerator bigShotCycle(float wait)
    {
        yield return new WaitForSeconds(wait);
        chargingBullet = true;
        yield return new WaitForSeconds(1.0f);
        fireBullet = true;
        yield return new WaitForSeconds(1.0f);
        refreshBullet = true;
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

    public void motionlessDirection()
    {
        if ( ((Player.instance.transform.position.y-transform.position.y) > (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) > (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))) )
        {
            currentFP = FPU;
            isMovingVertUp = true;
            isMovingHoriz = false;
            isMovingVertDown = false;
        }else if (((Player.instance.transform.position.y - transform.position.y) < (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) > (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))))
        {
            currentFP = FPD;
            isMovingVertUp = false;
            isMovingHoriz = false;
            isMovingVertDown = true;
        }
        else if (((Player.instance.transform.position.y - transform.position.y) < (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) < (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))))
        {
            currentFP = FPR;
            isMovingVertUp = false;
            isMovingHoriz = true;
            isMovingVertDown = false;
            sprite.flipX = true;
        }
        else if (((Player.instance.transform.position.y - transform.position.y) > (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) < (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))))
        {
            currentFP = FPL;
            isMovingVertUp = false;
            isMovingHoriz = true;
            isMovingVertDown = false;
            sprite.flipX = false;
        }
        if (((Player.instance.transform.position.y - transform.position.y) > (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) > (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))))
        {
            currentFP = FPU;
            isMovingVertUp = true;
            isMovingHoriz = false;
            isMovingVertDown = false;
        }
    }

    public void viewDirection()
    {

        //for complete random walk
        //xDirection = Random.Range(-1.0f, 1.0f);
        //yDirection = Random.Range(-1.0f, 1.0f);

        //directionVector = new Vector3(xDirection, yDirection, 0.0f);
        //directionVector = directionVector.normalized;
        /////////////////////////
        
        if ((moveDirection.y > moveDirection.x) && (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x)))
        {
            currentFP = FPU;
            isMovingVertUp = true;
            isMovingHoriz = false;
            isMovingVertDown = false;

        }
        else if ((moveDirection.y < moveDirection.x) && (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x)))
        {
            currentFP = FPD;
            isMovingVertUp = false;
            isMovingHoriz = false;
            isMovingVertDown = true;

        }
        else if ((moveDirection.y < moveDirection.x) && (Mathf.Abs(moveDirection.y) < Mathf.Abs(moveDirection.x)))
        {
            currentFP = FPR;
            isMovingVertUp = false;
            isMovingHoriz = true;
            isMovingVertDown = false;
            sprite.flipX = true;

        }
        else if ((moveDirection.y > moveDirection.x) && (Mathf.Abs(moveDirection.y) < Mathf.Abs(moveDirection.x)))
        {
            currentFP = FPL;
            isMovingVertUp = false;
            isMovingHoriz = true;
            isMovingVertDown = false;
            sprite.flipX = false;

        }
       


    }

}

