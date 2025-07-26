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

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthUIPlayer();
        UpdatePowerUIPlayer();
        playerControl = GetComponent<Player1Control>();
        healthBarFill = GameObject.FindWithTag("LifeBarP1").GetComponent<Image>();
        powerBarFill = GameObject.FindWithTag("PowerBarP1").GetComponent<Image>();
        /*healthBarFill = GameObject.Find("LifeBarFilledP1").GetComponent<Image>();
        powerBarFill = GameObject.Find("PowerBarFilledP1").GetComponent<Image>();*/
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
        currentHealth -= damage;
        animator.SetTrigger("Damage");
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining: {currentHealth}");
        UpdateHealthUIPlayer();

        if (currentHealth <= 0)
        {
            DieEnemy();
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

    void DieEnemy()
    {
        Debug.Log($"{gameObject.name} died.");
        animator.SetTrigger("Die");
        //colliderObject.enabled = false;
    }
}
