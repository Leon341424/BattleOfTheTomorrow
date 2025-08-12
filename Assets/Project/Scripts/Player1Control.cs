using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

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
    private PlayerHealth playerHealth;

    private Weapon arma;

    private GameObject nearbyWeapon = null;

    private bool isWeapon = false;

    private Shooter shooter;
    private Hitbox hitbox;

    public bool isBlock { get; private set; }
    private bool isDown;
    private bool isPlayerOnLeft;
    private bool isForwardPressed;
    private bool isBackPressed;
    private bool isRunning;
    private bool isWalking;

    private InputSystem_Actions control;

    [SerializeField] private UIManager pausa;

    public PlayerControlMapping controlMapping;

    private InputAction lowPunchAction;
    private InputAction lowKickAction;
    private InputAction hardPunchAction;
    private InputAction hardKickAction;
    private InputAction blockAction;
    private InputAction throwAction;

    private void Awake()
    {
        control = new InputSystem_Actions();
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
        specialScript = GetComponentInChildren<Special>();
        playerHealth = GetComponent<PlayerHealth>();
        arma = GetComponent<Weapon>();
        shooter = GetComponentInChildren<Shooter>();
        hitbox = GetComponent<Hitbox>();
        pausa = FindFirstObjectByType<UIManager>();

        GameObject obj = GameObject.FindWithTag("Enemy");
        if (obj == null) obj = GameObject.FindWithTag("Player2");
        opponent = obj.transform;

        ReloadMapping();
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

        isBlock = blockAction.IsPressed();
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
        }

        Vector3 movement = new Vector3(movX, 0f, 0f);
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, 0f);

        if (control.Player.Up.triggered && !isJumping && !isDown && !isBlock && !isSpecial)
        {
            isJumping = true;
            playerAnimator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (throwAction.triggered && !isGrabbing && !isJumping /*&& !isSpecial*/)
        {
            TryGrab();
        }

        //Aqui empieza el control del combate
        //Golpes en el suelo
        if (lowPunchAction.triggered && !isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("LowPunch");
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        if (lowKickAction.triggered && !isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("LowKick");
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        if (hardPunchAction.triggered && !isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("HardPunch");
            hitbox.AddExtraDamage(7.5f);
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        if (hardKickAction.triggered && !isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("HardKick");
            hitbox.AddExtraDamage(7.5f);
            isHitting = true;
            StartCoroutine(delayedHit());
        }

        //Golpes en el aire
        if (lowPunchAction.triggered && isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("JLP");
        }

        if (lowKickAction.triggered && isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("JLK");
        }

        if (hardPunchAction.triggered && isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("JHP");
        }

        if (hardKickAction.triggered && isJumping && !isSpecial)
        {
            playerAnimator.SetTrigger("JHK");
        }

        //Golpes agachado
        if ((lowPunchAction.triggered || hardPunchAction.triggered) && isDown)
        {
            playerAnimator.SetTrigger("DownPunch");
            playerAnimator.ResetTrigger("LowPunch");
            playerAnimator.ResetTrigger("HardPunch");
        }

        if ((lowKickAction.triggered|| hardKickAction.triggered) && isDown)
        {
            playerAnimator.SetTrigger("DownKick");
            playerAnimator.ResetTrigger("LowKick");
            playerAnimator.ResetTrigger("HardKick");
        }

        //Aqui empiezan los especiales
        if (control.Player.Down.triggered)
        {
            AddInput("Down");
        }

        if ((control.Player.Left.triggered && !isPlayerOnLeft) || (control.Player.Right.triggered && isPlayerOnLeft))
        {
            AddInput("Forward");
        }

        if (lowPunchAction.triggered)
        {
            AddInput("Attack");
            CheckHadouken();
            isSpecial = false;
        }

        if ((control.Player.Right.triggered && !isPlayerOnLeft) || (control.Player.Left.triggered && isPlayerOnLeft))
        {
            AddInput("Back");
        }

        if (lowKickAction.triggered)
        {
            AddInput("Attack1");
            CheckHadouken();
            isSpecial = false;
        }

        if (hardPunchAction.triggered)
        {
            AddInput("Attack2");
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

        if ((lowPunchAction.triggered || hardPunchAction.triggered || lowKickAction.triggered
            || hardKickAction.triggered) && isWeapon)
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

        var playerControl = opponent.GetComponent<EnemyControl>();
        if (playerControl != null)
            playerControl.enabled = false;

        playerAnimator.SetTrigger("GrabSuccess");

        Animator enemyAnimator = opponent.GetComponentInChildren<Animator>();
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("grabbed");
            Debug.Log("Enemigo haciendo animacion");
        }

        EnemyHealth enemyHealth = grabbedOpponent.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.EnableThrow();
            enemyHealth.TakeDamageEnemy(20);
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

            var playerControl = grabbedOpponent.GetComponent<EnemyControl>();
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
            //Debug.Log("Especial ejecutado!");
            isSpecial = true;
            playerAnimator.SetTrigger("special");
            specialScript.EnableSpecial();
            inputHistory.Clear();
            return;
        }
        else if (inputHistory[count - 3].input == "Down" &&
            inputHistory[count - 2].input == "Back" &&
            inputHistory[count - 1].input == "Attack1")
        {
            isSpecial = true;
            Debug.Log("Especial 2 ejecutado!");
            playerAnimator.SetTrigger("special2");
            specialScript.EnableSpecial2();
            inputHistory.Clear();
            return;
        }
        else if (inputHistory[count - 3].input == "Forward" &&
            inputHistory[count - 2].input == "Down" &&
            inputHistory[count - 1].input == "Attack2")
        {
            isSpecial = true;
            playerAnimator.SetTrigger("special3");
            //specialScript.EnableSpecial();
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

    public void ReloadMapping()
    {
        lowPunchAction = control.Player.GetType().GetProperty(controlMapping.lowPunch).GetValue(control.Player) as InputAction;
        lowKickAction = control.Player.GetType().GetProperty(controlMapping.lowKick).GetValue(control.Player) as InputAction;
        hardPunchAction = control.Player.GetType().GetProperty(controlMapping.hardPunch).GetValue(control.Player) as InputAction;
        hardKickAction = control.Player.GetType().GetProperty(controlMapping.hardKick).GetValue(control.Player) as InputAction;
        blockAction = control.Player.GetType().GetProperty(controlMapping.Block).GetValue(control.Player) as InputAction;
        throwAction = control.Player.GetType().GetProperty(controlMapping.Throw).GetValue(control.Player) as InputAction;

        lowPunchAction?.Enable();
        lowKickAction?.Enable();
        hardPunchAction?.Enable();
        hardKickAction?.Enable();
        blockAction?.Enable();
        throwAction?.Enable();

        control.Player.Enable();
    }

}
