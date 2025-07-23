using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private bool ShootActive;
    public GameObject BulletPrefab;
    public float BulletForce;
    private GameObject tmpShoot;
    public Transform opponent;
    void Update()
    {
        if (ShootActive)
        {
            StartCoroutine(DelayShoot(0.4f));
            DisableShoot();
        }
    }

    public void EnableShoot()
    {
        ShootActive = true;
    }

    public void DisableShoot()
    {
        ShootActive = false;
    }

    private void FireShoot()
    {
        tmpShoot = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        Vector3 direction = (opponent.position.x > transform.position.x) ? Vector3.right : Vector3.left;
        tmpShoot.transform.right = direction;
        tmpShoot.GetComponent<Rigidbody>().AddForce(direction * BulletForce, ForceMode.Impulse);
    }

    private IEnumerator DelayShoot(float time)
    {
        yield return new WaitForSeconds(time);
        FireShoot();
    }
}
