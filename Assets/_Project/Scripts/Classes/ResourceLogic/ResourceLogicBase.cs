// Ruta: Assets/_Project/Scripts/Classes/ResourceLogic/ResourceLogicBase.cs
using UnityEngine;

/// <summary>
/// Patr�n Strategy (Base): Define la l�gica de un recurso de clase (Man�, Ira, etc.)
/// Hereda de ScriptableObject para que podamos crear "l�gicas" como assets
/// y asignarlas en el Inspector.
/// </summary>
public abstract class ResourceLogicBase : ScriptableObject
{
    // Define la interfaz p�blica para toda l�gica de recursos
    public abstract void Initialize(CharacterStats stats);
    public abstract void UpdateResource();
    public abstract bool HasEnoughResource(float amountToSpend);
    public abstract void SpendResource(float amount);
    public abstract float CurrentValue { get; }
    public abstract float MaxValue { get; }
}