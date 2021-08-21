using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float Speed = 4;
    public Vector3 LaunchOffSet;
    public bool thrown;
    public GameObject impactEffect;
    public float damageToGive = 3;
    public float splashRange =1.0f;
    [SerializeField]
    GameObject explosionPrefab;


    void Start()
    {
        if (thrown)
        {
            var direction = transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction*Speed, ForceMode2D.Impulse);
        }
        transform.Translate(LaunchOffSet);        
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
                        var closestPoint = hitCollider.ClosestPoint(transform.position);
                        var distance = Vector2.Distance(closestPoint, transform.position);
                        var damagePercent = Mathf.InverseLerp(splashRange, 0, distance);
                        enemy1.DamageEnemy(damagePercent * damageToGive);
                    }
                    if (enemy2)
                    {
                        var closestPoint = hitCollider.ClosestPoint(transform.position);
                        var distance = Vector2.Distance(closestPoint, transform.position);
                        var damagePercent = Mathf.InverseLerp(splashRange, 0, distance);
                        enemy2.DamageEnemy(damagePercent * damageToGive);
                    }
                    if (enemy3)
                    {
                        var closestPoint = hitCollider.ClosestPoint(transform.position);
                        var distance = Vector2.Distance(closestPoint, transform.position);
                        var damagePercent = Mathf.InverseLerp(splashRange, 0, distance);
                        enemy3.DamageEnemy(damagePercent * damageToGive);
                    }
                    if (enemy4)
                    {
                        var closestPoint = hitCollider.ClosestPoint(transform.position);
                        var distance = Vector2.Distance(closestPoint, transform.position);
                        var damagePercent = Mathf.InverseLerp(splashRange, 0, distance);
                        enemy4.DamageEnemy(damagePercent * damageToGive);
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
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
