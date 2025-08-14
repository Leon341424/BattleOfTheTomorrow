using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    public float maxPower;
    private float currentPower = 0f;

    private Animator animator;

    private Image healthBarFill;
    private Image powerBarFill;

    private Player1Control playerControl;
    private Player1ControlKeyboard playerControl1;
    CapsuleCollider col;
    private GameManager gameManager;
    private bool isThrow = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthUIPlayer();
        UpdatePowerUIPlayer();
        playerControl = GetComponent<Player1Control>();
        playerControl1 = GetComponent<Player1ControlKeyboard>();
        healthBarFill = GameObject.FindWithTag("LifeBarP1").GetComponent<Image>();
        powerBarFill = GameObject.FindWithTag("PowerBarP1").GetComponent<Image>();
        col = GetComponent<CapsuleCollider>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        UpdatePowerUIPlayer();
        if (currentPower >= 100f)
        {
            if (playerControl != null)
            {
                playerControl.EnableSuper();
            }
            else
            {
                playerControl1.EnableSuper();
            }
        }
    }

    public void TakeDamagePlayer(float damage)
    {
        bool playerBlock = playerControl != null && playerControl.isBlock;
        bool player1Block = playerControl1 != null && playerControl1.isBlock;

        if (playerBlock || player1Block)
        {
            damage *= 0.1f;
            GainPower(2f);
        }

        currentHealth -= damage;

        if (!(playerBlock || player1Block) && !isThrow)
        {
            animator.SetTrigger("Damage");
            AudioManager.Instance.PlayOneShot("Punch");
        }
        
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining: {currentHealth}");
        UpdateHealthUIPlayer();

        if (currentHealth <= 0)
        {
            Die();
        }

        DisableThrow();
    }

    void UpdateHealthUIPlayer()
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

    void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        animator.SetTrigger("Die");
        col.direction = 2;
        Player1Control ec = GetComponent<Player1Control>();
        if (ec != null) ec.enabled = false;

        Player1ControlKeyboard p2c = GetComponent<Player1ControlKeyboard>();
        if (p2c != null) p2c.enabled = false;
        gameManager.PlayerWonRound(2);
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
