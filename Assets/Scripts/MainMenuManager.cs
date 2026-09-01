using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Nombre exacto de tu escena principal de juego
    [SerializeField] private string levelToLoad = "Level 1";

    // Método que llamará el botón
    public void PlayGame()
    {
        // Asegurarse de que el tiempo esté corriendo (por si venimos de un menú de pausa)
        Time.timeScale = 1f;

        // Cargar la escena del nivel
        SceneManager.LoadScene(levelToLoad);
    }
}