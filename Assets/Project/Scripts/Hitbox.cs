using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage;
    private bool canDamage = false;
    private HashSet<GameObject> alreadyHit = new HashSet<GameObject>();
    private Coroutine damageCoroutine;

    void OnTriggerEnter(Collider other)
    {
        if (canDamage && other.CompareTag("Enemy") && !alreadyHit.Contains(other.gameObject))
        {
            EnemyHealth targetHealth = other.GetComponent<EnemyHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamageEnemy(damage);
                alreadyHit.Add(other.gameObject);
                StartCoroutine(DamageWindow(0.035f));
            }
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
    
    private IEnumerator DamageWindow(float time)
    {
        yield return new WaitForSeconds(time);
        DisableDamage();
    }
}
