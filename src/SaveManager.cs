using Godot;
using Godot.Collections;

namespace LootGoblin;

public partial class SaveManager: Node
{
    public static SaveManager Instance { get; private set; }
    
    private Dictionary<string, int> _saveData = new();

    private StringName _highScore = "high_score";
    
    public override void _Ready()
    {
        Instance = this;
        
        SetProcessMode(ProcessModeEnum.Always);

        LoadSaveFile();
    }

    private void LoadSaveFile()
    {
        ConfigFile saveFile = new ConfigFile();

        Error err = saveFile.Load("user://save.cfg");

        if (err != Error.Ok)
        {
            _saveData[_highScore] = 0;
            return;
        }
        
        int score = (int)saveFile.GetValue("player1", _highScore);
        _saveData[_highScore] = score;

    }
    
    public void SaveScore(int score)
    {
        ConfigFile saveFile = new();

        saveFile.SetValue("player1", _highScore, score);
        
        saveFile.Save("user://save.cfg");
        
    }

    public int GetHighScore()
    {
        LoadSaveFile();
        return _saveData != null ? _saveData["high_score"] : 0;
    }
    
}