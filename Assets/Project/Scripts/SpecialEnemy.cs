using System.Collections;
using UnityEngine;

public class SpecialEnemy : MonoBehaviour
{
    public GameObject SpecialPrefab;
    public GameObject SuperSpecialPrefab;
    public float SpecialForce;

    private GameObject tmpSpecial;

    private GameObject tmpSuperSpecial;

    private bool specialActive;

    private bool superSpecialActive;
    private Transform opponent;

    void Start()
    {
        GameObject obj = GameObject.FindWithTag("Enemy");
        
        if (obj == null)
        {
            obj = GameObject.FindWithTag("Player");
        }
        opponent = obj.transform;
    }
    void Update()
    {
        if (specialActive)
        {
            StartCoroutine(DelaySpecial(0.75f));
            AudioManager.Instance.PlayOneShot("special");
            DisableSpecial();
        }

        if (superSpecialActive)
        {
            StartCoroutine(DelaySuperSpecial(1.25f));
            AudioManager.Instance.PlayOneShot("SuperSpecial");
            DisableSuperSpecial();
        }
    }

    public void EnableSpecial()
    {
        specialActive = true;
    }

    public void DisableSpecial()
    {
        specialActive = false;
    }

    public void EnableSuperSpecial()
    {
        superSpecialActive = true;
    }

    public void DisableSuperSpecial()
    {
        superSpecialActive = false;
    }

    private void Fire()
    {
        GameObject tmpSpecial = Instantiate(SpecialPrefab, transform.position, Quaternion.identity);
        Vector3 direction = (opponent.position.x < transform.position.x) ? Vector3.right : Vector3.left;
        tmpSpecial.transform.right = direction;
        tmpSpecial.GetComponent<Rigidbody>().AddForce(direction * SpecialForce, ForceMode.Impulse);
        Destroy(tmpSpecial, 4f);
    }

    private void FireSuper()
    {
        GameObject tmpSuperSpecial = Instantiate(SuperSpecialPrefab, transform.position + new Vector3(33.5f, 2f, 0f),
        Quaternion.identity);
        Vector3 direction = (opponent.position.x < transform.position.x) ? Vector3.right : Vector3.left;
        tmpSuperSpecial.transform.right = direction;
        Destroy(tmpSuperSpecial, 1.75f);
    }

    private IEnumerator DelaySpecial(float time)
    {
        yield return new WaitForSeconds(time);
        Fire();
    }

    private IEnumerator DelaySuperSpecial(float time)
    {
        yield return new WaitForSeconds(time);
        FireSuper();
    }
}
