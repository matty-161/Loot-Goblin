using System;
using Godot;
using LootGoblin.Weapons;

namespace LootGoblin.Player;

public partial class WeaponSlot : Node2D
{
	[Export] private Dagger _weapon;

	private bool _isAttacking;

	public override void _Ready()
	{
		_weapon.AttackStartedEvent += OnAttackStartedEvent;
		_weapon.AttackEndedEvent += OnAttackEndedEvent;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!_isAttacking)
		{
			LookAt(GetGlobalMousePosition());
			_weapon.SetWeaponSlotRotation(Rotation);
		}
		// GD.Print(Rotation % (Math.PI * 2));
	}

	public override void _ExitTree()
	{
		_weapon.AttackStartedEvent -= OnAttackStartedEvent;
		_weapon.AttackEndedEvent -= OnAttackEndedEvent;
	}

	private void OnAttackStartedEvent()
	{
		LookAt(GetGlobalMousePosition());
		_weapon.SetWeaponSlotRotation(Rotation);
		_isAttacking = true;
	}

	private void OnAttackEndedEvent()
	{
		_isAttacking = false;
	}
}