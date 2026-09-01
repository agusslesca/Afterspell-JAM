using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{
    [Header("Ataque Magico")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Camera mainCamera;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Detecta el clic izquierdo para disparar
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    // Método que llaman las cartas de upgrade para modificar la velocidad
    public void ApplySpeedMultiplier(float multiplier)
    {
        float oldSpeed = moveSpeed;
        moveSpeed *= multiplier;

        Debug.Log($"[CARTA APLICADA] Velocidad anterior: {oldSpeed} -> Nueva Velocidad: {moveSpeed}");
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();

        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", movement.magnitude);
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null || mainCamera == null) return;

        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(screenMousePos);

        Vector3 firePointPos = firePoint.position;
        firePointPos.z = 0;
        mouseWorldPos.z = 0;

        Vector2 shootDirection = (Vector2)(mouseWorldPos - firePointPos);

        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Setup(shootDirection);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }
}