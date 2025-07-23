using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform handPoint;
    public GameObject weaponInHand = null;
    private GameObject nearbyWeapon = null;

    private bool isWeapon;
    public bool isGun;
    public bool arm;
    private Animator playerAnimator;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") || other.CompareTag("Gun"))
        {
            nearbyWeapon = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Weapon") || other.CompareTag("Gun")) && other.gameObject == nearbyWeapon)
        {
            nearbyWeapon = null;
        }
    }

    public void TryPickup()
    {
        if (nearbyWeapon != null && weaponInHand == null)
        {
            PickupWeapon(nearbyWeapon);
        }
    }

    public void TryDrop()
    {
        if (weaponInHand != null)
        {
            DropWeapon();
        }
    }

    /*public void WeaponOrGun()
    {
        arm = isGun;
    }*/

    void PickupWeapon(GameObject weapon)
    {
        ResetAllAnimatorTriggers(playerAnimator);
        isWeapon = true;
        playerAnimator.SetBool("Weapon", isWeapon);
        Debug.Log("arma recogida");

        if (weapon.CompareTag("Gun"))
            isGun = true;
        if (weapon.CompareTag("Weapon"))
            isGun = false;

        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        Collider col = weapon.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        weapon.transform.SetParent(handPoint);
        weapon.transform.localPosition = Vector3.zero;
        //weapon.transform.localRotation = Quaternion.identity;

        weaponInHand = weapon;
        nearbyWeapon = null;

        //Debug.Log("¡Arma recogida!");
    }

    private void DropWeapon()
    {
        isWeapon = false;
        playerAnimator.SetBool("Weapon", isWeapon);
        ResetAllAnimatorTriggers(playerAnimator);
        weaponInHand.transform.SetParent(null);

        Rigidbody rb = weaponInHand.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        Collider col = weaponInHand.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        weaponInHand = null;

        //Debug.Log("¡Arma soltada!");
    }
    void ResetAllAnimatorTriggers(Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }
}
