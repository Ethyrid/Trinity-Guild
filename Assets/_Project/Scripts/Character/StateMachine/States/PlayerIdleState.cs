// Ruta: Assets/_Project/Scripts/Character/StateMachine/States/PlayerIdleState.cs
using UnityEngine;

/// <summary>
/// Estado Concreto: Ocioso.
/// </summary>
public class PlayerIdleState : PlayerBaseState
{
    // Constructor: Pasa la referencia de la StateMachine a la clase base
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        // TODO: Configurar Animator para la animación "Idle"
        // _stateMachine.Animator.SetBool("IsMoving", false);
        Debug.Log("Entrando a IdleState");
    }

    public override void Tick(float deltaTime)
    {
        // --- Lógica de Transición ---
        // Si el jugador presiona el botón de moverse...
        if (_stateMachine.MoveInput != Vector2.zero)
        {
            _stateMachine.SwitchState(new PlayerMoveState(_stateMachine));
            return;
        }

        // --- Lógica de Transición ---

        // Prioridad 1: Atacar
        // TODO: Añadir comprobación de estamina
        if (_stateMachine.IsAttackPressed /* && _stateMachine.CharacterStats.HasEnoughStamina(10f) */)
        {
            _stateMachine.SwitchState(new PlayerAttackState(_stateMachine));
            return;
        }

        // Prioridad 2: Moverse
        if (_stateMachine.MoveInput != Vector2.zero)
        {
            _stateMachine.SwitchState(new PlayerMoveState(_stateMachine));
            return;
        }

        // --- Lógica del Estado (si la hay) ---
        // (En Idle, usualmente no hay lógica en Tick)
    }

    public override void ExitState()
    {
        Debug.Log("Saliendo de IdleState");
    }
}