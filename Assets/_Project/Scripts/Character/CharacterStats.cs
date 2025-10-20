// Ruta: Assets/_Project/Scripts/Character/CharacterStats.cs
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Este MonoBehaviour vive en el personaje (Jugador, Bot, Enemigo).
/// Gestiona los atributos base y calcula las estad�sticas derivadas (GDD 7.4).
/// </summary>
public class CharacterStats : MonoBehaviour
{
    [Header("Definici�n de Clase")]
    [SerializeField] private CharacterClassData _classData;

    // Diccionario para los Atributos Base (Nivel 1, 2, 3...)
    // Esto almacena la *inversi�n* de XP General del jugador.
    private Dictionary<BaseAttribute, int> _baseAttributes;

    // Estad�sticas Derivadas (Calculadas)
    public float MaxHealth { get; private set; }
    public float MaxStamina { get; private set; }
    public float Poise { get; private set; }
    // ...y todas las dem�s (CritChance, Damage, etc.)

    // Referencia al Recurso de Clase (Man�, Ira...)
    // Nota: Instanciamos el SO para que sea �nico para este personaje
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

        // 2. Instanciar e Inicializar el Recurso de Clase (Patr�n Strategy)
        Resource = Instantiate(_classData.resourceLogic);
        Resource.Initialize(this);

        // 3. Calcular Estad�sticas Derivadas
        CalculateDerivedStats();
    }

    private void CalculateDerivedStats()
    {
        // Implementamos las f�rmulas del GDD (Secci�n 7.4.3)
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

    // TODO: A�adir funci�n "LevelUpAttribute(BaseAttribute attribute)"
    // que ser� llamada por el "Entrenador" en la Taberna
    // y que gastar� XP General (GDD 7.2.2).
}