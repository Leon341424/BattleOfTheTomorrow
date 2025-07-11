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

    private float lastPressTime = -1f;
    private float doubleTapTime = 0.3f;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
    }

    void Update()
    {
        stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        movX = 0f;

        bool isDown = Input.GetKey(KeyCode.S);
        playerAnimator.SetBool("down", isDown);

        bool isBlock = Input.GetKey(KeyCode.O);
        playerAnimator.SetBool("block", isBlock);
   
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isWalking = Input.GetKey(KeyCode.D);

        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("forward", false);
            speed = originalSpeed;
        }

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) && !isDown && !isBlock)
        {
            speed = originalSpeed;
            movX = 1;
            playerAnimator.SetBool("forward", isWalking);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) && !isDown && !isBlock)
        {
            speed = originalSpeed * 2;
            movX = 1;
            playerAnimator.SetBool("run", isRunning);
        }
        else
        {
            playerAnimator.SetBool("run", false);
        }

        bool isBack = Input.GetKey(KeyCode.A);
        playerAnimator.SetBool("back", isBack);
        if (Input.GetKey(KeyCode.A) && !isDown && !isBlock)
        {
            movX = -1f;
        }

        Vector3 movement = new Vector3(movX, 0f, 0f);
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, rb.linearVelocity.z);

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
