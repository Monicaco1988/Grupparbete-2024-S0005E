using Godot;
using System;
using System.Runtime.CompilerServices;
using static System.Formats.Asn1.AsnWriter;

public partial class GameManager : Control
{
    //calls GameState enum list to makes a class for the different states that the game has
    public GameState State;

    //this sends a signal everywhere to all scripts and thells them that there that if something happens in this function (in my case "UpdateGameState2") then it will send a signal so that it triggers an action everywhere where the scripts are listening
    //to this signal -> UpdateGameState2"
    [Signal]
    public delegate void UpdateGameStateSignalEventHandler(GameState newState);

    public override void _Ready()
    {
        //starts of with a GameState.Menu) when initializing the game
        UpdateGameState(GameState.Menu);
        // links the signal to the function
        UpdateGameStateSignal += UpdateGameState;
    }

    //function updates state of what scene will be instantiated
    private void UpdateGameState(GameState newState)
    {
        //setting variable 
        State = newState;

        //State Pattern to select the different gamestates
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
            case GameState.AudioManager:
                HandleAudioSelect();
                break;
        }
    }

    //functions to handle different Managers
    private void HandleAudioSelect()
    {
        throw new NotImplementedException();
    }

    private void HandleLevelSelect()
    {
        //instanciates the level
        var sceneWorld = ResourceLoader.Load<PackedScene>("res://Scenes/World.tscn").Instantiate<Node3D>();
        AddChild(sceneWorld);
    }

    private void HandlePlayerSelect()
    {
        throw new NotImplementedException();
    }

    public void HandleMenuSelect()
    {
        //GD.Print("jag kommer hit iaf"); -- Debugging
        //Instanciates the menumanager
        var sceneMenu = ResourceLoader.Load<PackedScene>("res://Scenes/menu.tscn").Instantiate<Control>();
        AddChild(sceneMenu);
    }

    // function to end game by pressing the escape key
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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}

// enum list index of different states that the game has
public enum GameState
{
    Menu,
    PlayerManager,
    LevelManager,
    AudioManager

}