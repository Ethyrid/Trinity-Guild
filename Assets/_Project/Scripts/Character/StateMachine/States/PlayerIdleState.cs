// Ruta: Assets/_Project/Scripts/Character/StateMachine/States/PlayerIdleState.cs
using UnityEngine;

/// <summary>
/// Estado Concreto: Ocioso.
/// </summary>
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        // Decirle al Animator que ya no nos movemos
        _stateMachine.Animator.SetBool("IsMoving", false);
        // Reseteamos la velocidad por si acaso
        _stateMachine.Animator.SetFloat("MoveSpeed", 0f);
        // Debug.Log("Entrando a IdleState"); // Puedes quitar los Debug.Log si ya funciona
    }

    public override void Tick(float deltaTime)
    {
        // --- L�gica de Transici�n ---

        // Prioridad 1: Atacar
        // (A�adimos la comprobaci�n de estamina aqu� para evitar entrar al estado si no podemos)
        if (_stateMachine.IsAttackPressed && _stateMachine.CharacterStats.HasEnoughStamina(10f)) // TODO: Usar variable para costo
        {
            _stateMachine.SwitchState(new PlayerAttackState(_stateMachine));
            // _stateMachine.SwitchState(_stateMachine.AttackState); // Si usamos optimizaci�n
            return;
        }

        // Prioridad 2: Moverse
        if (_stateMachine.MoveInput != Vector2.zero)
        {
            _stateMachine.SwitchState(new PlayerMoveState(_stateMachine));
            // _stateMachine.SwitchState(_stateMachine.MoveState); // Si usamos optimizaci�n
            return;
        }

        // --- L�gica del Estado ---
        // (Nada que hacer en Idle)
    }

    public override void ExitState()
    {
        // Debug.Log("Saliendo de IdleState");
    }
}