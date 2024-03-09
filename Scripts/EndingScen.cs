using Godot;
using System;

public partial class EndingScen : Control
{
    [Export]
    private Label winningPlayer;

	private Button _pressButton;
	private Button resetGameButton;
	private Button quitGameButton;

	//public static MenuManager Instance;

	private GameManager _GetSignalFromGameManager;

	public override void _Ready()
	{
		resetGameButton = GetNode<Button>("HBoxContainer/VBoxContainer/Button");
        quitGameButton = GetNode<Button>("HBoxContainer/VBoxContainer/Button2");
		resetGameButton.GrabFocus();
        _GetSignalFromGameManager = GetNode<GameManager>("/root/GameManager");
		_GetSignalFromGameManager.UpdateGameState2 += Test2;
        WinningPlayer();
	}



    public void WinningPlayer()
    {
        winningPlayer.Text = "Contgrats! \n" + player_manager.instance.leader + "\n Saved a granny!";

        if(player_manager.instance.leader == null)
            winningPlayer.Text = "Contgrats! \n" + "You Have all " + "\n Saved a granny!";
    }




    private void Test2(GameState tst)
	{
		if(GameState.Menu != _GetSignalFromGameManager.State)
			_GetSignalFromGameManager.UpdateGameState2 -= Test2; //deque avoid memleak
	}

    public override void _Process(double delta)
    {

        var controllers = Input.GetConnectedJoypads();

        foreach (var controller in controllers)
        {
            // same as pushing start with the mouse on the button but on the x-box controller instead
            if (Input.IsActionJustReleased("MenuSelect") && resetGameButton.HasFocus())//
            {
                GetTree().Paused = false;
                _GetSignalFromGameManager.EmitSignal(nameof(_GetSignalFromGameManager.UpdateGameState2), 0);

                _GetSignalFromGameManager.UpdateGameState2 -= Test2;
                QueueFree();
            }
            // same as pushing quit with the mouse on the button but on the x-box controller instead
            if (Input.IsActionJustReleased("MenuSelect") && quitGameButton.HasFocus())
            {
                GetTree().Quit();
            }
        }
    }

 //   public void OnButtonPressed() // when pressing Start the State should change to PlayerState but now it would be nice with whichever tho...
	//{
 //       GetTree().Paused = false;
 //       _GetSignalFromGameManager.EmitSignal(nameof(_GetSignalFromGameManager.UpdateGameState2), 0);

	//	_GetSignalFromGameManager.UpdateGameState2 -= Test2;

 //       //GetNode<Control>("/root/GameManager/AudioManager").QueueFree();
	//	Destroy();

 //   }


	void OnButtonPressed2() 
	{
		GetTree().Quit();
	}

	void Destroy()
	{
        QueueFree();
    }
}
