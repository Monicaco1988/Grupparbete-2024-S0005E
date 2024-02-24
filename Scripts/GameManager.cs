using Godot;
using System;

public partial class GameManager : Node
{
	public GameState State;

	[Signal]
	public delegate void UpdateGameState2EventHandler(GameState newState);

	public override void _Ready()
	{
		UpdateGameState2 += UpdateGameState;
		UpdateGameState(GameState.AudioManager);
		UpdateGameState(GameState.Menu);
	}

	public void UpdateGameState(GameState newState)
	{
		State = newState;
		switch (newState)
		{
			case GameState.Menu:
				HandleMenuSelect();
				break;
			case GameState.PlayerManager:
				HandlePlayerSelect();
				break;
			case GameState.StoryManager:
				HanldeStorySelect();
				break;
			case GameState.LevelManager:
				HandleLevelSelect();
				break;
			case GameState.AudioManager:
				HandleAudioSelect();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
		}

	}

    private void HanldeStorySelect()
    {
        var sceneStory = ResourceLoader.Load<PackedScene>("res://Scenes/Storyart_before_start.tscn").Instantiate<Control>();
        AddChild(sceneStory);
    }

    private void HandleAudioSelect()
	{
	//throw new NotImplementedException();
		var sceneAudio = ResourceLoader.Load<PackedScene>("res://Scenes/audio_manager.tscn").Instantiate<Control>();
		AddChild(sceneAudio);
	}

	private void HandleLevelSelect()
	{
		var sceneWorld = ResourceLoader.Load<PackedScene>("res://Scenes/RacingScene_Multiplayer.tscn").Instantiate<Node3D>();
		AddChild(sceneWorld);
	}
	private void HandlePlayerSelect()
	{
		GD.Print("hi there, seems fine");
		//not doing his right now
		var sceneplayer_manager = ResourceLoader.Load<PackedScene>("res://Scenes/player_manager.tscn").Instantiate<Node3D>();
		AddChild(sceneplayer_manager);
	}
	private void HandleMenuSelect()
	{
		var sceneMenu = ResourceLoader.Load<PackedScene>("res://Scenes/Menumanager.tscn").Instantiate<Control>();
		AddChild(sceneMenu);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
			{
				GetTree().Quit();
			}
		}
	}
}

public enum GameState
{
	Menu,
	PlayerManager,
    StoryManager,
    LevelManager,
	AudioManager,
    
}
