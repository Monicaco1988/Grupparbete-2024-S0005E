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
            case GameState.LevelManager:
                HandleLevelSelect();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

    }

    private void HandleLevelSelect()
    {
        throw new NotImplementedException();
    }
    private void HandlePlayerSelect()
    {
        GD.Print("hi there, seems fine");
        //throw new NotImplementedException();
    }
    private void HandleMenuSelect()
    {
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
    LevelManager

}
