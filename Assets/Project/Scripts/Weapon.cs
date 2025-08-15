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

        //Quaternion originalRotation = weapon.transform.rotation;
        weapon.transform.SetParent(handPoint);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        weaponInHand = weapon;
        nearbyWeapon = null;

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
            Vector3 throwDirection = Vector3.zero;
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                throwDirection = new Vector3(horizontal, 0.3f, vertical).normalized;
                float throwForce = 8f;
                rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);
            }
            
        }

        Collider col = weaponInHand.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        weaponInHand = null;

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
