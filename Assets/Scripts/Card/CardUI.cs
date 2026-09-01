using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegúrate de incluir esto para TextMeshPro

public class CardUI : MonoBehaviour
{
    [Header("Componentes de UI")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button selectButton;

    private CardData currentCardData;
    private CardSelectionManager manager;

    public void SetupCard(CardData data, CardSelectionManager selectionManager)
    {
        currentCardData = data;
        manager = selectionManager;

        if (titleText != null) titleText.text = data.cardName;
        if (descriptionText != null) descriptionText.text = data.description;
        if (iconImage != null && data.icon != null) iconImage.sprite = data.icon;

        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnCardClicked);
        }
    }

    private void OnCardClicked()
    {
        if (manager != null && currentCardData != null)
        {
            manager.SelectCard(currentCardData);
        }
    }
}