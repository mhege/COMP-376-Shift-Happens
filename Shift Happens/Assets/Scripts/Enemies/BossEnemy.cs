using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    private Rigidbody2D theRB;
    private Vector3 moveDirection;
    private Animator anim;

    //Fading Out after Death
    public float fadeDelay = 1f;
    public float alphaValue = 0;
    public bool destroyGameObject = false;

    //Chase Player Enemy Type (Skeleton) and All Other Enemy Types
    [Header("Chase Player")]
    public float rangeToChasePlayer;

    //Enemy should dodge 
    [Header("Dodging")]
    public bool dodges;
    private Vector2 clickedPos;//worldspace click coordinate

    [Header("Variables")]
    public float maxhealth = 5;
    public float health = 5;
    public EnemyHealthBar healthBar;
    public float moveSpeed;
    public bool isInvicible;

    [Header("Damage Counters")]
    public GameObject dmgParentPrefab;
    GameObject dmgTemp;

    //access main script

    GameObject mainGO;
    main mainScript;
    GameObject playerGO;

    //raytrace
    RaycastHit2D hit;

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

    [Header("Variables2")]
    public float moveSpeed2;
    public float rangeToChasePlayer2;
    public float runawayRange;

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

    //Wandering Enemy Type (Blue)
    [Header("Wandering")]
    public float wanderLength;
    public float pauseLength;
    private float wanderCounter;
    private float pauseCounter;
    private Vector3 wanderDirection;

    [Header("Variables3")]
    public float moveSpeed3;

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

    //bossEnemy inserts

    int type;
    float typeChange = 5.0f;
    float currentTypeTime = 0.0f;
    public int typeNow;
    private IEnumerator bigShotCoroutine;
    private IEnumerator largeAttackCycle;
    private IEnumerator laserCycle;
    bool isPrepingLaser = false;
    bool laserStart = false;
    bool laserEnd = false;
    int laserDir = 0;
    float laserTime = 0.0f;
    GameObject laserPrepGO;
    GameObject laserShotGO1;
    GameObject laserShotGO2;
    GameObject laserShotGO3;
    GameObject laserShotGO4;
    public GameObject laserPrepPrefab;
    public GameObject laserShotPrefab;

    int currentLvl;

    // Start is called before the first frame update
    void Start()
    {
        isInvicible = false;
        //type 1
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        mainGO = GameObject.FindGameObjectWithTag("main");
        mainScript = mainGO.GetComponent<main>();
        healthBar.SetHealth(health, maxhealth);

        //type 2
        bigShotCoroutine = bigShotCycle(3.0f);
        //StartCoroutine(bigShotCoroutine);
        currentFP = FPL;
        initialBulletVec = new Vector3(1.0f, 0.0f, 0.0f);

        //type 3
        pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        //StartCoroutine(attackCycle(attackTiming));
        chargeCoroutine = chargeAttack(0.5f);
        largeAttackCycle = attackCycle(attackTiming);

        type = Random.Range(1, 5);
        typeNow = type;

        if(typeNow == 2)
        {
            bigShotCoroutine = bigShotCycle(3.0f);
            StartCoroutine(bigShotCoroutine);
        }

        if(typeNow == 3)
        {
            largeAttackCycle = attackCycle(attackTiming);
            StartCoroutine(largeAttackCycle);
        }

        currentLvl=PlayerPrefs.GetInt("levelAt",3);
    }

    public void FixedUpdate()
    {
        if(type == 1)
            dodge();
    }

    public void dodge()
    {

        if (sprite.isVisible)
        {

            //where in z is player clicking
            Vector3 screenPosDepth = Input.mousePosition;
            screenPosDepth.z = -1.0f;
            clickedPos = Camera.main.ScreenToWorldPoint(screenPosDepth);

            //Based on player firepoint location
            //Vector2 rotatefp = new Vector2(firepointGO.transform.position.x, firepointGO.transform.position.y);
            //rotatefp = rotatefp.Rotate(firepointGO.transform.rotation.eulerAngles.z);
            //Vector2 v = new Vector2(clickedPos.x - rotatefp.x, clickedPos.y - rotatefp.y);
            //v = v.normalized;

            //Base on player center coordinates
            Vector2 playerCenter = new Vector2(playerGO.transform.position.x, playerGO.transform.position.y);
            Vector2 directionVector = new Vector2(clickedPos.x - playerCenter.x, clickedPos.y - playerCenter.y);
            directionVector = directionVector.normalized;

            //raytrace       

            for (int i = 0; i < 60; i++)
            {
                hit = Physics2D.Raycast(new Vector2(playerGO.transform.position.x + directionVector.x, playerGO.transform.position.y + directionVector.y), directionVector);

                if (hit && (hit.collider.gameObject.tag == "boss"))
                {
                    int temp = Random.Range(0, 2);
                    if (temp == 0)
                        temp = -1;
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(temp * Vector2.Perpendicular(directionVector) * 20.0f, ForceMode2D.Impulse);

                    break;
                }


            }

        }

    }

    // Update is called once per frame
    void Update()
    {

        currentTypeTime += Time.deltaTime;

        if(type == 1)
        {
            if (sprite.isVisible) //Enemy is within 
            {
                moveDirection = Vector3.zero;

                if (Vector3.Distance(transform.position, Player.instance.transform.position) < rangeToChasePlayer) //if within range
                {
                    moveDirection = Player.instance.transform.position - transform.position;
                }

                moveDirection.Normalize(); // no matter the direction
                                           //transform.Translate(moveDirection.x * moveSpeed * Time.deltaTime, moveDirection.y * moveSpeed * Time.deltaTime, 0.0f);
                theRB.velocity = moveDirection * moveSpeed; //rb moves with enemy body

            }

            //animation trigger for enemy movement
            if (moveDirection != Vector3.zero)
            {
                viewDirectionSmall();

                anim.SetBool("isMovingInX", isMovingHoriz);
                anim.SetBool("isMovingPosY", isMovingVertUp);
                anim.SetBool("isMovingNegY", isMovingVertDown);
            }
            else
            {
                isMovingVertUp = false;
                isMovingHoriz = false;
                isMovingVertDown = false;

                anim.SetBool("isMovingInX", isMovingHoriz);
                anim.SetBool("isMovingPosY", isMovingVertUp);
                anim.SetBool("isMovingNegY", isMovingVertDown);
            }

            if(currentTypeTime > typeChange)
            {
                type = Random.Range(1, 5);
                typeNow = type;

                if (typeNow == 2)
                {
                    bigShotCoroutine = bigShotCycle(3.0f);
                    StartCoroutine(bigShotCoroutine);
                }

                if (typeNow == 3)
                {
                    largeAttackCycle = attackCycle(attackTiming);
                    StartCoroutine(largeAttackCycle);
                }

                currentTypeTime = 0.0f;
            }

        }else if (type == 2)
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
                        bigShotCoroutine = bigShotCycle(3.0f);
                        StartCoroutine(bigShotCoroutine);
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
                    theRB.velocity = moveDirection * moveSpeed2; //rb moves with enemy body

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
                            bulletTemp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                    }
                }


            }

            timeView += Time.deltaTime;

            //animation trigger for enemy movement
            if (moveDirection != Vector3.zero)
            {
                viewDirectionMedium();

                anim.SetBool("isMovingInX", isMovingHoriz);
                anim.SetBool("isMovingPosY", isMovingVertUp);
                anim.SetBool("isMovingNegY", isMovingVertDown);
            }
            else
            {
                isMovingVertUp = false;
                isMovingHoriz = false;
                isMovingVertDown = false;

                if (timeView > 1.0f)
                {
                    motionlessDirection();//motionless view
                }

                anim.SetBool("isMovingInX", isMovingHoriz);
                anim.SetBool("isMovingPosY", isMovingVertUp);
                anim.SetBool("isMovingNegY", isMovingVertDown);
            }

            if (currentTypeTime > typeChange)
            {
                type = Random.Range(1, 5);
                typeNow = type;
                currentTypeTime = 0.0f;

                if (typeNow == 2)
                {
                    bigShotCoroutine = bigShotCycle(3.0f);
                    StartCoroutine(bigShotCoroutine);
                }

                if (typeNow == 3)
                {
                    largeAttackCycle = attackCycle(attackTiming);
                    StartCoroutine(largeAttackCycle);
                }


                StopCoroutine(bigShotCoroutine);

                fireBullet = false;
                refreshBullet = false;
                chargingBullet = false;
            }

        }
        else if (type == 3)
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
                    //Debug.Log("Hi");
                    chargeAmount = 0;
                    isCharging = false;
                    chargeNow = false;
                    endCharge = false;
                    chargeTimer = false;
                    hasCollided = false;
                    StopCoroutine(chargeCoroutine);

                    if (currentTypeTime > typeChange)
                    {

                        chargeAmount = 0;
                        isCharging = false;
                        chargeNow = false;
                        endCharge = false;
                        chargeTimer = false;
                        hasCollided = false;
                        chargeVector = Vector3.zero;
                        StopCoroutine(chargeCoroutine);

                        Destroy(szGO);
                        Destroy(bzGO);
                        Destroy(zdGO);

                        type = Random.Range(1, 5);
                        typeNow = type;
                        currentTypeTime = 0.0f;

                        StopCoroutine(largeAttackCycle);

                        if (typeNow == 2)
                        {
                            bigShotCoroutine = bigShotCycle(3.0f);
                            StartCoroutine(bigShotCoroutine);
                        }

                        if (typeNow == 3)
                        {
                            largeAttackCycle = attackCycle(attackTiming);
                            StartCoroutine(largeAttackCycle);
                        }
                    }
                    else
                    {
                        largeAttackCycle = attackCycle(attackTiming);
                        StartCoroutine(largeAttackCycle);
                    }
                        
                }
                else if ((isCharging && chargeTimer))
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

                    //Debug.Log("safe");
                    moveDirection = Vector3.zero;
                    transform.Translate(chargeVector.x * chargeSpeed * Time.deltaTime, chargeVector.y * chargeSpeed * Time.deltaTime, 0.0f);
                    viewDirectionLarge(chargeVector);

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
                    theRB.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    viewDirectionLarge(moveDirection);
                }
                else
                {

                    theRB.velocity = moveDirection * moveSpeed3; //change this all to translate
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
                        szGO = Instantiate(smallZonePrefab, new Vector3(GameObject.FindWithTag("Player").transform.position.x, GameObject.FindWithTag("Player").transform.position.y - .6f, -2.0f), Quaternion.identity) as GameObject;
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
                    endStomp = false;

                    if (currentTypeTime > typeChange)
                    {

                        chargeAmount = 0;
                        isCharging = false;
                        chargeNow = false;
                        endCharge = false;
                        chargeTimer = false;
                        hasCollided = false;
                        chargeVector = Vector3.zero;
                        StopCoroutine(chargeCoroutine);

                        Destroy(szGO);
                        Destroy(bzGO);
                        Destroy(zdGO);

                        type = Random.Range(1, 5);
                        typeNow = type;
                        currentTypeTime = 0.0f;

                        StopCoroutine(largeAttackCycle);

                        if (typeNow == 2)
                        {
                            bigShotCoroutine = bigShotCycle(3.0f);
                            StartCoroutine(bigShotCoroutine);
                        }

                        if (typeNow == 3)
                        {
                            largeAttackCycle = attackCycle(attackTiming);
                            StartCoroutine(largeAttackCycle);
                        }
                    }
                    else
                    {
                        largeAttackCycle = attackCycle(attackTiming);
                        StartCoroutine(largeAttackCycle);
                    }
                        
                    
                }

                if (endCharge)
                {
                    endCharge = false;

                    if (currentTypeTime > typeChange)
                    {

                        chargeAmount = 0;
                        isCharging = false;
                        chargeNow = false;
                        endCharge = false;
                        chargeTimer = false;
                        hasCollided = false;
                        StopCoroutine(chargeCoroutine);
                        chargeVector = Vector3.zero;
                        Destroy(szGO);
                        Destroy(bzGO);
                        Destroy(zdGO);

                        type = Random.Range(1, 5);
                        typeNow = type;
                        currentTypeTime = 0.0f;

                        StopCoroutine(largeAttackCycle);

                        if (typeNow == 2)
                        {
                            bigShotCoroutine = bigShotCycle(3.0f);
                            StartCoroutine(bigShotCoroutine);
                        }

                        if (typeNow == 3)
                        {
                            largeAttackCycle = attackCycle(attackTiming);
                            StartCoroutine(largeAttackCycle);
                        }
                    }
                    else
                    {
                        largeAttackCycle = attackCycle(attackTiming);
                        StartCoroutine(largeAttackCycle);
                    }
                        
                    
                }

            }

            //animation trigger for enemy movement
            if ( (moveDirection != Vector3.zero) || (chargeVector != Vector3.zero))
            {
                if (moveDirection != Vector3.zero)
                    viewDirectionLarge(moveDirection);
                else
                {
                    //Debug.Log("Check");
                    viewDirectionLarge(chargeVector);
                }
                    

                anim.SetBool("isNowStomping", isStomping);
                anim.SetBool("isNowCharging", chargeNow);
                anim.SetBool("isMovingInX", isMovingHoriz);
                anim.SetBool("isMovingPosY", isMovingVertUp);
                anim.SetBool("isMovingNegY", isMovingVertDown);
            }
            else
            {
                isMovingVertUp = false;
                isMovingHoriz = false;
                isMovingVertDown = false;
                
                anim.SetBool("isMovingInX", isMovingHoriz);
                anim.SetBool("isMovingPosY", isMovingVertUp);
                anim.SetBool("isMovingNegY", isMovingVertDown);
                anim.SetBool("isNowStomping", isStomping);
                anim.SetBool("isNowCharging", chargeNow);

            }

        }
        else if (type == 4)
        {
            //Not implemented yet.
            // Debug.Log("Type 4!");

            moveDirection = Vector3.zero;
            theRB.velocity = Vector3.zero;

            if (!laserStart)
            {
                isInvicible = true;
                gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                //Debug.Log("Hi");
                laserCycle = laserAttack(3.0f);
                StartCoroutine(laserCycle);
                laserStart = true;
            }


            if (laserEnd)
            {
                isInvicible = false;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                type = Random.Range(1, 5);
                typeNow = type;
                currentTypeTime = 0.0f;

                if (typeNow == 2)
                {
                    bigShotCoroutine = bigShotCycle(3.0f);
                    StartCoroutine(bigShotCoroutine);
                }

                if (typeNow == 3)
                {
                    largeAttackCycle = attackCycle(attackTiming);
                    StartCoroutine(largeAttackCycle);
                }

                laserStart = false;
                laserEnd = false;
            }
            
            
        }
       

    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        //Debug.Log("Hi");


        hasCollided = true;
    }

    IEnumerator laserAttack(float wait)
    {

        isPrepingLaser = true;
        isMovingVertUp = false;
        isMovingHoriz = false;
        isMovingVertDown = false;
        isStomping = false;
        chargeNow = false;

        anim.SetBool("isMovingInX", isMovingHoriz);
        anim.SetBool("isMovingPosY", isMovingVertUp);
        anim.SetBool("isMovingNegY", isMovingVertDown);
        anim.SetBool("isNowStomping", isStomping);
        anim.SetBool("isNowCharging", chargeNow);
        anim.SetBool("isNowPrepingLaser", isPrepingLaser);

        yield return new WaitForSeconds(1.0f);

        laserPrepGO = Instantiate(laserPrepPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z-.1f), Quaternion.identity) as GameObject;

        //Debug.Log("hi2");

        yield return new WaitForSeconds(wait-1.0f);

        Destroy(laserPrepGO);

        //laserMotionlessDirection();

        laserShotGO1 = Instantiate(laserShotPrefab, new Vector3(transform.position.x+.1f, transform.position.y - 3.0f, transform.position.z-.1f), Quaternion.Euler(0, 0, 90)) as GameObject;
        laserShotGO2 = Instantiate(laserShotPrefab, new Vector3(transform.position.x-.1f, transform.position.y + 3.0f, transform.position.z+.1f), Quaternion.Euler(0, 0, -90)) as GameObject;
        laserShotGO3 = Instantiate(laserShotPrefab, new Vector3(transform.position.x - 3.0f, transform.position.y-.1f, transform.position.z-.1f), Quaternion.identity) as GameObject;
        laserShotGO4 = Instantiate(laserShotPrefab, new Vector3(transform.position.x + 3.0f, transform.position.y+.1f, transform.position.z-.1f), Quaternion.Euler(0, 0, 180)) as GameObject;
               
        //Quaternion.Euler(0, 0, bulletRotation)
        yield return new WaitForSeconds(wait - 1.0f);

        isPrepingLaser = false;
        anim.SetBool("isNowPrepingLaser", isPrepingLaser);

        Destroy(laserShotGO1);
        Destroy(laserShotGO2);
        Destroy(laserShotGO3);
        Destroy(laserShotGO4);

        laserEnd = true;

        //Debug.Log("hi3");
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
            Color newColor = new Color(1, 0, 0, Mathf.Lerp(alpha, 255.0f, t / (2 * appearTime)));
            bzGO.GetComponent<SpriteRenderer>().material.color = newColor;

            yield return new WaitForSeconds(1.0f / 30.0f);
        }

        //Debug.Log("Done");
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
        float alpha = sprite.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Lerp(alpha, aValue, t));
            sprite.color = newColor;
            yield return null;
        }


        Destroy(gameObject);
        Destroy(szGO);
        Destroy(bzGO);
        Destroy(zdGO);
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LevelComplete();
        PlayerPrefs.SetInt("levelAt",currentLvl+1);
    }

    public void DamageEnemy(float damage)
    {
        if (!isInvicible)
        {
            health -= damage;
            healthBar.SetHealth(health, maxhealth);

            if (health <= 0)
            {
                healthBar.gameObject.SetActive(false);
                StartCoroutine(FadeTo(alphaValue, fadeDelay));
            }
        }
    }

    public void laserMotionlessDirection()
    {
        if (((Player.instance.transform.position.y - transform.position.y) > (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) > (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))))
        {
            laserDir = 1;
            isMovingVertUp = true;
            isMovingHoriz = false;
            isMovingVertDown = false;
        }
        else if (((Player.instance.transform.position.y - transform.position.y) < (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) > (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))))
        {
            laserDir = 2;
            isMovingVertUp = false;
            isMovingHoriz = false;
            isMovingVertDown = true;
        }
        else if (((Player.instance.transform.position.y - transform.position.y) < (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) < (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))))
        {
            laserDir = 3;
            isMovingVertUp = false;
            isMovingHoriz = true;
            isMovingVertDown = false;
            sprite.flipX = true;
        }
        else if (((Player.instance.transform.position.y - transform.position.y) > (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) < (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))))
        {
            laserDir = 4;
            isMovingVertUp = false;
            isMovingHoriz = true;
            isMovingVertDown = false;
            sprite.flipX = false;
        }
    }

    public void motionlessDirection()
    {
        if (((Player.instance.transform.position.y - transform.position.y) > (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) > (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))))
        {
            currentFP = FPU;
            isMovingVertUp = true;
            isMovingHoriz = false;
            isMovingVertDown = false;
        }
        else if (((Player.instance.transform.position.y - transform.position.y) < (Player.instance.transform.position.x - transform.position.x)) && ((Mathf.Abs(Player.instance.transform.position.y - transform.position.y)) > (Mathf.Abs(Player.instance.transform.position.x - transform.position.x))))
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
    }

    public void viewDirectionMedium()
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

    public void viewDirectionSmall()
    {

        //for complete random walk
        //xDirection = Random.Range(-1.0f, 1.0f);
        //yDirection = Random.Range(-1.0f, 1.0f);

        //directionVector = new Vector3(xDirection, yDirection, 0.0f);
        //directionVector = directionVector.normalized;
        /////////////////////////

        //Debug.Log(moveDirection.x);
        //Debug.Log(moveDirection.y);

        if ((moveDirection.y > moveDirection.x) && (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x)))
        {
            //Debug.Log(moveDirection.x);
            //Debug.Log(moveDirection.y);
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

    public void viewDirectionLarge(Vector3 moveDirection)
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

