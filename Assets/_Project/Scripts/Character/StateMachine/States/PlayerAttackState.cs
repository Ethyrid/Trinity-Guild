// Ruta: Assets/_Project/Scripts/Character/StateMachine/States/PlayerAttackState.cs
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    [Header("Configuración de Ataque")]
    private float _attackDuration = 0.5f; // Duración de la animación (simulada)
    private float _attackStaminaCost = 10f;

    private float _timeSinceAttackStarted;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        Debug.Log("Entrando a AttackState");
        _timeSinceAttackStarted = 0f;

        // Consumir Estamina (¡solo una vez, al entrar!)
        _stateMachine.CharacterStats.SpendStamina(_attackStaminaCost);

        // TODO: Reproducir la animación de ataque
        // _stateMachine.Animator.SetTrigger("Attack_Light");
    }

    public override void Tick(float deltaTime)
    {
        _timeSinceAttackStarted += deltaTime;

        // --- Lógica de Transición (Compromiso de Animación) ---
        // Solo podemos salir de este estado DESPUÉS de que termine la duración.
        if (_timeSinceAttackStarted >= _attackDuration)
        {
            // Al terminar, volvemos a Idle.
            _stateMachine.SwitchState(new PlayerIdleState(_stateMachine));
            return;
        }

        // --- Lógica del Estado ---
        // (Opcional: mover ligeramente al jugador hacia adelante si es un combo)
    }

    public override void ExitState()
    {
        Debug.Log("Saliendo de AttackState");
    }
}