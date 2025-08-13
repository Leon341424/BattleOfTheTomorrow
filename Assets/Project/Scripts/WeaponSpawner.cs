using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject[] weapons;
    public float spawnDelay;
    public float weaponLifetime; 

    void Start()
    {
        StartCoroutine(newWeapon());
    }

    private IEnumerator newWeapon()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            int randomIndex = Random.Range(0, weapons.Length);
            GameObject weapon = Instantiate(weapons[randomIndex], transform.position, Quaternion.identity);

            Destroy(weapon, weaponLifetime);
        }
    }  
}
