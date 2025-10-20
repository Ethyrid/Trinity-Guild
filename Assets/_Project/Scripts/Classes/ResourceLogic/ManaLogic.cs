// Ruta: Assets/_Project/Scripts/Classes/ResourceLogic/ManaLogic.cs
using UnityEngine;

/// <summary>
/// Implementación (Strategy Concreto) para el Maná.
/// </summary>
[CreateAssetMenu(fileName = "Logic_Mana", menuName = "TrinityGuild/Classes/Resource Logic/Mana")]
public class ManaLogic : ResourceLogicBase
{
    [SerializeField] private float manaRegenPerSecond = 2.0f;

    private float _currentMana;
    private float _maxMana;
    private CharacterStats _ownerStats;

    public override float CurrentValue => _currentMana;
    public override float MaxValue => _maxMana;

    public override void Initialize(CharacterStats stats)
    {
        _ownerStats = stats;
        // La "Mente" (Mind) define el Maná (GDD 7.4.2)
        _maxMana = stats.GetBaseAttributeValue(BaseAttribute.Mind) * 10; // Fórmula de ejemplo
        _currentMana = _maxMana;
    }

    public override void UpdateResource()
    {
        // Lógica de regeneración
        if (_currentMana < _maxMana)
        {
            _currentMana += manaRegenPerSecond * Time.deltaTime;
            _currentMana = Mathf.Clamp(_currentMana, 0, _maxMana);
        }
    }

    public override bool HasEnoughResource(float amountToSpend)
    {
        return _currentMana >= amountToSpend;
    }

    public override void SpendResource(float amount)
    {
        if (HasEnoughResource(amount))
        {
            _currentMana -= amount;
        }
    }
}