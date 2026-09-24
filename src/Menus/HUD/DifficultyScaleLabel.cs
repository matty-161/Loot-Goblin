using Godot;
using System;

namespace LootGoblin;

public partial class DifficultyScaleLabel : Label
{

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = "D: " + Math.Round(DifficultyManager.DifficultyScale, 2);
	}
}
