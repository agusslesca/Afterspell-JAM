using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [Header("Configuracion de Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthBar;

    private float currentHealth;
    private Animator animator;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;
    private bool isDead = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null){
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;   
        }
    }

    // Metodo que llama el proyectil al chocar
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        //Actualizar el Slider de vida
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Si la vida llega a 0, muere
        if (currentHealth <= 0)
        {
            Die();

        }
    }

    private void Die()
    {
        isDead = true;

        //Ocultar la barra de vida
        if (healthBar != null)
        {

            healthBar.gameObject.SetActive(false);
        }

        //Desactivar colisiones y fisicas para que los proyectiles ya no le peguen
        if (enemyCollider != null) enemyCollider.enabled = false;
        if (rb != null) rb.simulated = false;

        //Activar la animacion de muerte
        if (animator != null)
        {
            animator.SetTrigger("IsDead");
        }

        //Destruir el gambeobject 
        Destroy(gameObject, 0.6f);
    }
}
