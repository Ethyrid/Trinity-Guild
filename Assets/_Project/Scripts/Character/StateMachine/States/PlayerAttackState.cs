// Ruta: Assets/_Project/Scripts/Character/StateMachine/States/PlayerAttackState.cs
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    [Header("Configuraci�n de Ataque")]
    // IMPORTANTE: Esta duraci�n ahora debe coincidir o ser ligeramente MENOR
    // que la duraci�n real del clip de animaci�n 'Attack_Light'.
    // Si no, volveremos a Idle antes de que la animaci�n termine visualmente.
    [SerializeField] private float _attackTransitionDelay = 0.8f; // Tiempo para volver a Idle (ajustar a la animaci�n)
    [SerializeField] private float _attackStaminaCost = 10f;

    private float _timeSinceAttackStarted;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        // Debug.Log("Entrando a AttackState");
        _timeSinceAttackStarted = 0f;

        // Consumir Estamina
        _stateMachine.CharacterStats.SpendStamina(_attackStaminaCost);

        // �Decirle al Animator que ataque!
        _stateMachine.Animator.SetTrigger("Attack_Light");

        // (Opcional) Reseteamos la velocidad en el Animator por si ven�amos corriendo
        _stateMachine.Animator.SetFloat("MoveSpeed", 0f);
    }

    public override void Tick(float deltaTime)
    {
        _timeSinceAttackStarted += deltaTime;

        // --- L�gica de Transici�n (Compromiso de Animaci�n) ---
        // Esperamos el delay definido antes de permitir la transici�n.
        // Este delay debe estar sincronizado con la duraci�n de la animaci�n + transici�n de salida.
        if (_timeSinceAttackStarted >= _attackTransitionDelay)
        {
            // Volvemos a Idle (o podr�amos chequear si hay input de movimiento para ir a MoveState).
            // Por simplicidad, volvemos a Idle por ahora.
            _stateMachine.SwitchState(new PlayerIdleState(_stateMachine));
            // _stateMachine.SwitchState(_stateMachine.IdleState); // Si usamos optimizaci�n
            return;
        }

        // --- L�gica del Estado ---
        // Durante el ataque, usualmente no hay mucho m�s que hacer aqu�
        // a menos que quieras permitir cancelar el ataque con una esquiva (lo cual NO queremos por ahora).
    }

    public override void ExitState()
    {
        // Debug.Log("Saliendo de AttackState");
        // (Opcional) Asegurarse de resetear el trigger por si acaso, aunque usualmente no es necesario.
        // _stateMachine.Animator.ResetTrigger("Attack_Light");
    }
}