
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [Header("Configuracion de Escena")]
    [SerializeField] private string sceneToLoad = "Nivel1";

    [Header("UI E Interaccion")]
    [SerializeField] private GameObject interactionText;

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (interactionText != null)
        {
            interactionText.SetActive(false);
        }
    }

    private void Update()
    {
        //si el jugador esta cerca y presiona la tecla E
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            EnterPortal();
        }
    }

    private void EnterPortal()
    {
        Debug.Log("Cargando Escena: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
    }

    //Se ejecuta al entrar al area del portal
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionText != null)
            {
                interactionText.SetActive(true);
            }
        }
    }

    // Se ejecuta al salir del area del portal
    private void OnTriggerExit2D(Collider2D collision)
    {
       if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionText != null)
            {
                interactionText.SetActive(false);
            }
        }
    }

}
