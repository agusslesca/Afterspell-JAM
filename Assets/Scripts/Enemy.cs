using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Estadísticas del Enemigo")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float xpReward = 25f; // <--- Nueva variable para la XP otorgada

    [Header("Ataque al Jugador")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f; // Tiempo en segundos entre cada golpe
    private float lastAttackTime;

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

        // Busca al Player automáticamente en la escena
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Movimiento hacia la posición del jugador
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

    // Primer impacto al chocar los colliders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryAttack(collision.gameObject);
        }
    }

    // Mantiene el daño si el enemigo sigue pegado/empujando al jugador
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryAttack(collision.gameObject);
        }
    }

    private void TryAttack(GameObject playerObj)
    {
        // Solo ataca si ya pasó el tiempo de cooldown desde el último golpe
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth player = playerObj.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
                lastAttackTime = Time.time;
            }
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
        // 1. Otorga la experiencia al Player antes de morir
        if (playerTransform != null)
        {
            PlayerXP playerXP = playerTransform.GetComponent<PlayerXP>();
            if (playerXP != null)
            {
                playerXP.AddXP(xpReward);
            }
        }

        // 2. Destruye al enemigo
        Destroy(gameObject);
    }
}