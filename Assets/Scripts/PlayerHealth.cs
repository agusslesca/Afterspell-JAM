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
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    // Método para recibir daño desde otros scripts (o por colisión)
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        // Limitar la vida entre 0 y el máximo
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Actualizar el Slider
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        Debug.Log("El jugador recibió " + damageAmount + " de daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Método opcional para curarse
    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("¡El Jugador ha muerto!");

        // Desactivar el movimiento o reiniciar la escena aquí
        // Opcional: Ocultar el personaje
        gameObject.SetActive(false);
    }

    // Detección de daño por toque continuo o impacto directo de enemigos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Quita 10 de daño al tocar un esqueleto
            TakeDamage(10f);
        }
    }
}