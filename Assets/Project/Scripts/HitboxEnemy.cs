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
    private bool weapon;

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
            if (weapon)
            {
                enemyPower.GainPower(1f);
            }
            else
            {
                enemyPower.GainPower(10f);
            }
            DisableDamage();
            currentDamage = damage;
        }
    }
    public void weaponActive()
    {
        weapon = true;
    }

    public void weaponDesactive()
    {
        weapon = false;
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
