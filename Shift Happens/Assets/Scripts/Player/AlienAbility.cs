using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienAbility : MonoBehaviour
{


    // Start is called before the first frame update
    public ShootingGun player;
    [HideInInspector] public float cooldownTime = 5;
    public Image imageCooldown;
    bool isColdown = false;
    Transform firePoint;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;

    void Start()
    {
        cooldownTime = 4;
        firePoint = player.firePoint;
    }

    // Update is called once per frame
    void Update()
    {
        firePoint = player.firePoint;
        if (!isColdown)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                StartCoroutine(Shoot());
                isColdown = true;
            }
        }
        if (isColdown)
        {
            imageCooldown.fillAmount += 1 / cooldownTime * Time.deltaTime;
            if (imageCooldown.fillAmount >= 1)
            {
                imageCooldown.fillAmount = 0;
                isColdown = false;
            }
        }
    }

    IEnumerator Shoot()
    {



        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        //if(lineRenderer.gameObject.GetComponent<AudioSource>())
            lineRenderer.gameObject.GetComponent<AudioSource>().Play();

        if (hitInfo)
            {
                SmallEnemy smallEnemy = hitInfo.transform.GetComponent<SmallEnemy>();
                MediumEnemy mediumEnemy = hitInfo.transform.GetComponent<MediumEnemy>();
                LargeEnemy largeEnemy = hitInfo.transform.GetComponent<LargeEnemy>();
                BossEnemy bossEnemy = hitInfo.transform.GetComponent<BossEnemy>();

            if (smallEnemy != null)
                {
                    smallEnemy.DamageEnemy(3);
                }
            if (mediumEnemy != null)
            {
                mediumEnemy.DamageEnemy(3);
            }
            if (largeEnemy != null)
            {
                largeEnemy.DamageEnemy(3);
            }
            if (bossEnemy != null)
            {
                bossEnemy.DamageEnemy(3);
            }


            Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
                lineRenderer.SetPosition(0, firePoint.position);
                lineRenderer.SetPosition(1, hitInfo.point);
            }

            else
            {
                lineRenderer.SetPosition(0, firePoint.position);
                lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
            }

            lineRenderer.enabled = true;

        yield return 0;

            lineRenderer.enabled = false;
       
        
    }
}
