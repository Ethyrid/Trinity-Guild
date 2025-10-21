// Ruta: Assets/_Project/Scripts/Character/StateMachine/PlayerBaseState.cs

/// <summary>
/// Clase base abstracta para todos los estados del jugador (Patrón State).
/// </summary>
public abstract class PlayerBaseState
{
    // Protegido para que las clases hijas puedan acceder al "cerebro"
    protected readonly PlayerStateMachine _stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    // Se llama una vez al entrar en el estado
    public abstract void EnterState();

    // Se llama cada frame (en el Update de la StateMachine)
    public abstract void Tick(float deltaTime);

    // Se llama una vez al salir del estado
    public abstract void ExitState();
}