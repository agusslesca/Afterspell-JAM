using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Estadísticas de Salud")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    [SerializeField] private Slider healthBar;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        // Configuración inicial de la barra de vida
        UpdateHealthUI();
    }

    // Método que llaman las cartas de upgrade para modificar la vida máxima
    public void ApplyHealthMultiplier(float multiplier)
    {
        float oldMaxHealth = maxHealth;

        // Modifica la vida máxima (ej: 100 * 1.2 = 120)
        maxHealth *= multiplier;

        // Aumenta la vida actual proporcionalmente para no quedar con la barra vacía
        currentHealth *= multiplier;

        UpdateHealthUI();

        Debug.Log($"[CARTA APLICADA] Vida Máxima anterior: {oldMaxHealth} -> Nueva Vida Máxima: {maxHealth}");
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();

        Debug.Log("El jugador recibió " + damageAmount + " de daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void HealPercentage(float percentage)
    {
        if (isDead) return;

        float healAmount = maxHealth * percentage;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();

        Debug.Log($"[CARTA APLICADA] Curación del {percentage * 100}% (+{healAmount} HP). Vida actual: {currentHealth}/{maxHealth}");
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("¡El Jugador ha muerto!");

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(10f);
        }
    }
}