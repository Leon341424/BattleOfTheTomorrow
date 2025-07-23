using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage;
    private float currentDamage;
    private bool canDamage = false;
    private HashSet<GameObject> alreadyHit = new HashSet<GameObject>();
    private PlayerHealth playerPower;

    void Start()
    {
        playerPower = GetComponent<PlayerHealth>();
        currentDamage = damage;
    }
    void OnTriggerEnter(Collider other)
    {
        if (canDamage && other.CompareTag("Enemy") && !alreadyHit.Contains(other.gameObject))
        {
            EnemyHealth targetHealth = other.GetComponent<EnemyHealth>();
            targetHealth.TakeDamageEnemy(currentDamage);
            alreadyHit.Add(other.gameObject);
            playerPower.GainPower(50f);
            DisableDamage();
            currentDamage = damage;
        }
    }

    public void EnableDamage()
    {
        canDamage = true;
        alreadyHit.Clear();
        //Debug.Log("daño activado");
    }

    public void DisableDamage()
    {
        canDamage = false;
        //Debug.Log("daño desactivado");
    }

    /*private IEnumerator DamageWindow(float time)
    {
        yield return new WaitForSeconds(time);
        canDamage = false;
    }*/
    public void AddExtraDamage(float extra)
    {
        currentDamage = damage + extra;
    }
    
}
