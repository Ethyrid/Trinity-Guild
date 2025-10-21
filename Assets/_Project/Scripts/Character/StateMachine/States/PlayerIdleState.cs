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
        // TODO: Configurar Animator para la animaci�n "Idle"
        // _stateMachine.Animator.SetBool("IsMoving", false);
        Debug.Log("Entrando a IdleState");
    }

    public override void Tick(float deltaTime)
    {
        // --- L�gica de Transici�n ---
        // Si el jugador presiona el bot�n de moverse...
        if (_stateMachine.MoveInput != Vector2.zero)
        {
            _stateMachine.SwitchState(new PlayerMoveState(_stateMachine));
            return;
        }

        // --- L�gica del Estado (si la hay) ---
        // (En Idle, usualmente no hay l�gica en Tick)
    }

    public override void ExitState()
    {
        Debug.Log("Saliendo de IdleState");
    }
}