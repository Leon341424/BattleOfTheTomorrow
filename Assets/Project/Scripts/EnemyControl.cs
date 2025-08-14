using System.Collections;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private Transform opponent;
    public float speed;
    public float attackRange;
    public float jumpForce;

    private Animator animator;
    private Rigidbody rb;

    private bool isJumping = false;
    private bool isAttacking = false;
    private float decisionTimer;
    private float decisionInterval = 1f;

    private SpecialEnemy specialScript;
    private bool isSpecial = false;
    public bool isBlock { get; private set; }

    private EnemyHealth enemyHealth;
    private HitboxEnemy enemyHitbox;
    private ShooterEnemy shooter;

    private bool isSuperSpecial = false;

    private Weapon arma;
    private bool isWeapon;
    private bool isGrabbing = false;
    private GameObject grabbedOpponent;
    public Transform grabPoint;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        /*GameObject obj = GameObject.FindWithTag("Player");
        opponent = obj.transform;*/
        Invoke("FindPlayer", 0.1f);
        specialScript = GetComponentInChildren<SpecialEnemy>();
        arma = GetComponent<Weapon>();
        enemyHitbox = GetComponent<HitboxEnemy>();
        shooter = GetComponentInChildren<ShooterEnemy>();
    }

    void FindPlayer()
    {
        GameObject obj = GameObject.FindWithTag("Player");
        opponent = obj.transform;
    }

    void Update()
    {
        if (opponent == null) return;

        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0f)
        {
            decisionTimer = decisionInterval;
            MakeDecision();
        }

        if (transform.position.x < opponent.position.x)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    void LateUpdate()
    {
        Vector3 fixedPos = transform.position;
        fixedPos.z = 0f;
        transform.position = fixedPos;
    }

    void MakeDecision()
    {
        float distance = Vector3.Distance(transform.position, opponent.position);

        if (distance > attackRange)
        {
            if (Random.value > 0.7 && !isWeapon)
            {
                StartCoroutine(SpecialRoutine());
            }

            else if (Random.value > 0.3 && isSuperSpecial && !isWeapon)
            {
                StartCoroutine(SuperSpecialRoutine());
                enemyHealth.ResetPower();
                DisableSuper();
            }

            else if (Random.value > 0.1 && !isBlock && !isSpecial & !isSuperSpecial)
            {
                float direction = transform.position.x < opponent.position.x ? 1f : -1f;
                rb.linearVelocity = new Vector3(direction * speed, rb.linearVelocity.y, 0f);
                animator.SetBool("forward", true);
            }

            isBlock = Random.value < 0.15f;
            animator.SetBool("block", isBlock);

        }
        else
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            animator.SetBool("forward", false);

            if (!isAttacking && Random.value < 0.7f)
            {
                StartCoroutine(AttackRoutine());
            }

            isBlock = Random.value < 0.15f;
            animator.SetBool("block", isBlock);
        }

        if (!isJumping && Random.value < 0.1f)
        {
            isJumping = true;
            animator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (arma.weaponInHand == null)
        {
            arma.TryPickup();
            isWeapon = true;
            enemyHitbox.weaponActive();
            return;
        }

        if (arma.weaponInHand != null && Random.value < 0.1f)
        {
            arma.TryDrop();
            isWeapon = false;
            enemyHitbox.weaponDesactive();
            return;
        }
    }

    IEnumerator AttackRoutine()
    {
        float distanceToOpponent = Vector3.Distance(transform.position, opponent.position);
        if (arma != null && arma.weaponInHand != null && isWeapon)
        {
            if (arma.isGun)
            {
                animator.SetTrigger("Shoot");
                shooter.EnableShoot();
            }
            else
            {
                animator.SetTrigger("WeaponAttack");
                enemyHitbox.AddExtraDamage(15f);
            }
        }
        else if (Random.value < 0.2f && !isJumping && !isWeapon)
        {
            isJumping = true;
            animator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            yield return new WaitForSeconds(0.2f);

            string[] airAttacks = new string[] { "JLP", "JLK", "JHP", "JHK" };
            string selectedAirAttack = airAttacks[Random.Range(0, airAttacks.Length)];
            animator.SetTrigger(selectedAirAttack);
            yield return new WaitForSeconds(0.6f);
        }
        if (!isGrabbing && distanceToOpponent < 2.5f && Random.value < 0.3f)
        {
            TryGrab();
            yield return new WaitForSeconds(1.0f);
            isAttacking = false;
            yield break;
        }
        else
        {
            string[] attackTriggers = new string[] { "LowPunch", "LowKick", "HardPunch", "HardKick" };

            string selectedAttack = attackTriggers[Random.Range(0, attackTriggers.Length)];
            animator.SetTrigger(selectedAttack);
            yield return new WaitForSeconds(0.5f);
            isAttacking = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
    }

    IEnumerator SpecialRoutine()
    {
        isSpecial = true;
        animator.SetTrigger("special");
        specialScript.EnableSpecial();
        yield return new WaitForSeconds(1.2f);
        isSpecial = false;
    }

    IEnumerator SuperSpecialRoutine()
    {
        animator.SetTrigger("SuperSpecial");
        specialScript.EnableSuperSpecial();
        enemyHealth.ResetPower();
        yield return new WaitForSeconds(1.5f);
        DisableSuper();
    }

    public void EnableSuper()
    {
        isSuperSpecial = true;
    }

    public void DisableSuper()
    {
        isSuperSpecial = false;
    }

    void TryGrab()
    {
        animator.SetTrigger("grab");
        float distanceToOpponent = Vector3.Distance(transform.position, opponent.position);

        if (distanceToOpponent < 2.5f)
        {
            StartCoroutine(ExecuteSuccessfulGrab());
            enemyHealth.GainPower(10f);
            Debug.Log("IA: agarre exitoso");
        }
        else
        {
            Debug.Log("IA: agarre fallido");
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
        opponent.SetParent(grabPoint);

        if (opponent.GetComponent<Rigidbody>())
            opponent.GetComponent<Rigidbody>().isKinematic = true;
        rb.isKinematic = true;

        var playerControl = opponent.GetComponent<Player1Control>();
        if (playerControl != null)
            playerControl.enabled = false;

        animator.SetTrigger("GrabSuccess");
        Debug.Log("Opponent: " + opponent.name);
        Animator enemyAnimator = opponent.GetComponent<Animator>();
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("grabbed");
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
}
