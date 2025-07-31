using System.Collections;
using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    private bool ShootActive;
    public GameObject BulletPrefab;
    public float BulletForce;
    private GameObject tmpShoot;
    private Transform opponent;
    void Start()
    {
        GameObject obj = GameObject.FindWithTag("Player");
        opponent = obj.transform;
    }
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
