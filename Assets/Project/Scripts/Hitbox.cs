using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage;
    private bool canDamage = false;
    private bool alreadyDamagedInThisWindow = false;
    private HashSet<GameObject> alreadyHit = new HashSet<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (canDamage && !alreadyDamagedInThisWindow && other.CompareTag("Enemy") && !alreadyHit.Contains(other.gameObject))
        {
            EnemyHealth targetHealth = other.GetComponent<EnemyHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamageEnemy(damage);
                alreadyHit.Add(other.gameObject);
                //StartCoroutine(DamageWindow(0.035f));
                //DisableDamage();
            }
        }
    }

    public void EnableDamage()
    {
        //canDamage = true; 
        //alreadyHit.Clear();
        StartCoroutine(DamageWindow(0.035f));
    }

    public void DisableDamage()
    {
        canDamage = false;
    }
    
    private IEnumerator DamageWindow(float time)
    {
        canDamage = true; 
        alreadyHit.Clear();
        yield return new WaitForSeconds(time);
        canDamage = false;
    }
}
