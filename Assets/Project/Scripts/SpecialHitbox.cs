using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpecialHitbox : MonoBehaviour
{
    public float damage;
    private PlayerHealth playerPower;
    void Start()
    {
        playerPower = GetComponent<PlayerHealth>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth targetHealth = other.GetComponent<EnemyHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamageEnemy(damage);
                playerPower.GainPower(8f);
                Destroy(this.gameObject);
            }
        }
    }
}
