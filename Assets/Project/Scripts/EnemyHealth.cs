using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    private Animator animator;

    public Image healthBarFill;

    private Collider colliderObject;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthUI();
        colliderObject = GetComponent<Collider>();
    }

    public void TakeDamageEnemy(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Damage");
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining: {currentHealth}");
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            DieEnemy();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    void DieEnemy()
    {
        Debug.Log($"{gameObject.name} died.");
        //Destroy(gameObject); 
        animator.SetTrigger("Die");
        //colliderObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        colliderObject.enabled = false;
        SceneManager.LoadScene("combat");
        //StartCoroutine(DelayedDieEnemy(3f));
    }

    /*private IEnumerator DelayedDieEnemy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject); 
    }    */
}
