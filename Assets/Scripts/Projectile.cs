using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float lifeTime = 3f;

    private Vector2 moveDirection;

    //Metodo para inicializar la direccion desde el player

    public void Setup(Vector2 direction)
    {
        moveDirection = direction.normalized;

        //Rotar el sprite hacia la dirreccion a la que vuela

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Se destruye automaticamente tras unos segundos si no choca con nada
        Destroy(gameObject, lifeTime);

    }

    private void Update()
    {
        //Mueve el proyectil cuadro a cuadro
        transform.Translate(moveDirection * speed *  Time.deltaTime, Space.World);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca con el jugador, NO hace nada (lo ignora)
        if (collision.CompareTag("Player")) return;

        // Si choca con un enemigo, le hace daño
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(25f); // Cambia 25 por el daño que quieras
            }
        }

        // Se destruye al chocar con cualquier otra cosa (Paredes, Enemigos, etc.)
        Destroy(gameObject);
    }
}
