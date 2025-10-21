// Ruta: Assets/_Project/Scripts/Character/StateMachine/States/PlayerMoveState.cs
using UnityEngine;

/// <summary>
/// Estado Concreto: Moviéndose.
/// </summary>
public class PlayerMoveState : PlayerBaseState
{
    [SerializeField] private float _moveSpeed = 5.0f;

    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        // TODO: Configurar Animator para la animación "Move"
        // _stateMachine.Animator.SetBool("IsMoving", true);
        Debug.Log("Entrando a MoveState");
    }

    public override void Tick(float deltaTime)
    {
        // --- Lógica de Transición ---
        // Si el jugador suelta el botón de moverse...
        if (_stateMachine.MoveInput == Vector2.zero)
        {
            // ...cambiamos al estado Ocioso.
            _stateMachine.SwitchState(new PlayerIdleState(_stateMachine));
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

        // Prioridad 2: Dejar de moverse
        if (_stateMachine.MoveInput == Vector2.zero)
        {
            _stateMachine.SwitchState(new PlayerIdleState(_stateMachine));
            return;
        }

        // --- Lógica del Estado (Mover al jugador) ---
        Vector3 moveDirection = new Vector3(_stateMachine.MoveInput.x, 0, _stateMachine.MoveInput.y);

        // Usamos el CharacterController para movernos
        _stateMachine.CharacterController.Move(moveDirection * _moveSpeed * deltaTime);

        // TODO: Rotar al jugador para que mire en la dirección del movimiento
        // if (moveDirection != Vector3.zero)
        // {
        //     _stateMachine.transform.rotation = Quaternion.LookRotation(moveDirection);
        // }
    }

    public override void ExitState()
    {
        Debug.Log("Saliendo de MoveState");
    }
}