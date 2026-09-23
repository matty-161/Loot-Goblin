using Godot;

namespace LootGoblin;

public partial class PlayerData : Node
{
    [Export] private HealthComponent _healthComponent;
    
    private CharacterMotor _playerMotor;

    public static Vector2 PlayerPosition;

    private static int _maxHealth;

    public static int MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = value;
            GameEventManager.PlayerMaxHealthSetEvent?.Invoke(value);
        }
    }

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
        GameEventManager.PlayerHealthChangedEvent?.Invoke(newHealth);
        if (newHealth <= 0)
        {
            GameEventManager.PlayerDied?.Invoke();
        }
    }
}