// Ruta: Assets/_Project/Scripts/Classes/ResourceLogic/ResourceLogicBase.cs
using UnityEngine;

/// <summary>
/// Patrón Strategy (Base): Define la lógica de un recurso de clase (Maná, Ira, etc.)
/// Hereda de ScriptableObject para que podamos crear "lógicas" como assets
/// y asignarlas en el Inspector.
/// </summary>
public abstract class ResourceLogicBase : ScriptableObject
{
    // Define la interfaz pública para toda lógica de recursos
    public abstract void Initialize(CharacterStats stats);
    public abstract void UpdateResource();
    public abstract bool HasEnoughResource(float amountToSpend);
    public abstract void SpendResource(float amount);
    public abstract float CurrentValue { get; }
    public abstract float MaxValue { get; }
}