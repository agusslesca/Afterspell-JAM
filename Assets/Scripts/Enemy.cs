using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Estadísticas del Enemigo")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float maxHealth = 50f;

    [Header("UI (Opcional)")]
    [SerializeField] private UnityEngine.UI.Slider healthBar;

    private float currentHealth;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Busca al Player automáticamente en cuanto el WaveSpawner crea al esqueleto
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    private void Update()
    {
        // Se mueve hacia el jugador en cada frame
        if (playerTransform != null)
        {
            MoveTowardsPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Busca el script PlayerHealth en el objeto que tocó
            PlayerHealth player = collision.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(10f); // Quita 10 de vida al jugador
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // Movimiento constante
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerTransform.position,
            speed * Time.deltaTime
        );

        // Voltear el sprite según la posición del jugador
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = playerTransform.position.x < transform.position.x;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}