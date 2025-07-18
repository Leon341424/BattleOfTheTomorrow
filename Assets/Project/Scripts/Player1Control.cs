using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Player1Control : MonoBehaviour
{
    public class InputCommand
    {
        public string input;
        public float time;

        public InputCommand(string input, float time)
        {
            this.input = input;
            this.time = time;
        }
    }
    private Special specialScript;

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

    private bool isSpecial;

    private List<InputCommand> inputHistory = new List<InputCommand>();
    public float inputBufferTime = 0.6f;

    public Transform grabPoint;
    private bool isGrabbing;
    private GameObject grabbedOpponent;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        specialScript = GetComponentInChildren<Special>();
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

        if (Input.GetKeyDown(KeyCode.U) && !isGrabbing && !isJumping /*&& !isSpecial*/)
        {
            TryGrab();
        }

        //Aqui empieza el control del combate
        //Golpes en el suelo

        if (Input.GetKeyDown(KeyCode.J) && !isJumping && !isSpecial)
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

        //Aqui empiezan los especiales
        if (Input.GetKeyDown(KeyCode.S))
            AddInput("Down");

        if (Input.GetKeyDown(forwardKey))
            AddInput("Forward");

        if (Input.GetKeyDown(KeyCode.J))
        {
            AddInput("Attack");
            CheckHadouken();
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

    void TryGrab()
    {
        playerAnimator.SetTrigger("grab");
        float distanceToOpponent = Vector3.Distance(transform.position, opponent.position);

        if (distanceToOpponent < 2.5f)
        {
            StartCoroutine(ExecuteSuccessfulGrab());
            Debug.Log("agarre exitoso");
            //playerAnimator.SetTrigger("GrabSuccess");
        }
        else
        {
            Debug.Log("agarre fallido");
        }
    }

    IEnumerator ExecuteSuccessfulGrab()
    {
        yield return new WaitForSeconds(0.3f); 

        float distanceToOpponent = Vector3.Distance(transform.position, opponent.position);
        if (distanceToOpponent > 2.0f) yield break;

        isGrabbing = true;
        grabbedOpponent = opponent.gameObject;

        opponent.position = grabPoint.position;
        opponent.rotation = grabPoint.rotation;
        opponent.SetParent(grabPoint);

        if (opponent.GetComponent<Rigidbody>())
            opponent.GetComponent<Rigidbody>().isKinematic = true;
            rb.isKinematic = true;

        var enemyControl = opponent.GetComponent<Player1Control>();
        if (enemyControl != null)
            enemyControl.enabled = false;

        playerAnimator.SetTrigger("GrabSuccess");

        Animator enemyAnimator = opponent.GetComponent<Animator>();
        if (enemyAnimator != null)
            enemyAnimator.SetTrigger("grabbed");

        EnemyHealth enemyHealth = grabbedOpponent.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamageEnemy(20);
            Debug.Log("¡Agarre causó daño!");
        }

        StartCoroutine(ReleaseGrab(1.2f));
    }

    IEnumerator ReleaseGrab(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (grabbedOpponent != null)
        {
            grabbedOpponent.transform.SetParent(null);

            if (grabbedOpponent.GetComponent<Rigidbody>())
                grabbedOpponent.GetComponent<Rigidbody>().isKinematic = false;

            var enemyControl = grabbedOpponent.GetComponent<Player1Control>();
            if (enemyControl != null)
                enemyControl.enabled = true;
        }
        rb.isKinematic = false;
        isGrabbing = false;
    }

    void AddInput(string input)
    {
        inputHistory.Add(new InputCommand(input, Time.time));
        inputHistory.RemoveAll(i => Time.time - i.time > inputBufferTime);
    }

    void CheckHadouken()
    {
        if (inputHistory.Count < 3) return;

        int count = inputHistory.Count;
        if (inputHistory[count - 3].input == "Down" &&
            inputHistory[count - 2].input == "Forward" &&
            inputHistory[count - 1].input == "Attack")
        {
            Debug.Log("Especial ejecutado!");
            isSpecial = true;
            playerAnimator.SetTrigger("special");
            specialScript.EnableSpecial();
            inputHistory.Clear();
            return;
        }
        else
        {
            isSpecial = false;
        }
    }

}
