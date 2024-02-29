using Godot;
using System;

public partial class EndingScen : Control
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
        GetTree().Paused = false;
        _GetSignalFromGameManager.EmitSignal(nameof(_GetSignalFromGameManager.UpdateGameState2), 0);

		_GetSignalFromGameManager.UpdateGameState2 -= Test2;

        //GetNode<Control>("/root/GameManager/AudioManager").QueueFree();
		Destroy();

    }


	void OnButtonPressed2() 
	{
		GetTree().Quit();
	}

	void Destroy()
	{
        QueueFree();
    }
}
