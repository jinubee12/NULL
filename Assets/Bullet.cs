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
            collision.gameObject.GetComponent<Health>().UpdateHealth(-attackDamage);
            if (collision.gameObject.GetComponent<Health>().health == 0f)
                Destroy(collision.gameObject);
            Destroy(gameObject);

        }
        if(collision.gameObject.CompareTag("Wall"))
            Destroy(gameObject);
    }
}
