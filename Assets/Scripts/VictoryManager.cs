using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Configuración de Escena")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    // Método que se llama cuando se gana la partida
    public void ShowVictoryScreen()
    {
        // Pausar el juego
        Time.timeScale = 0f;

        // Mostrar el panel de victoria
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    // Método que invocará el botón 'Volver al Menú'
    public void ReturnToMainMenu()
    {
        // Reanudar el tiempo para que las siguientes escenas funcionen correctamente
        Time.timeScale = 1f;

        // Cargar el menú principal
        SceneManager.LoadScene(mainMenuSceneName);
    }
}