// Ruta: Assets/_Project/Scripts/Character/StateMachine/States/PlayerMoveState.cs
using UnityEngine;

/// <summary>
/// Estado Concreto: Moviéndose. Ahora usa Root Motion.
/// </summary>
public class PlayerMoveState : PlayerBaseState
{
    // Ya no necesitamos _moveSpeed aquí, la velocidad viene de la animación.
    [Header("Configuración de Rotación")]
    [SerializeField] private float _rotationSpeed = 10f; // Qué tan rápido rota el personaje

    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        _stateMachine.Animator.SetBool("IsMoving", true);
        // Debug.Log("Entrando a MoveState");
    }

    public override void Tick(float deltaTime)
    {
        // --- Lógica de Transición ---

        // Prioridad 1: Atacar
        if (_stateMachine.IsAttackPressed && _stateMachine.CharacterStats.HasEnoughStamina(10f)) // TODO: Usar variable para costo
        {
            _stateMachine.SwitchState(new PlayerAttackState(_stateMachine));
            // _stateMachine.SwitchState(_stateMachine.AttackState); // Si usamos optimización
            return;
        }

        // Prioridad 2: Dejar de moverse
        if (_stateMachine.MoveInput == Vector2.zero)
        {
            _stateMachine.SwitchState(new PlayerIdleState(_stateMachine));
            // _stateMachine.SwitchState(_stateMachine.IdleState); // Si usamos optimización
            return;
        }

        // --- Lógica del Estado ---

        // 1. Calcular dirección de movimiento relativa a la cámara
        Vector3 moveDirection = CalculateMoveDirection();

        // 2. Rotar al jugador para que mire en esa dirección
        Rotate(moveDirection, deltaTime);

        // 3. ¡YA NO LLAMAMOS A CharacterController.Move() PARA XZ! Root Motion lo hace.

        // 4. Actualizar el parámetro 'MoveSpeed' del Animator para el Blend Tree
        // Usamos la magnitud del input, no la velocidad real (ya que Root Motion la controla).
        // Esto le dice al Blend Tree si mezclar Idle, Walk o Run.
        float inputMagnitude = _stateMachine.MoveInput.magnitude;
        _stateMachine.Animator.SetFloat("MoveSpeed", inputMagnitude, 0.1f, deltaTime); // El 0.1f es un suavizado
    }

    private Vector3 CalculateMoveDirection()
    {
        // Obtener dirección relativa a la cámara
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0; // Ignorar inclinación vertical de la cámara
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calcular la dirección final basada en el input
        return (cameraForward * _stateMachine.MoveInput.y + cameraRight * _stateMachine.MoveInput.x).normalized;
    }

    private void Rotate(Vector3 moveDirection, float deltaTime)
    {
        if (moveDirection != Vector3.zero)
        {
            // Calcular la rotación deseada
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            // Interpolar suavemente hacia la rotación deseada
            _stateMachine.transform.rotation = Quaternion.Slerp(
                _stateMachine.transform.rotation,
                targetRotation,
                _rotationSpeed * deltaTime
            );
        }
    }


    public override void ExitState()
    {
        // Debug.Log("Saliendo de MoveState");
    }
}