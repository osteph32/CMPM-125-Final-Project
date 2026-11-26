using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth = 5;
    [SerializeField] private Slider healthBar; // Reference to health bar slider
    [SerializeField] private float invincibilityDuration = 1f;
    private float nextDamageTime = 0f;
    private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        resetHealth();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        // Ignore damage if the player is currently invincible.
        if (Time.time < nextDamageTime)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        updateHealthBar(); // Update the health bar after taking damage.

        nextDamageTime = Time.time + invincibilityDuration; // Set the next time the player can take damage.

        if (currentHealth <= 0) // Handle death
        {
            die();
        }
    }

    public void resetHealth()
    {
        currentHealth = maxHealth;
    }

    private void die()
    {
        SceneManager.LoadScene("Main Menu");
        Debug.Log("Player has died");
    }
    private void updateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
}
