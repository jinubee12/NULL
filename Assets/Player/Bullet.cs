using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attackDamage = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Health enemyHealth = collision.gameObject.GetComponent<Health>();

            // Null check to avoid potential errors
            if (enemyHealth != null)
            {
                enemyHealth.UpdateHealth(-attackDamage);

                // Check if the enemy's health is 0 or below
                if (enemyHealth.health <= 0f)
                {
                    Destroy(collision.gameObject);
                }
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
