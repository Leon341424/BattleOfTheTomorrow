using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpecialHitbox : MonoBehaviour
{
    public int damage;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth targetHealth = other.GetComponent<EnemyHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamageEnemy(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
