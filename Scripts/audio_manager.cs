using Godot;
using System;

public partial class audio_manager : Control
{
	[Export]
	AudioStreamPlayer track_1;

    [Export]
    AudioStreamPlayer track_2;

    [Export]
    AudioStreamPlayer track_3;

    [Export]
    AudioStreamPlayer track_4;

    private GameManager _GetSignalFromGameManager;
    
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	    track_1.Playing = true;

     _GetSignalFromGameManager = GetNode<GameManager>("/root/GameManager");
     _GetSignalFromGameManager.UpdateGameState2 += ChangeMusic;
    }

    public void ChangeMusic(GameState newState)
    {
        GD.Print(newState); // debugging
        //case use this logic if it works down here
        if(newState == GameState.PlayerManager) // newState and _GetSignalFromGameManager.State  are the same
        {
            track_1.Playing = false;
            track_2.Playing = true;
        }

        if (_GetSignalFromGameManager.State == GameState.StoryManager)
        {
            track_2.Playing = false;
            track_3.Playing = true;
        }

        if (_GetSignalFromGameManager.State == GameState.LevelManager)
        {
            track_3.Playing = false;
            track_4.Playing = true;
        }
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
