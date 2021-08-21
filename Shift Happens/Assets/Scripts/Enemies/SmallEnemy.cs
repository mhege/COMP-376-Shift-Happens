using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : MonoBehaviour
{
    private Rigidbody2D theRB;
    private Vector3 moveDirection;
    private Animator anim;

    //Fading Out after Death
    public float fadeDelay = 0.5f;
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

    [Header("Damage Counters")]
    public GameObject dmgParentPrefab;
    GameObject dmgTemp;

    //access main script

    GameObject mainGO;
    main mainScript;
    GameObject playerGO;

    //raytrace
    RaycastHit2D hit;

    //animation direction
    private SpriteRenderer sprite;
    bool isMovingVertUp = false;
    bool isMovingHoriz = false;
    bool isMovingVertDown = false;

    //bossEnemy inserts

    int type;
    [Header("MiniBoss")]
    public bool isMiniBoss;
    public MiniBoss miniBoss;

    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerGO = GameObject.FindGameObjectWithTag("Player");

        mainGO = GameObject.FindGameObjectWithTag("main");
        mainScript = mainGO.GetComponent<main>();

        
        healthBar.SetHealth(health, maxhealth);

        type = Random.Range(1, 4);
    }

    public void FixedUpdate()
    {
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

                if (hit && (hit.collider.gameObject.tag == "small"))
                {
                    int temp = Random.Range(0, 2);
                    if (temp == 0)
                        temp = -1;

                    hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(temp * Vector2.Perpendicular(directionVector) * 3.0f, ForceMode2D.Impulse);

                    break;
                }


            }

        }

    }

    // Update is called once per frame
    void Update()
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

            anim.SetBool("isMovingInX", isMovingHoriz);
            anim.SetBool("isMovingPosY", isMovingVertUp);
            anim.SetBool("isMovingNegY", isMovingVertDown);
        }

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

        if (isMiniBoss && miniBoss!=null)
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

    public void viewDirection()
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

}

