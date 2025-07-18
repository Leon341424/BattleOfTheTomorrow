using System.Collections;
using UnityEngine;

public class Special : MonoBehaviour
{
    public GameObject SpecialPrefab;
    public float SpecialForce;

    private GameObject tmpSpecial;

    private bool specialActive;
    public Transform opponent;

    void Start()
    {

    }
    void Update()
    {
        if (specialActive)
        {
            StartCoroutine(DelaySpecial(0.75f));
            DisableSpecial();
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

    private void Fire()
    {
        GameObject tmpSpecial = Instantiate(SpecialPrefab, transform.position, Quaternion.identity);
        /*tmpSpecial.transform.right = transform.right;
        tmpSpecial.GetComponent<Rigidbody>().AddForce(transform.right * SpecialForce, ForceMode.Impulse);*/
        Vector3 direction = (opponent.position.x > transform.position.x) ? Vector3.right : Vector3.left;
        tmpSpecial.transform.right = direction;
        tmpSpecial.GetComponent<Rigidbody>().AddForce(direction * SpecialForce, ForceMode.Impulse);
    }

    private IEnumerator DelaySpecial(float time)
    {
        yield return new WaitForSeconds(time);
        Fire();
    }
}
