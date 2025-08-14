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

    private bool weapon;

    void Start()
    {
        playerPower = GetComponent<PlayerHealth>();
        currentDamage = damage;
    }
    void OnTriggerEnter(Collider other)
    {
        if (canDamage && other.CompareTag("Enemy")
            && !alreadyHit.Contains(other.gameObject))
        {
            EnemyHealth targetHealth = other.GetComponent<EnemyHealth>();
            targetHealth.TakeDamageEnemy(currentDamage);
            alreadyHit.Add(other.gameObject);
            if (weapon)
            {
                playerPower.GainPower(1f);
            }
            else
            {
                playerPower.GainPower(10f);
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
        Debug.Log("daño activado");
    }

    public void DisableDamage()
    {
        canDamage = false;
        Debug.Log("daño desactivado");
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
