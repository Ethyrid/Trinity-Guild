// Ruta: Assets/_Project/Scripts/Character/StateMachine/PlayerStateMachine.cs
using UnityEngine;

/// <summary>
/// El cerebro del jugador. Este MonoBehaviour gestiona el estado actual
/// y maneja la lógica de las transiciones.
/// Referencia: Patrón State (GDD 7.3.2)
/// </summary>
public class PlayerStateMachine : MonoBehaviour
{
    // Referencia al estado actual (ej. Idle, Moving, Attacking)
    private PlayerBaseState _currentState;

    // Propiedades públicas para que los estados accedan a componentes
    // comunes (principio DRY).
    [HideInInspector] public CharacterController CharacterController { get; private set; }
    [HideInInspector] public CharacterStats CharacterStats { get; private set; }
    [HideInInspector] public Animator Animator { get; private set; }

    // Propiedad para el input (la moveremos a una interfaz en el futuro)
    public Vector2 MoveInput { get; private set; }

    // Referencia a nuestro asset de Input Actions
    private PlayerInputActions _inputActions;

    private void Awake()
    {
        // Cachear componentes
        CharacterController = GetComponent<CharacterController>();
        CharacterStats = GetComponent<CharacterStats>();
        Animator = GetComponentInChildren<Animator>(); // Asumimos que el Animator está en un hijo

        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    private void Start()
    {
        // Estado inicial
        _currentState = new PlayerIdleState(this);
        _currentState.EnterState();
    }

    private void Update()
    {
        // Leer el input cada frame
        MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();

        // Delegar la lógica de Update al estado actual
        _currentState?.Tick(Time.deltaTime);
    }

    // Método para que los estados cambien a otro estado
    public void SwitchState(PlayerBaseState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }
}