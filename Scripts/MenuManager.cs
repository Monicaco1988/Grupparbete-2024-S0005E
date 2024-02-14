using Godot;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

public partial class MenuManager : Control
{
    private Button _pressButton;

    //public static MenuManager Instance;

    private GameManager _GetSignalFromGameManager;

    public override void _Ready()
    {
        _GetSignalFromGameManager = GetNode<GameManager>("/root/GameManager");
        _GetSignalFromGameManager.UpdateGameState2 += Test2;
    }

    private void Test2(GameState tst)
    {
        if(GameState.Menu != _GetSignalFromGameManager.State)
            _GetSignalFromGameManager.UpdateGameState2 -= Test2; //deque avoid memleak
    }

    public void OnButtonPressed() // when pressing Start the State should change to PlayerState but now it would be nice with whichever tho...
    {

        _GetSignalFromGameManager.EmitSignal(nameof(_GetSignalFromGameManager.UpdateGameState2), 1);

        _GetSignalFromGameManager.UpdateGameState2 -= Test2;
        QueueFree();
    }


    public override void _Process(double delta)
    {
        var controllers = Input.GetConnectedJoypads();

        foreach (var controller in controllers)
        {
            // same as pushing start with the mouse on the button but on the x-box controller instead
            if (Input.IsJoyButtonPressed(controller, JoyButton.Start))
            {
                _GetSignalFromGameManager.EmitSignal(nameof(_GetSignalFromGameManager.UpdateGameState2), 1);

                _GetSignalFromGameManager.UpdateGameState2 -= Test2;
                QueueFree();
            }
            // same as pushing quit with the mouse on the button but on the x-box controller instead
            if (Input.IsJoyButtonPressed(controller, JoyButton.Back))
            {
                GetTree().Quit();
            }
        }
    }

    void OnButtonPressed2() 
    {
        GetTree().Quit();
    }

    
}