using UnityEngine;

public class SpecialEnemyHitbox : MonoBehaviour
{
    public float damage;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth targetHealth = other.GetComponent<PlayerHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamagePlayer(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
