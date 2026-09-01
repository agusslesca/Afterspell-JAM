using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Estadísticas del Enemigo")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float xpReward = 25f;

    [Header("Ataque al Jugador")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    [Header("Animación y Muerte")]
    [SerializeField] private float deathAnimationDuration = 2.0f; // Tiempo de espera para la animación

    [Header("UI (Opcional)")]
    [SerializeField] private UnityEngine.UI.Slider healthBar;

    private float currentHealth;
    private bool isDead = false; // Control para no morir varias veces
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D enemyCollider;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();

        // Busca al Player automáticamente en la escena
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    private void Update()
    {
        // Si está muerto, detiene el movimiento
        if (isDead) return;

        if (playerTransform != null)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerTransform.position,
            speed * Time.deltaTime
        );

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = playerTransform.position.x < transform.position.x;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            TryAttack(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            TryAttack(collision.gameObject);
        }
    }

    private void TryAttack(GameObject playerObj)
    {
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
        if (isDead) return;

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
        isDead = true;

        // 1. Otorga la experiencia al Player
        if (playerTransform != null)
        {
            PlayerXP playerXP = playerTransform.GetComponent<PlayerXP>();
            if (playerXP != null)
            {
                playerXP.AddXP(xpReward);
            }
        }

        // 2. Desactiva colisiones para que no bloquee ni haga daño mientras muere
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        // 3. Oculta la barra de vida si existe
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }

        // 4. Activa el Trigger de animación de muerte
        if (animator != null)
        {
            animator.SetTrigger("IsDead"); // Reemplaza "Die" por el nombre exacto de tu Trigger en el Animator
        }

        // 5. Destruye al enemigo tras esperar la duración de la animación
        Destroy(gameObject, deathAnimationDuration);
    }
}