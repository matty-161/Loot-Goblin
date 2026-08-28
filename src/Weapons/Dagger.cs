using System;
using Godot;

namespace LootGoblin;

public partial class Dagger : Node2D
{
	public Action AttackStartedEvent;

	public Action AttackEndedEvent;
	
	[Export] private int _damage = 1;
	
	private Sprite2D _sprite;
	private Hitbox2D _hitbox;
	
	private Tween _tween;
	private float _weaponSlotRotation;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("%Sprite2D");
		_sprite.Hide();
		_hitbox = GetNode<Hitbox2D>("%Hitbox2D");
		_hitbox.Damage = _damage;
		_hitbox.Monitorable = false;
		_hitbox.Monitoring = false;

	}
	

	private void Attack()
	{
		AttackStartedEvent?.Invoke();
		_sprite.Hide();
		_tween?.Kill();
		Rotation = 0;

		_hitbox.CanDamage = true;
		
		_tween = GetTree().CreateTween();
		_tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
		_tween.TweenProperty(_sprite, "visible", true, 0); // make sprite visible
		_tween.TweenProperty(_hitbox, "monitorable", true, 0); // enable hitbox monitorable
		_tween.TweenProperty(_hitbox, "monitoring", true, 0); // enable hitbox monitoring

		if ((Math.Abs(_weaponSlotRotation) + Math.PI / 2) % (2 * Math.PI) < Math.PI)
		{
			_tween.TweenProperty(this, "rotation", Math.PI * .5, .25f);
		}
		else
		{
			// _tween.TweenProperty(this, "rotation", 2 * Math.PI + .6f * Math.PI, 0);
			_tween.TweenProperty(this, "rotation", Math.PI * .5, 0);
			_tween.TweenProperty(this, "rotation", 0, .25f);
		}
		
		_tween.TweenProperty(_sprite, "visible", false, 0); // make sprite invisible
		_tween.TweenProperty(_hitbox, "monitorable", false, 0); // disable hitbox monitorable
		_tween.TweenProperty(_hitbox, "monitoring", false, 0); // disable hitbox monitoring
		
		if (AttackEndedEvent != null)
			_tween.TweenCallback(Callable.From(AttackEndedEvent.Invoke));
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("attack"))
		{
			Attack();
		}
	}

	public void SetWeaponSlotRotation(float rotation)
	{
		_weaponSlotRotation = rotation;
	}

	private void OnHitboxHitSomething()
	{
		_hitbox.Monitorable = false;
		_hitbox.Monitoring = false;
	}
}