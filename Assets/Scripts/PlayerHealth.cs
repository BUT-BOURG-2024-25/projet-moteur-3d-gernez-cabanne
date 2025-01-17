using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public HealthUI healthUI;
    private Animator animator;

    private bool isInvincible = false;
    public float invincibilityDuration = 2f;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (healthUI != null)
        {
            healthUI.UpdateHealthUI(currentHealth, maxHealth);
        }

    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;

        if (currentHealth < 0) currentHealth = 0;

        if (healthUI != null)
        {
            healthUI.UpdateHealthUI(currentHealth, maxHealth);
        }

        if (currentHealth == 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthUI != null)
        {
            healthUI.UpdateHealthUI(currentHealth, maxHealth);
        }
    }

    private void Die()
    {
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.TriggerGameOver();
        }

    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;

        if (healthUI != null)
        {
            healthUI.UpdateHealthUI(currentHealth, maxHealth);
        }

        Debug.Log($"Max health increased by {amount}. New max health: {maxHealth}");
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}
