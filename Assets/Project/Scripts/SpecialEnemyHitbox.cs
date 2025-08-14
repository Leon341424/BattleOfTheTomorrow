using UnityEngine;

public class SpecialEnemyHitbox : MonoBehaviour
{
    public float damage;
    private EnemyHealth enemyPower;
    void Start()
    {
        enemyPower = GetComponent<EnemyHealth>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth targetHealth = other.GetComponent<PlayerHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamagePlayer(damage);
                enemyPower.GainPower(8f);
                Destroy(this.gameObject);
            }
        }
    }
}
