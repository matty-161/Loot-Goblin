using Godot;
using System;

namespace LootGoblin;
public partial class DifficultyTimerLabel : Label
{
	[Export] private DifficultyManager _difficultyManager;
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = Mathf.RoundToInt(_difficultyManager.RunTime).ToString();
	}
}
