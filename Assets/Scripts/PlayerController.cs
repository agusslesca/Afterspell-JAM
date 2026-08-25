using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{
    [Header("Ataque Magico")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

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
        //Detecta el clicl izquierdo para disparar
        if (Input.GetMouseButtonDown(0)) {

            Shoot();
        }
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();

        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }

        //Pasamos el movimiento del animator si esta en 0 o en 1

        animator.SetFloat("Speed", movement.magnitude);
       
    }

    private void Shoot()
    {
        // 1. Verificaciones de seguridad
        if (projectilePrefab == null || firePoint == null || mainCamera == null) return;

        // 2. OBTENER LA POSICIÓN DEL RATÓN CORRECTAMENTE PARA 2D
        // Usamos una variable temporal para no tocar el Input.mousePosition original
        Vector3 screenMousePos = Input.mousePosition;

        // --- EL TRUCO ESTÁ AQUÍ ---
        // Le decimos a la cámara a qué profundidad (Z) del mundo queremos 
        // proyectar el punto. Para 2D, lo mejor es poner la distancia Z 
        // de la propia cámara pero en positivo.
        screenMousePos.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(screenMousePos);

        // 3. CALCULAR LA DIRECCIÓN "APLANADA" EN 2D
        // Forzamos que ambos puntos tengan Z=0 para que la resta sea pura en X e Y
        Vector3 firePointPos = firePoint.position;
        firePointPos.z = 0;
        mouseWorldPos.z = 0;

        // Ahora restamos sin miedo a la profundidad
        Vector2 shootDirection = (Vector2)(mouseWorldPos - firePointPos);

        // 4. CREAR Y CONFIGURAR EL PROYECTIL
        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        if (projectile != null)
        {
            // Pasamos la dirección limpia
            projectile.Setup(shootDirection);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }
}
