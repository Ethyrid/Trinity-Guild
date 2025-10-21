// Ruta: Assets/_Project/Scripts/Character/StateMachine/PlayerStateMachine.cs
using UnityEngine;

/// <summary>
/// El cerebro del jugador. Este MonoBehaviour gestiona el estado actual
/// y maneja la l�gica de las transiciones.
/// Referencia: Patr�n State (GDD 7.3.2)
/// </summary>
[RequireComponent(typeof(CharacterController), typeof(CharacterStats), typeof(Animator))] // Asegura componentes
public class PlayerStateMachine : MonoBehaviour
{
    // --- Componentes Cacheados ---
    [HideInInspector] public CharacterController CharacterController { get; private set; }
    [HideInInspector] public CharacterStats CharacterStats { get; private set; }
    [HideInInspector] public Animator Animator { get; private set; }
    [HideInInspector] public PlayerInputActions InputActions { get; private set; }

    // --- Variables de Estado ---
    private PlayerBaseState _currentState;
    public PlayerBaseState CurrentState => _currentState; // Para debugging o UI

    // --- Inputs Le�dos ---
    public Vector2 MoveInput { get; private set; }
    public bool IsAttackPressed { get; private set; }
    // TODO: A�adir inputs para Sprint, Dodge, Jump, etc.

    // --- Gravedad (para Root Motion H�brido) ---
    [Header("Gravedad")]
    [SerializeField] private float _gravity = -9.81f;
    private float _verticalVelocity = 0f;

    // --- Referencias de Estados (Opcional, para optimizar) ---
    // En lugar de 'new PlayerIdleState(this)' cada vez, podr�amos tener instancias aqu�.
    // public PlayerIdleState IdleState { get; private set; }
    // public PlayerMoveState MoveState { get; private set; }
    // public PlayerAttackState AttackState { get; private set; }

    private void Awake()
    {
        // Cachear componentes
        CharacterController = GetComponent<CharacterController>();
        CharacterStats = GetComponent<CharacterStats>();
        Animator = GetComponent<Animator>(); // Ya no usamos GetComponentInChildren si el Animator est� en el mismo objeto
        InputActions = new PlayerInputActions();

        // Inicializar estados si usamos la optimizaci�n
        // IdleState = new PlayerIdleState(this);
        // MoveState = new PlayerMoveState(this);
        // AttackState = new PlayerAttackState(this);
    }

    private void OnEnable()
    {
        InputActions.Player.Enable();
    }

    private void OnDisable()
    {
        InputActions.Player.Disable();
        // Aseg�rate de limpiar el estado si el objeto se desactiva
        _currentState?.ExitState();
        _currentState = null;
    }

    private void Start()
    {
        // Estado inicial
        SwitchState(new PlayerIdleState(this)); // Usamos SwitchState para asegurar Enter/Exit correctos
        // SwitchState(IdleState); // Si usamos la optimizaci�n
    }

    private void Update()
    {
        ReadInput();
        ApplyGravity(); // Aplicar gravedad cada frame
        _currentState?.Tick(Time.deltaTime); // Delegar l�gica al estado actual
    }

    private void ReadInput()
    {
        MoveInput = InputActions.Player.Move.ReadValue<Vector2>();
        IsAttackPressed = InputActions.Player.Attack.WasPressedThisFrame();
        // TODO: Leer otros inputs
    }

    private void ApplyGravity()
    {
        if (CharacterController.isGrounded && _verticalVelocity < 0f)
        {
            // Si estamos en el suelo y la velocidad es negativa (o cero), reseteamos a un valor peque�o negativo
            // para asegurar que el CharacterController permanezca pegado al suelo.
            _verticalVelocity = -2f;
        }
        else
        {
            // Si estamos en el aire, aplicamos la aceleraci�n de la gravedad.
            _verticalVelocity += _gravity * Time.deltaTime;
        }

        // Movemos el CharacterController *solo* en el eje Y.
        // El movimiento XZ ahora es manejado por Root Motion del Animator.
        CharacterController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }


    // M�todo para que los estados cambien a otro estado
    public void SwitchState(PlayerBaseState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState();
        // Debug.Log($"Switched to: {newState.GetType().Name}"); // �til para depurar
    }

    // --- Funci�n para Eventos de Animaci�n ---
    // Esta funci�n ser� llamada por el Animation Event que a�adiremos.
    public void OnAttackHitFrame()
    {
        Debug.Log("ANIMATION EVENT: �Frame de impacto del ataque!");
        // TODO Hito 3.3: Aqu� ir� la l�gica para detectar enemigos y aplicar da�o.
        // Ejemplo:
        // Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward, 1.0f, enemyLayerMask);
        // foreach (Collider hit in hits) { hit.GetComponent<EnemyStats>()?.TakeDamage(damageAmount); }
    }
}