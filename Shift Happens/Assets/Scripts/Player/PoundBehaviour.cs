using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoundBehaviour : MonoBehaviour
{

    public float Speed = 5;
    public Vector3 LaunchOffSet;
    public bool thrown;
    public GameObject impactEffect;
    public float damageToGive = 3;
    public float splashRange = 2.0f;
    [SerializeField]
    GameObject explosionPrefab;
    [SerializeField]
    public float knockback;


    void Start()
    {
        if (thrown)
        {        
           Vector3 scaleChange = new Vector3(1, 1f, 1.139f);
           this.transform.localScale = scaleChange;
        }
        StartCoroutine(Explotion());

    }

    // Update is called once per frame
    void Update()
    {
        if (!thrown)
        {
            transform.position += -transform.right * Speed * Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("small") || other.CompareTag("medium") || other.CompareTag("large") || other.CompareTag("boss"))
        {

            if (splashRange > 0)
            {
                var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);
                foreach (var hitCollider in hitColliders)
                {
                    var enemy1 = hitCollider.GetComponent<SmallEnemy>();
                    var enemy2 = hitCollider.GetComponent<MediumEnemy>();
                    var enemy3 = hitCollider.GetComponent<LargeEnemy>();
                    var enemy4 = hitCollider.GetComponent<BossEnemy>();


                    if (enemy1)
                    {
                       /* Rigidbody2D smallEnemy = other.GetComponent<Rigidbody2D>();
                        smallEnemy.isKinematic = false;a
                        Vector2 difference = smallEnemy.transform.position - transform.position;
                        difference = difference.normalized * knockback;
                        smallEnemy.AddForce(difference, ForceMode2D.Impulse);
                        smallEnemy.isKinematic = true;*/

                        enemy1.DamageEnemy(1);
                    }
                    if (enemy2)
                    {
                        Rigidbody2D mediumEnemy = other.GetComponent<Rigidbody2D>();
                        mediumEnemy.isKinematic = false;
                        Vector2 difference = mediumEnemy.transform.position - transform.position;
                        difference = difference.normalized * knockback;
                        mediumEnemy.AddForce(difference, ForceMode2D.Impulse);
                        mediumEnemy.isKinematic = true;
                     
                        enemy2.DamageEnemy(1);
                    }
                    if (enemy3)
                    {
                        Rigidbody2D largeEnemy = other.GetComponent<Rigidbody2D>();
                        largeEnemy.isKinematic = false;
                        Vector2 difference = largeEnemy.transform.position - transform.position;
                        difference = difference.normalized * knockback;
                        largeEnemy.AddForce(difference, ForceMode2D.Impulse);
                        largeEnemy.isKinematic = true;
                        enemy3.DamageEnemy(1);
                    }
                    if (enemy4)
                    {
                        Rigidbody2D bossEnemy = other.GetComponent<Rigidbody2D>();
                        bossEnemy.isKinematic = false;
                        Vector2 difference = bossEnemy.transform.position - transform.position;
                        difference = difference.normalized * knockback;
                        bossEnemy.AddForce(difference, ForceMode2D.Impulse);
                        bossEnemy.isKinematic = true;
                        enemy4.DamageEnemy(1);
                    }
                }

            }
            bombImpact();
        }

    }

    private void bombImpact()
    {
        Destroy(gameObject);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    private IEnumerator Explotion()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
