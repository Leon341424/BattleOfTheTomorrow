using System.Collections;
using UnityEngine;

public class BossEnemyControl : MonoBehaviour
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
        enemyHealth = GetComponent<EnemyHealth>();
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
            decisionTimer = decisionInterval / 1.25f;
            BossDecision();
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

    void BossDecision()
    {
        float distance = Vector3.Distance(transform.position, opponent.position);

        if (distance > attackRange)
        {
            if (Random.value > 0.5f && !isSpecial)
            {
                StartCoroutine(SpecialRoutine());
            }
            else if (Random.value > 0.3 && isSuperSpecial && !isWeapon)
            {
                StartCoroutine(SuperSpecialRoutine());
                DisableSuper();

            }
            else if (Random.value > 0.5 && !isBlock && !isSpecial & !isSuperSpecial)
            {
                float direction = transform.position.x < opponent.position.x ? 1f : -1f;
                rb.linearVelocity = new Vector3(direction * speed, rb.linearVelocity.y, 0f);
                animator.SetBool("forward", true);
            }
            isBlock = false;
            animator.SetBool("block", isBlock);
        }
        else
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            animator.SetBool("forward", false);

            if (!isAttacking)
            {
                StartCoroutine(BossAttackRoutine());
            }
        }

        if (!isJumping && Random.value < 0.1f)
        {
            isJumping = true;
            animator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    IEnumerator BossAttackRoutine()
    {
        isAttacking = true;
        float distanceToOpponent = Vector3.Distance(transform.position, opponent.position);

        string[] attackTriggers = new string[] { "LowPunch", "LowKick", "HardPunch", "HardKick" };
        string selectedAttack = attackTriggers[Random.Range(0, attackTriggers.Length)];
        for (int i = 0; i < 3; i++)
        {
            animator.ResetTrigger(selectedAttack);
            animator.SetTrigger(selectedAttack);
            yield return new WaitForSeconds(0.5f);
        }
        

        isAttacking = false;
    }

    IEnumerator SpecialRoutine()
    {
        isSpecial = true;
        if (Random.value < 0.5f)
        {
            animator.SetTrigger("special");
            specialScript.EnableSpecial();
        }
        else
        {
            animator.SetTrigger("special2");
            specialScript.EnableSpecial2();
        } 
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
}
