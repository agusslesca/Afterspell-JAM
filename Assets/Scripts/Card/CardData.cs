using UnityEngine;


    [CreateAssetMenu(fileName = "NuevaCarta", menuName = "Cartas/Carta Upgrade")]
    public class CardData : ScriptableObject
    {
        public string cardName;
        [TextArea] public string description;
        public Sprite icon;

        [Header("Multiplicadores de estadisticas")]
        public float damageMultiplier = 1f; // ejemplo: 1.25 para +25% de danio
        public float speedMultiplier = 1f;
        public float healthMultiplier = 1f;


    [Header("Efectos Instantaneos")]
    [Tooltip("Porcentaje de vida a recuperar")]
    public float healPercentage = 0f; 
    

    }

