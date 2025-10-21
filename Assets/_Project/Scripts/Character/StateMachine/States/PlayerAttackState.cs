// Ruta: Assets/_Project/Scripts/Character/StateMachine/States/PlayerAttackState.cs
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    [Header("Configuración de Ataque")]
    // IMPORTANTE: Esta duración ahora debe coincidir o ser ligeramente MENOR
    // que la duración real del clip de animación 'Attack_Light'.
    // Si no, volveremos a Idle antes de que la animación termine visualmente.
    [SerializeField] private float _attackTransitionDelay = 0.8f; // Tiempo para volver a Idle (ajustar a la animación)
    [SerializeField] private float _attackStaminaCost = 10f;

    private float _timeSinceAttackStarted;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        // Debug.Log("Entrando a AttackState");
        _timeSinceAttackStarted = 0f;

        // Consumir Estamina
        _stateMachine.CharacterStats.SpendStamina(_attackStaminaCost);

        // ¡Decirle al Animator que ataque!
        _stateMachine.Animator.SetTrigger("Attack_Light");

        // (Opcional) Reseteamos la velocidad en el Animator por si veníamos corriendo
        _stateMachine.Animator.SetFloat("MoveSpeed", 0f);
    }

    public override void Tick(float deltaTime)
    {
        _timeSinceAttackStarted += deltaTime;

        // --- Lógica de Transición (Compromiso de Animación) ---
        // Esperamos el delay definido antes de permitir la transición.
        // Este delay debe estar sincronizado con la duración de la animación + transición de salida.
        if (_timeSinceAttackStarted >= _attackTransitionDelay)
        {
            // Volvemos a Idle (o podríamos chequear si hay input de movimiento para ir a MoveState).
            // Por simplicidad, volvemos a Idle por ahora.
            _stateMachine.SwitchState(new PlayerIdleState(_stateMachine));
            // _stateMachine.SwitchState(_stateMachine.IdleState); // Si usamos optimización
            return;
        }

        // --- Lógica del Estado ---
        // Durante el ataque, usualmente no hay mucho más que hacer aquí
        // a menos que quieras permitir cancelar el ataque con una esquiva (lo cual NO queremos por ahora).
    }

    public override void ExitState()
    {
        // Debug.Log("Saliendo de AttackState");
        // (Opcional) Asegurarse de resetear el trigger por si acaso, aunque usualmente no es necesario.
        // _stateMachine.Animator.ResetTrigger("Attack_Light");
    }
}