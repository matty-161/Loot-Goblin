using Godot;

namespace LootGoblin;

public partial class LevelTransition : Control
{
	[Export] private int _duration = 1;
	[Export] private ColorRect _colorRect;

	private Timer _timer;
	
	

	private Tween _tween;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
		GameplaySignalBus.Instance.LevelTransitionFadeOutEvent += OnFadeOut;
	}

	public override void _ExitTree()
	{
		GameplaySignalBus.Instance.LevelTransitionFadeOutEvent -= OnFadeOut;
	}

	private void OnFadeOut()
	{
		Show();
		
		_tween?.Kill();
		_tween = GetTree().CreateTween();

		_tween.TweenProperty(_colorRect, "modulate", Colors.White, .5f);
		_tween.TweenCallback(Callable.From(StartTimer));
	}

	private void StartTimer()
	{
		GameplaySignalBus.Instance.LevelTransitionEvent?.Invoke();
		_timer?.QueueFree();
		_timer = new Timer();
		AddChild(_timer);
		_timer.WaitTime = _duration;
		_timer.Start();
		_timer.Timeout += OnTimerTimeout;
	}

	private void OnTimerTimeout()
	{
		_timer.Timeout -= OnTimerTimeout;
		GameplaySignalBus.Instance.LevelTransitionFadeInEvent?.Invoke();
		_tween?.Kill();
		_tween = GetTree().CreateTween();
		_tween.TweenProperty(_colorRect, "modulate", Colors.Transparent, .5f);
		_tween.TweenCallback(Callable.From(Hide));
	}

}