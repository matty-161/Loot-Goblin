using Godot;

namespace LootGoblin;

public partial class PlayerData : Node
{
    [Export] private HealthComponent _healthComponent;
    
    private CharacterMotor _playerMotor;

    public static Vector2 PlayerPosition;

    public override void _Ready()
    {
        _playerMotor = GetParent<CharacterMotor>();
        _healthComponent.HealthChangedEvent += OnPlayerHealthChanged;
    }

    public override void _PhysicsProcess(double delta)
    {
        PlayerPosition = _playerMotor.GlobalPosition;
    }

    private void OnPlayerHealthChanged(int newHealth)
    {
        GameplaySignalBus.Instance.PlayerHealthChangedEvent?.Invoke(newHealth);
    }
}