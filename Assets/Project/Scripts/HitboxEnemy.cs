using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxEnemy : MonoBehaviour
{
    public float damage;
    private float currentDamage;
    private bool canDamage = false;
    private HashSet<GameObject> alreadyHit = new HashSet<GameObject>();
    private EnemyHealth enemyPower;

    void Start()
    {
        enemyPower = GetComponent<EnemyHealth>();
        currentDamage = damage;
    }
    void OnTriggerEnter(Collider other)
    {
        if (canDamage && other.CompareTag("Player") && !alreadyHit.Contains(other.gameObject))
        {
            PlayerHealth targetHealth = other.GetComponent<PlayerHealth>();
            targetHealth.TakeDamagePlayer(currentDamage);
            alreadyHit.Add(other.gameObject);
            enemyPower.GainPower(10f);
            DisableDamage();
            currentDamage = damage;
        }
    }

    public void EnableDamage()
    {
        canDamage = true;
        alreadyHit.Clear();
    }

    public void DisableDamage()
    {
        canDamage = false;
    }
    public void AddExtraDamage(float extra)
    {
        currentDamage = damage + extra;
    }
    
}
