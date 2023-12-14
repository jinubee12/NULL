using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float health = 0f;
    [SerializeField] private float maxHealth = 100f;
    public HealthBar healthBar;

    private void Start()
    {
        health = maxHealth;
    }

    public void UpdateHealth(float mod)
    {
        health += mod;

        if(health <= 0f)
        {
            health = 0f;

        }
        healthBar.SetHealth(health);
    }
}
