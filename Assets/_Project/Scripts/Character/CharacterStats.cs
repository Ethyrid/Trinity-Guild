// Ruta: Assets/_Project/Scripts/Character/CharacterStats.cs
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Este MonoBehaviour vive en el personaje (Jugador, Bot, Enemigo).
/// Gestiona los atributos base y calcula las estadísticas derivadas (GDD 7.4).
/// </summary>
public class CharacterStats : MonoBehaviour
{
    [Header("Definición de Clase")]
    [SerializeField] private CharacterClassData _classData;

    // Diccionario para los Atributos Base (Nivel 1, 2, 3...)
    // Esto almacena la *inversión* de XP General del jugador.
    private Dictionary<BaseAttribute, int> _baseAttributes;

    // Estadísticas Derivadas (Calculadas)
    public float MaxHealth { get; private set; }
    public float MaxStamina { get; private set; }
    public float Poise { get; private set; }
    // ...y todas las demás (CritChance, Damage, etc.)

    // Referencia al Recurso de Clase (Maná, Ira...)
    // Nota: Instanciamos el SO para que sea único para este personaje
    public ResourceLogicBase Resource { get; private set; }

    private void Awake()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        // 1. Inicializar Atributos Base desde el SO
        _baseAttributes = new Dictionary<BaseAttribute, int>
        {
            { BaseAttribute.Vitality, _classData.startingStats.vitality },
            { BaseAttribute.Resistance, _classData.startingStats.resistance },
            { BaseAttribute.Mind, _classData.startingStats.mind },
            { BaseAttribute.Strength, _classData.startingStats.strength },
            { BaseAttribute.Dexterity, _classData.startingStats.dexterity },
            { BaseAttribute.Intelligence, _classData.startingStats.intelligence },
            { BaseAttribute.Faith, _classData.startingStats.faith },
            { BaseAttribute.Luck, _classData.startingStats.luck }
        };

        // 2. Instanciar e Inicializar el Recurso de Clase (Patrón Strategy)
        Resource = Instantiate(_classData.resourceLogic);
        Resource.Initialize(this);

        // 3. Calcular Estadísticas Derivadas
        CalculateDerivedStats();
    }

    private void CalculateDerivedStats()
    {
        // Implementamos las fórmulas del GDD (Sección 7.4.3)
        MaxHealth = GetBaseAttributeValue(BaseAttribute.Vitality) * 10; // + BonusEquipo
        MaxStamina = GetBaseAttributeValue(BaseAttribute.Resistance) * 8; // + BonusEquipo
        Poise = GetBaseAttributeValue(BaseAttribute.Resistance) * 0.2f; // + ValorPoiseArmadura

        // ...calcular el resto
    }

    // Helper para obtener un valor de forma segura
    public int GetBaseAttributeValue(BaseAttribute attribute)
    {
        return _baseAttributes.TryGetValue(attribute, out int value) ? value : 0;
    }

    // TODO: Añadir función "LevelUpAttribute(BaseAttribute attribute)"
    // que será llamada por el "Entrenador" en la Taberna
    // y que gastará XP General (GDD 7.2.2).
}