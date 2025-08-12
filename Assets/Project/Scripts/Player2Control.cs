using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Player2Control : MonoBehaviour
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
    private SpecialEnemy specialScript;

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
    private Transform opponent;
    private bool isHitting;

    private KeyCode forwardKey;
    private KeyCode backKey;

    private bool isSpecial;

    private List<InputCommand> inputHistory = new List<InputCommand>();
    public float inputBufferTime = 0.6f;

    public Transform grabPoint;
    private bool isGrabbing;
    private GameObject grabbedOpponent;

    private bool isSuperSpecial;
    private EnemyHealth playerHealth;

    private Weapon arma;

    private GameObject nearbyWeapon = null;

    private bool isWeapon = false;

    private ShooterEnemy shooter;
    private HitboxEnemy hitbox;

    public bool isBlock { get; private set; }
    public bool isDown;
    private bool isPlayerOnLeft;
    private bool isForwardPressed;
    private bool isBackPressed;
    private bool isRunning;
    private bool isWalking;

    private InputSystem_ActionsGamepad control;

    [SerializeField] private UIManager pausa;

    private void Awake()
    {
        control = new InputSystem_ActionsGamepad();
    }

    private void OnEnable()
    {
        control.Player.Enable();
    }

    private void OnDisable()
    {
        control.Player.Disable();
    }

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        specialScript = GetComponentInChildren<SpecialEnemy>();
        playerHealth = GetComponent<EnemyHealth>();
        arma = GetComponent<Weapon>();
        shooter = GetComponentInChildren<ShooterEnemy>();
        hitbox = GetComponent<HitboxEnemy>();
        pausa = FindFirstObjectByType<UIManager>();

        Invoke("FindPlayer", 0.1f);
        /*GameObject obj = GameObject.FindWithTag("Enemy");
        if (obj == null) 
        GameObject obj = GameObject.FindWithTag("Player");
        opponent = obj.transform;*/

    }

    void FindPlayer()
    {
        GameObject obj = GameObject.FindWithTag("Player");
        opponent = obj.transform;
    }

    void Update()
    {
        movX = 0f;

        isPlayerOnLeft = transform.position.x < opponent.position.x;
        transform.rotation = isPlayerOnLeft ? Quaternion.Euler(0f, 90f, 0f) : Quaternion.Euler(0f, -90f, 0f);

        isForwardPressed = (isPlayerOnLeft && control.Player.Right.IsPressed()) ||
                                (!isPlayerOnLeft && control.Player.Left.IsPressed());

        isBackPressed = (isPlayerOnLeft && control.Player.Left.IsPressed()) ||
                            (!isPlayerOnLeft && control.Player.Right.IsPressed());

        isDown = control.Player.Down.IsPressed();
        playerAnimator.SetBool("down", isDown);

        isBlock = control.Player.Block.IsPressed();
        playerAnimator.SetBool("block", isBlock);

        isRunning = control.Player.Run.IsPressed();
        isWalking = isForwardPressed;

        if (!isForwardPressed && !isBackPressed)
        {
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("forward", false);
            speed = originalSpeed;
        }

        if (!isRunning && isForwardPressed && !isDown && !isBlock)
        {
            speed = originalSpeed;
            movX = isHitting ? 0f : (transform.position.x < opponent.position.x ? 1f : -1f);
            playerAnimator.SetBool("forward", isWalking);
        }

        if (isRunning && isForwardPressed && !isDown && !isBlock)
        {
            speed = originalSpeed * 2.5f;
            movX = isHitting ? 0f : (transform.position.x < opponent.position.x ? 1f : -1f);
            playerAnimator.SetBool("run", isRunning);
        }
        else
        {
            playerAnimator.SetBool("run", false);
        }

        playerAnimator.SetBool("back", isBackPressed);
        if (isBackPressed && !isDown && !isBlock)
        {
            movX = isHitting ? 0f : (transform.position.x < opponent.position.x ? -1f : 1f);
            Debug.Log(movX);
        }

        Vector3 movement = new Vector3(movX, 0f, 0f);
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, 0f);

        if (control.Player.Up.triggered && !isJumping && !isDown && !isBlock && !isSpecial)
        {
            isJumping = true;
            playerAnimator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (control.Player.Throw.triggered && !isGrabbing && !isJumping /*&& !isSpecial*/)
        {
            TryGrab();
        }

        //Aqui empieza el control del combate
        //Golpes en el suelo
        if (control.Player.LowPunch.triggered && !isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("LowPunch");
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        if (control.Player.LowKick.triggered && !isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("LowKick");
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        if (control.Player.HardPunch.triggered && !isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("HardPunch");
            hitbox.AddExtraDamage(7.5f);
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        if (control.Player.HardKick.triggered && !isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("HardKick");
            hitbox.AddExtraDamage(7.5f);
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        //Golpes en el aire
        if (control.Player.LowPunch.triggered && isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("JLP");
        }

        if (control.Player.LowKick.triggered && isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("JLK");
        }

        if (control.Player.HardPunch.triggered && isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("JHP");
        }

        if (control.Player.HardKick.triggered && isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("JHK");
        }

        //Golpes agachado
        if ((control.Player.LowPunch.triggered || control.Player.HardPunch.triggered) && isDown)
        {
            playerAnimator.SetTrigger("DownPunch");
            playerAnimator.ResetTrigger("LowPunch");
            playerAnimator.ResetTrigger("HardPunch");
        }

        if ((control.Player.LowKick.triggered || control.Player.HardKick.triggered) && isDown)
        {
            playerAnimator.SetTrigger("DownKick");
            playerAnimator.ResetTrigger("LowKick");
            playerAnimator.ResetTrigger("HardKick");
        }

        //Aqui empiezan los especiales
        if (control.Player.Down.triggered)
        {
            AddInput("Down");
            Debug.Log("Down");
        }

        if ((control.Player.Left.triggered && !isPlayerOnLeft) || (control.Player.Right.triggered && isPlayerOnLeft))
        {
            AddInput("Forward");
            Debug.Log("Forward");
        }

        if (control.Player.LowPunch.triggered)
        {
            AddInput("Attack");
            Debug.Log("Attack");
            CheckHadouken();
            isSpecial = false;
        }

        //superspecial

        if (control.Player.SuperSpecial.triggered && !isJumping && isSuperSpecial)
        {
            specialScript.EnableSuperSpecial();
            playerAnimator.SetTrigger("SuperSpecial");
            DisableSuper();
            playerHealth.ResetPower();
        }


        //Las armas

        if (control.Player.Pick.triggered)
        {
            if (arma.weaponInHand == null)
            {
                arma.TryPickup();
                isWeapon = true;
            }
            else
            {
                arma.TryDrop();
                isWeapon = false;
            }
        }

        if ((control.Player.LowPunch.triggered || control.Player.HardPunch.triggered
        || control.Player.LowKick.triggered || control.Player.HardKick.triggered) && isWeapon)
        {
            if (arma.isGun)
            {
                playerAnimator.SetTrigger("Shoot");
                shooter.EnableShoot();
            }
            if (!arma.isGun)
            {
                playerAnimator.SetTrigger("WeaponAttack");
                hitbox.AddExtraDamage(15f);
            }
        }

        //bool isWalkWeaponBack = Input.GetKey(backKey);
        playerAnimator.SetBool("backWeapon", isBackPressed && isWeapon);

        //bool isWalkWeaponForward = Input.GetKey(forwardKey);
        playerAnimator.SetBool("forwardWeapon", isForwardPressed && isWeapon);

        if (control.Player.Pause.triggered)
        {
            pausa.pause();
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

        isGrabbing = true;
        grabbedOpponent = opponent.gameObject;

        opponent.position = grabPoint.position;
        opponent.rotation = grabPoint.rotation;
        opponent.transform.SetParent(grabPoint);

        if (opponent.GetComponent<Rigidbody>())
            opponent.GetComponent<Rigidbody>().isKinematic = true;
        rb.isKinematic = true;

        var playerControl = opponent.GetComponent<Player1Control>();
        if (playerControl != null)
            playerControl.enabled = false;

        playerAnimator.SetTrigger("GrabSuccess");

        Animator enemyAnimator = opponent.GetComponentInChildren<Animator>();
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("grabbed");
            Debug.Log("Enemigo haciendo animacion");
        }
        
        PlayerHealth enemyHealth = grabbedOpponent.GetComponent<PlayerHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.EnableThrow();
            enemyHealth.TakeDamagePlayer(20);
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

            var playerControl = grabbedOpponent.GetComponent<Player1Control>();
            if (playerControl != null)
                playerControl.enabled = true;             
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

    public void EnableSuper()
    {
        isSuperSpecial = true;
    }

    public void DisableSuper()
    {
        isSuperSpecial = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            nearbyWeapon = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon") && other.gameObject == nearbyWeapon)
        {
            nearbyWeapon = null;
        }
    }

}
