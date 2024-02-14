using Godot;
using System;

public partial class Menu : Control
{
    // creating a MainScene class to put the state in from GameManager
    private GameManager getSignalFromGameManager;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        //Getting signal from GameManager.
        getSignalFromGameManager = GetNode<GameManager>("/root/GameManager");
        // links the signal to the function from MainScene
        getSignalFromGameManager.UpdateGameStateSignal += UpdateGameState;
    }

    private void UpdateGameState(GameState newState)
    {
        GD.Print("Working");

        // Here is an example that i can get any variable from GameManager. Then i can use it inside this subclass if i want. I might want to change the State from inside the MenuManager and then send the State back with the EMIT function.
        //GameManager thiss = GameManager._instance;
        //thiss.State++;

        //throw new NotImplementedException();
    }

    // will start the game if start button is pressed.this should be in the menumanager
    public void OnStartButton()
    {
        GD.Print("Hello there. starting World scene");

        // changes the state to PlayerManager
        getSignalFromGameManager.State++;

        //sends/emits back a signal that the gamestate has changed to levelManager
        getSignalFromGameManager.EmitSignal(nameof(getSignalFromGameManager.UpdateGameStateSignal), 2); // if this give an error change to 1.


        // jag vill unsubscriba denna för att det inte ska bli en null när "subject" skickar en signal 
        getSignalFromGameManager.UpdateGameStateSignal -= UpdateGameState;

        //Removes the MenuManager when done
        QueueFree();
        //this.Hide(); No need to hide it because im removing it

    }

    // will quit game if quit button is pressed. this should be in the menumanager
    public void OnQuitButton()
    {
        GD.Print("Quit Game");
        GetTree().Quit();

    }
}
