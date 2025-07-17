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
    private bool isHitting;

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
        //Aqui empieza el control del movimiento
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
            //movX = transform.position.x < opponent.position.x ? 1f : -1f;
            movX = isHitting ? 0f : (transform.position.x < opponent.position.x ? 1f : -1f);
            playerAnimator.SetBool("forward", isWalking);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(forwardKey) && !isDown && !isBlock)
        {
            speed = originalSpeed * 2.5f;
            //movX = transform.position.x < opponent.position.x ? 1f : -1f;
            movX = isHitting ? 0f : (transform.position.x < opponent.position.x ? 1f : -1f);
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
            //smovX = transform.position.x < opponent.position.x ? -1f : 1f;
            movX = isHitting ? 0f : (transform.position.x < opponent.position.x ? -1f : 1f);
        }

        Vector3 movement = new Vector3(movX, 0f, 0f);
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, 0f);

        if (Input.GetKeyDown(KeyCode.W) && !isJumping && !isDown && !isBlock)
        {
            isJumping = true;
            playerAnimator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }


        //Aqui empieza el control del combate
        //Golpes en el suelo

        if (Input.GetKeyDown(KeyCode.J) && !isJumping)
        {
            playerAnimator.SetTrigger("LowPunch"); 
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        if (Input.GetKeyDown(KeyCode.L) && !isJumping)
        {
            playerAnimator.SetTrigger("LowKick");
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        if (Input.GetKeyDown(KeyCode.I) && !isJumping)
        {
            playerAnimator.SetTrigger("HardPunch");
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        if (Input.GetKeyDown(KeyCode.K) && !isJumping)
        {
            playerAnimator.SetTrigger("HardKick");
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        //Golpes en el aire
        if (Input.GetKeyDown(KeyCode.J) && isJumping)
        {
            playerAnimator.SetTrigger("JLP");
        }

        if (Input.GetKeyDown(KeyCode.L) && isJumping)
        {
            playerAnimator.SetTrigger("JLK");
        }

        if (Input.GetKeyDown(KeyCode.I) && isJumping)
        {
            playerAnimator.SetTrigger("JHP");
        }

        if (Input.GetKeyDown(KeyCode.K) && isJumping)
        {
            playerAnimator.SetTrigger("JHK");
        }

        //Golpes agachado
        if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.I)) && isDown)
        {
            playerAnimator.SetTrigger("DownPunch");
            playerAnimator.ResetTrigger("LowPunch");
            playerAnimator.ResetTrigger("HardPunch");
        }

        if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.K)) && isDown)
        {
            playerAnimator.SetTrigger("DownKick");
            playerAnimator.ResetTrigger("LowKick");
            playerAnimator.ResetTrigger("HardKick");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
        playerAnimator.ResetTrigger("JLP");
        playerAnimator.ResetTrigger("JLK");
        playerAnimator.ResetTrigger("JHP");
        playerAnimator.ResetTrigger("JHK");
    }

    private IEnumerator delayedHit()
    {
        yield return new WaitForSeconds(0.5f);
        isHitting = false;
    }

}
