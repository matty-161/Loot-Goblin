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
		_weapon.AttackStarted += () =>
		{
			LookAt(GetGlobalMousePosition());
			_weapon.SetWeaponSlotRotation(Rotation);
			_isAttacking = true;
		};
		_weapon.AttackEnded +=  () => _isAttacking = false;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!_isAttacking)
		{
			LookAt(GetGlobalMousePosition());
			_weapon.SetWeaponSlotRotation(Rotation);
		}
		GD.Print(Rotation % (Math.PI * 2));
	}
}