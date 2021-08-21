using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHP : MonoBehaviour
{
    private Player player;
    public int hpRestored;

    public void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player.currentHealth() < player.currentMaxHealth())
            {
                player.restoreHealth(hpRestored);
                Destroy(gameObject);
            }
        }
    }
}
