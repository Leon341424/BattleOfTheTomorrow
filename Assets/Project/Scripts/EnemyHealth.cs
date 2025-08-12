using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    public float maxPower;
    private float currentPower = 0f;
    private Animator animator;

    private Image healthBarFill;
    private Image powerBarFill;

    private Collider colliderObject;
    private EnemyControl enemyControl;
    private Player2Control enemyControl1;
    CapsuleCollider col;
    private GameManager gameManager;

    private bool isThrow = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthUI();
        UpdatePowerUIPlayer();
        enemyControl = GetComponent<EnemyControl>();
        enemyControl1 = GetComponent<Player2Control>();
        Invoke("FindBars", 0.1f);
        /*healthBarFill = GameObject.FindWithTag("LifeBarP2").GetComponent<Image>();
        powerBarFill = GameObject.FindWithTag("PowerBarP2").GetComponent<Image>();*/
        col = GetComponent<CapsuleCollider>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void FindBars()
    {
        healthBarFill = GameObject.FindWithTag("LifeBarP2").GetComponent<Image>();
        powerBarFill = GameObject.FindWithTag("PowerBarP2").GetComponent<Image>();
    }

    void Update()
    {
        UpdatePowerUIPlayer();
        if (currentPower >= 100f)
        {
            if (enemyControl != null)
            {
                enemyControl.EnableSuper();
            }
            else
            {
                enemyControl1.EnableSuper();
            }
        }
    }

    public void TakeDamageEnemy(float damage)
    {
        if ((enemyControl != null && enemyControl.isBlock) || (enemyControl1 != null && enemyControl1.isBlock))
        {
            damage *= 0.1f;
        }
        currentHealth -= damage;
        
        bool enemyBlock = enemyControl != null && enemyControl.isBlock;
        bool enemy1Block = enemyControl1 != null && enemyControl1.isBlock;

        if ((!enemyBlock || !enemy1Block) && !isThrow)
        {
            animator.SetTrigger("Damage");
            AudioManager.Instance.PlayOneShot("Punch");
        }
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining: {currentHealth}");
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            DieEnemy();
        }

        DisableThrow();
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    void UpdatePowerUIPlayer()
    {
        if (powerBarFill != null)
        {
            powerBarFill.fillAmount = currentPower / maxPower;
        }
    }

    public void GainPower(float amount)
    {
        currentPower += amount;
        currentPower = Mathf.Clamp(currentPower, 0, maxPower);
    }

    public void ResetPower()
    {
        currentPower = 0;
    }

    void DieEnemy()
    {
        Debug.Log($"{gameObject.name} died.");
        animator.SetTrigger("Die");
        col.direction = 2;
        GetComponent<EnemyControl>().enabled = false;
        GetComponent<Player2Control>().enabled = false;
        gameManager.PlayerWonRound(1);
        //StartCoroutine(OffControl());
        //SceneManager.LoadScene("combat");
    }

    public void EnableThrow()
    {
        isThrow = true;
    }
    public void DisableThrow()
    {
        isThrow = false;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
