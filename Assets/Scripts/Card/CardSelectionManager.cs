using System.Collections.Generic;
using UnityEngine;

public class CardSelectionManager : MonoBehaviour
{
    [Header("Base de Datos")]
    [SerializeField] private List<CardData> allCards;

    [Header("Referencias de UI")]
    [SerializeField] private GameObject selectionPanel;
    [SerializeField] private CardUI[] cardUIButtons;

    public void OpenCardSelection()
    {
        if (allCards.Count < 3)
        {
            Debug.LogWarning("Necesitas al menos 3 cartas en la lista 'allCards'.");
            return;
        }

        Time.timeScale = 0f;
        selectionPanel.SetActive(true);

        List<CardData> availableCards = new List<CardData>(allCards);

        for (int i = 0; i < cardUIButtons.Length; i++)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            CardData cardToAssign = availableCards[randomIndex];

            cardUIButtons[i].SetupCard(cardToAssign, this);
            availableCards.RemoveAt(randomIndex);
        }
    }

    public void SelectCard(CardData chosenCard)
    {
        ApplyUpgradesToPlayer(chosenCard);

        selectionPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ApplyUpgradesToPlayer(CardData card)
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // 1. Manejo de Salud (Vida Máxima y Curación)
            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
            {
                // Aplicar multiplicador de Vida Máxima
                if (card.healthMultiplier != 1f)
                {
                    health.ApplyHealthMultiplier(card.healthMultiplier);
                }

                // Aplicar curación instantánea si la carta tiene porcentaje de curación
                if (card.healPercentage > 0f)
                {
                    health.HealPercentage(card.healPercentage);
                }
            }

            // 2. Aplicar multiplicador de Velocidad de Movimiento
            Playercontroller controller = player.GetComponent<Playercontroller>();
            if (controller != null && card.speedMultiplier != 1f)
            {
                controller.ApplySpeedMultiplier(card.speedMultiplier);
            }

            Debug.Log($"<color=green>¡Carta elegida e integrada con éxito!: {card.cardName}</color>");
        }
        else
        {
            Debug.LogError("No se encontró ningún GameObject con la etiqueta 'Player'. Asegúrate de haber asignado el Tag al jugador.");
        }
    }
}