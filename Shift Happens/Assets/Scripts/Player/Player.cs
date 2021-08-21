using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public static Player instance;
    public GameManager gameManager;

    // Health
    int humanHealth;
    int alienHealth;
    int robotHealth;
    int humanInitialHealth = 100;
    int alienInitialHealth = 50;
    int robotInitialHealth = 150;


    public bool godMode;

    public SwitchCharacterScript switchCharacter;

    public HealthBar humanHealthBar;
    public HealthBar alienHealthBar;
    public HealthBar robotHealthBar;

    public bool isInvincible;

    Vector3 temp;

    public Rigidbody2D rb;
    bool movePlayer = false;
    bool movePlayerBoss = false;

    public GameObject bulletCollisionEffect;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (godMode)
        {
            humanInitialHealth = 1000000;
            alienInitialHealth = 1000000;
            robotInitialHealth = 1000000;
        }

        humanHealth = humanInitialHealth;
        alienHealth = alienInitialHealth;
        robotHealth = robotInitialHealth;

        // Set health to max at start
        humanHealthBar.SetMaxHealth(humanInitialHealth);
        alienHealthBar.SetMaxHealth(alienInitialHealth);
        robotHealthBar.SetMaxHealth(robotInitialHealth);
        isInvincible = false;
    }

    void Update()
    {
        // Set health bar to current health
        humanHealthBar.SetHealth(humanHealth);
        alienHealthBar.SetHealth(alienHealth);
        robotHealthBar.SetHealth(robotHealth);

         if(humanHealthBar.HealthAmount() <= 0 || alienHealthBar.HealthAmount() <= 0 || robotHealthBar.HealthAmount() <= 0)
        {
            //Destroy(gameObject);
            gameManager.GameOver();
        }
    }

    void FixedUpdate()
    {
        if (movePlayer)
        {
            rb.AddForce(temp * 50.0f, ForceMode2D.Impulse);
            movePlayer = false;
        }

        if (movePlayerBoss)
        {
            rb.AddForce(temp * 50.0f, ForceMode2D.Impulse);
            movePlayerBoss = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvincible)
        {
            if(collision.tag == "small")
            {
                if (collision.gameObject.GetComponent<AudioSource>() && !collision.gameObject.GetComponent<AudioSource>().isPlaying)
                {
                    collision.gameObject.GetComponent<AudioSource>().Play();
                }
                takeDamage(1);
            }

            if (collision.tag == "mediumBullet")
            {
                Instantiate(bulletCollisionEffect, collision.transform.position, Quaternion.identity);
                takeDamage(5);
            }

            if ( (collision.tag == "boss") && (collision.gameObject.GetComponent<BossEnemy>().typeNow == 1) )
            {
                //Debug.Log("one");
                takeDamage(5);
                temp = new Vector3(transform.position.x - collision.gameObject.transform.position.x, transform.position.y - collision.gameObject.transform.position.y, 0.0f);
                temp.Normalize();
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-temp * 50.0f, ForceMode2D.Impulse);
            }
            else if ((collision.tag == "boss") && (collision.gameObject.GetComponent<BossEnemy>().typeNow == 3))
            {
                //Debug.Log("two");
                takeDamage(20);
                temp = new Vector3(transform.position.x - collision.gameObject.transform.position.x, transform.position.y - collision.gameObject.transform.position.y, 0.0f);
                temp.Normalize();
                //move Player
                movePlayerBoss = true;
                //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-temp * 50.0f, ForceMode2D.Impulse);
            }

            else if((collision.tag == "boss"))
            {
                //Debug.Log("two");
                takeDamage(10);
                temp = new Vector3(transform.position.x - collision.gameObject.transform.position.x, transform.position.y - collision.gameObject.transform.position.y, 0.0f);
                temp.Normalize();
                //move Player
                movePlayerBoss = true;
                //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-temp * 50.0f, ForceMode2D.Impulse);
            }

            if (collision.tag == "large")
            {
                takeDamage(20);
                temp = new Vector3(transform.position.x - collision.gameObject.transform.position.x, transform.position.y - collision.gameObject.transform.position.y, 0.0f);
                temp.Normalize();
                //move player
                movePlayer = true;
                //rb.AddForce(temp * 200.0f, ForceMode2D.VelocityChange);
            }
            

            if (collision.tag == "zoneDmg")
            {
                takeDamage(50);
                //set up explosion
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isInvincible)
        {
            if (collision.tag == "small")
            {
                temp = new Vector3(transform.position.x - collision.gameObject.transform.position.x, transform.position.y - collision.gameObject.transform.position.y, 0.0f);
                temp.Normalize();
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-temp * 50.0f, ForceMode2D.Impulse);
            }

            if ((collision.tag == "boss") && (collision.gameObject.GetComponent<BossEnemy>().typeNow == 1))
            {
                //takeDamage(10);
                temp = new Vector3(transform.position.x - collision.gameObject.transform.position.x, transform.position.y - collision.gameObject.transform.position.y, 0.0f);
                temp.Normalize();
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-temp * 50.0f, ForceMode2D.Impulse);
            }else if ((collision.tag == "boss"))
            {
                //takeDamage(20);
                temp = new Vector3(transform.position.x - collision.gameObject.transform.position.x, transform.position.y - collision.gameObject.transform.position.y, 0.0f);
                temp.Normalize();
                //move Player
                movePlayerBoss = true;
                //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-temp * 50.0f, ForceMode2D.Impulse);
            }

            if ((collision.tag == "lasers") )
            {
                takeDamage(1);
                temp = new Vector3(transform.position.x - collision.gameObject.transform.position.x, transform.position.y - collision.gameObject.transform.position.y, 0.0f);
                temp.Normalize();
                //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-temp * 50.0f, ForceMode2D.Impulse);
            }

            
            if (collision.tag == "large")
            {
                //takeDamage(20);
                temp = new Vector3(transform.position.x - collision.gameObject.transform.position.x, transform.position.y - collision.gameObject.transform.position.y, 0.0f);
                temp.Normalize();
                //Move Player
                movePlayer = true;
                //rb.AddForce(-temp * 20.0f, ForceMode2D.Impulse);
            }
            
        }
    }

    public void takeDamage(int damage)
    {
        switch (switchCharacter.getActiveAvatar())
        {
            case 1:
                humanHealth -= damage;
                if (!GameObject.Find("HumanHit").GetComponent<AudioSource>().isPlaying)
                {
                    GameObject.Find("HumanHit").GetComponent<AudioSource>().Play();
                }
                break;
            case 2:
                alienHealth -= damage;
                if (!GameObject.Find("AlienHit").GetComponent<AudioSource>().isPlaying)
                {
                    GameObject.Find("AlienHit").GetComponent<AudioSource>().Play();
                }
                break;
            case 3:
                robotHealth -= damage;
                if (!GameObject.Find("RobotHit").GetComponent<AudioSource>().isPlaying)
                {
                    GameObject.Find("RobotHit").GetComponent<AudioSource>().Play();
                }
                break;
            default:
                break;
        }
            
    }

     public void restoreHealth(int health)
    {
        switch (switchCharacter.getActiveAvatar())
        {
            case 1:
                humanHealth += health;
                if(humanHealth >= humanInitialHealth) humanHealth = humanInitialHealth;
                break;
            case 2:
                alienHealth += health;
                if (alienHealth >= alienInitialHealth) alienHealth = alienInitialHealth;
                break;
            case 3:
                robotHealth += health;
                if (robotHealth >= robotInitialHealth) robotHealth = robotInitialHealth;
                break;
        }
    }

    public int currentHealth()
    {
        int health=0;
        switch (switchCharacter.getActiveAvatar())
        {
            case 1:
                health = humanHealth;
                break;
            case 2:
                health = alienHealth;
                break;
            case 3:
                health = robotHealth;
                break;
        }

        return health;
    }

    public int currentMaxHealth()
    {
        int health = 0;
        switch (switchCharacter.getActiveAvatar())
        {
            case 1:
                health = humanInitialHealth;
                break;
            case 2:
                health = alienInitialHealth;
                break;
            case 3:
                health = robotInitialHealth;
                break;
        }

        return health;
    }
}
