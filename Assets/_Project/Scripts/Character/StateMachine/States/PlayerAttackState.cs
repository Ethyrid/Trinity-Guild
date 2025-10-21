// Ruta: Assets/_Project/Scripts/Character/StateMachine/States/PlayerAttackState.cs
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    [Header("Configuraci�n de Ataque")]
    private float _attackDuration = 0.5f; // Duraci�n de la animaci�n (simulada)
    private float _attackStaminaCost = 10f;

    private float _timeSinceAttackStarted;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        Debug.Log("Entrando a AttackState");
        _timeSinceAttackStarted = 0f;

        // Consumir Estamina (�solo una vez, al entrar!)
        _stateMachine.CharacterStats.SpendStamina(_attackStaminaCost);

        // TODO: Reproducir la animaci�n de ataque
        // _stateMachine.Animator.SetTrigger("Attack_Light");
    }

    public override void Tick(float deltaTime)
    {
        _timeSinceAttackStarted += deltaTime;

        // --- L�gica de Transici�n (Compromiso de Animaci�n) ---
        // Solo podemos salir de este estado DESPU�S de que termine la duraci�n.
        if (_timeSinceAttackStarted >= _attackDuration)
        {
            // Al terminar, volvemos a Idle.
            _stateMachine.SwitchState(new PlayerIdleState(_stateMachine));
            return;
        }

        // --- L�gica del Estado ---
        // (Opcional: mover ligeramente al jugador hacia adelante si es un combo)
    }

    public override void ExitState()
    {
        Debug.Log("Saliendo de AttackState");
    }
}