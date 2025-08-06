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
    CapsuleCollider col;
    private GameManager gameManager;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthUIPlayer();
        UpdatePowerUIPlayer();
        playerControl = GetComponent<Player1Control>();
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
            playerControl.EnableSuper();
        }
    }

    public void TakeDamagePlayer(float damage)
    {
        if (playerControl.isBlock)
        {
            damage *= 0.1f;
            Debug.Log("¡Bloqueando! Daño reducido.");
        }
        currentHealth -= damage;
        if (!playerControl.isBlock)
        {
            animator.SetTrigger("Damage");
        }
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining: {currentHealth}");
        UpdateHealthUIPlayer();

        if (currentHealth <= 0)
        {
            Die();
        }
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
        gameManager.PlayerWonRound(2);
        //colliderObject.enabled = false;
    }
}
