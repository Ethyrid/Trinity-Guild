// Ruta: Assets/_Project/Scripts/ScriptableObjects/Characters/Classes/CharacterClassData.cs
using UnityEngine;

// Esta clase [System.Serializable] no es un SO, es un contenedor de datos
// que usaremos dentro de nuestro SO de Clase.
[System.Serializable]
public class BaseStatBlock
{
    public int vitality;
    public int resistance;
    public int mind;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int faith;
    public int luck;
}

[CreateAssetMenu(fileName = "Class_New", menuName = "TrinityGuild/Characters/Class Definition")]
public class CharacterClassData : ScriptableObject
{
    [Header("Info B�sica")]
    public string className;
    [TextArea(3, 5)]
    public string description;

    [Header("L�gica de Recursos")]
    [Tooltip("Asigna el Asset de L�gica de Recurso (ej. Logic_Mana) aqu�.")]
    public ResourceLogicBase resourceLogic; // �Nuestro Patr�n Strategy!

    [Header("Atributos Iniciales")]
    public BaseStatBlock startingStats;

    // [Header("�rbol de Habilidades")]
    // public SkillTree skillTree; // Lo a�adiremos en un hito futuro
}