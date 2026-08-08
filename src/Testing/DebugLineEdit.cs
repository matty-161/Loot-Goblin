using Godot;
using System;

namespace LootGoblin;

public partial class DebugLineEdit : LineEdit
{
	public Action<int> DebugTextChanged;
	
	public override void _Ready()
	{
		TextChanged += OnTextChanged;
		CallDeferred(MethodName.OnTextChanged, Text);
	}

	private void OnTextChanged(string newText)
	{
		int oldCaretPos = CaretColumn;
		string finalResult = "";
		
		RegEx regex = new();
		regex.Compile("[0-9]");

		int diff = regex.SearchAll(newText).Count - newText.Length;

		foreach (RegExMatch validChar in regex.SearchAll(newText))
		{
			finalResult += validChar.GetString();
		}
		
		SetText(finalResult);

		CaretColumn = oldCaretPos + diff;

		
		if (finalResult.Length < 1) return;
		
		try
		{
			int finalNum = int.Parse(finalResult);
			DebugTextChanged?.Invoke(finalNum);
		}
		catch
		{
			GD.PushError("Cannot parse string: ", finalResult);
		}
		

	}

}
