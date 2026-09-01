using UnityEngine;
using UnityEngine.UI;

public class PlayerXP : MonoBehaviour
{
    [Header("Ajustes de Experiencia")]
    public float currentXP = 0f;
    public float maxXP = 100f;

    [Header("Referencias UI y Manager")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private CardSelectionManager cardManager;

    private void Start()
    {
        UpdateUI();
    }

    public void AddXP(float amount)
    {
        currentXP += amount;

        // Verificar si sube de nivel
        if (currentXP >= maxXP)
        {
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        currentXP -= maxXP;      // Conserva el sobrante de XP
        maxXP *= 1.2f;           // Aumenta el requisito para el siguiente nivel

        if (cardManager != null)
        {
            cardManager.OpenCardSelection();
        }
    }

    private void UpdateUI()
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = maxXP;
            xpSlider.value = currentXP;
        }
    }
}
