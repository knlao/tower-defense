using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    /*
     * Enemy Script
     */

    [Header("Enemy Behaviour")]
    public float startSpeed = 10f;
    public float health = 100;
    public int worth = 50;
    public GameObject deathEffect;
    public LayerMask unwalkable;

    [Header("UI")]
    public Image healthBar;
    public Gradient healthBarGradient;

    [HideInInspector]
    public float speed;

    private float startHealth;
    
    private void Start()
    {
        speed = startSpeed;
        startHealth = health;
        healthBar.fillAmount = 1;
        healthBar.color = healthBarGradient.Evaluate(1);
    }
    
    public void TakeDamage(float amount)
    {
        health -= amount;

        float healthPct = health / startHealth;

        healthBar.fillAmount = healthPct;
        healthBar.color = healthBarGradient.Evaluate(healthPct);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1 - pct);
    }

    private void Die()
    {
        PlayerStats.Money += worth;
        //WaveSpawner.EnemiesAlive--;

        var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        
        Destroy(gameObject);
    }
}
