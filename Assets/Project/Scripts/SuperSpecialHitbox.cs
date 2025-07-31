using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SuperSpecialHitbox : MonoBehaviour
{
    public float damage;
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            EnemyHealth targetHealth = other.GetComponent<EnemyHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamageEnemy(damage);
            }
        }
    }
}
