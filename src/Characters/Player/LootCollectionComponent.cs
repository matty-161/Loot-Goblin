using Godot;

namespace LootGoblin;

public partial class LootCollectionComponent : Area2D
{
	[Export] private VBoxContainer _collectionPrompt;
	[Export] private ProgressBar _collectionBar;

	private float _collectionTime;

	private float _collectionProgress;
	private float CollectionProgress
	{
		get => _collectionProgress;
		set
		{
			if (value < 0) _collectionProgress = 0f;
			_collectionBar.Value = value;
			_collectionProgress = value;
		}
	}

	private LootCollectable _collectable;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

		_collectionPrompt.CallDeferred(Control.MethodName.Hide);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_collectable == null) return;
		
		if (Input.IsActionPressed("interact"))
		{
			CollectionProgress += (float)delta;

			if (CollectionProgress >= _collectionTime)
			{
				GameplaySignalBus.Instance.LootCollectedEvent?.Invoke(_collectable.LootResource.Amount);
				_collectable.QueueFree();
				_collectable = null;
				CollectionProgress = 0;
			}
		}
		else
		{
			if (CollectionProgress > 0)
				CollectionProgress -= (float)delta;
		}
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is not LootCollectable collectable) return;
		
		// GameplaySignalBus.Instance.LootCollectedEvent?.Invoke(collectable.LootResource.Amount);
		// collectable.QueueFree();

		_collectionPrompt.Show();
		_collectionBar.MaxValue = collectable.CollectionTime;
		
		_collectionTime = collectable.CollectionTime;
		_collectable = collectable;
	}

	private void OnAreaExited(Area2D area)
	{
		_collectionPrompt.Hide();
		_collectable = null;
	}
	
	
}
