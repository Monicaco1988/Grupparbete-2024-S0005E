using Godot;
using System;

public partial class Storyart_before_start : Control
{
    int scene = 0;

    private GameManager _GetSignalFromGameManager;

    public override void _Ready()
    {
        this.GetChild<TextureRect>(scene).Show();
        _GetSignalFromGameManager = GetNode<GameManager>("/root/GameManager");
        _GetSignalFromGameManager.UpdateGameState2 += Test2;
    }

    private void Test2(GameState tst)
    {
        if (GameState.StoryManager != _GetSignalFromGameManager.State)
            _GetSignalFromGameManager.UpdateGameState2 -= Test2; //deque avoid memleak
    }

    private void OnTimerTimeout()
	{
        GD.Print(scene);
        this.GetChild<TextureRect>(scene).Hide();
        scene++;
        if (scene == 3)
        {

            _GetSignalFromGameManager.EmitSignal(nameof(_GetSignalFromGameManager.UpdateGameState2), 3);

            _GetSignalFromGameManager.UpdateGameState2 -= Test2;
            this.QueueFree();
        }
       
        this.GetChild<TextureRect>(scene).Show();
    }
}
