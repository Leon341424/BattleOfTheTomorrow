using System.Collections;
using UnityEngine;

public class Player1Control : MonoBehaviour
{
    private Animator playerAnimator;
    private AnimatorStateInfo stateInfo;
    public float speed;
    private float originalSpeed;

    private float movX;
    private Vector3 originalPosition;
    private Vector3 currentPosition;
    private bool isJumping = false;
    public float jumpForce;
    private Rigidbody rb;
    public Transform opponent;

    private KeyCode forwardKey;
    private KeyCode backKey;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    void Update()
    {
        stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        movX = 0f;

        if (transform.position.x < opponent.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            forwardKey = KeyCode.D;
            backKey = KeyCode.A;
        }
        if (transform.position.x > opponent.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            forwardKey = KeyCode.A;
            backKey = KeyCode.D;
        }

        bool isDown = Input.GetKey(KeyCode.S);
        playerAnimator.SetBool("down", isDown);

        bool isBlock = Input.GetKey(KeyCode.O);
        playerAnimator.SetBool("block", isBlock);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isWalking = Input.GetKey(forwardKey);

        if (!Input.GetKey(forwardKey) && !Input.GetKey(backKey))
        {
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("forward", false);
            speed = originalSpeed;
        }

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(forwardKey) && !isDown && !isBlock)
        {
            speed = originalSpeed;
            //movX = 1;
            movX = transform.position.x < opponent.position.x ? 1f : -1f;
            playerAnimator.SetBool("forward", isWalking);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(forwardKey) && !isDown && !isBlock)
        {
            speed = originalSpeed * 2.5f;
            movX = transform.position.x < opponent.position.x ? 1f : -1f;
            playerAnimator.SetBool("run", isRunning);
        }
        else
        {
            playerAnimator.SetBool("run", false);
        }

        bool isBack = Input.GetKey(backKey);
        playerAnimator.SetBool("back", isBack);
        if (Input.GetKey(backKey) && !isDown && !isBlock)
        {
            movX = transform.position.x < opponent.position.x ? -1f : 1f;
        }

        Vector3 movement = new Vector3(movX, 0f, 0f);
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, 0f);

        if (Input.GetKeyDown(KeyCode.W) && !isJumping && !isDown && !isBlock)
        {
            isJumping = true;
            playerAnimator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
    }

}
