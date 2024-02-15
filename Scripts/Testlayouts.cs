using Godot;
using Godot.Collections;

public partial class Testlayouts : Node3D
{
	private GameManager _GetStateGameManager;
	Node3D camera;
	Vector3 cameraOffset = new Vector3(0, 10, 15); 
	Node3D spawn1;
	Node3D spawn2;
	Node3D spawn3;
	Node3D spawn4;
	betterCar player = null;
	PackedScene playerScene;
	Node3D buildings;

	int numberOfPlayers;
	int controllerID = 0;

    Array<betterCar> ambulances = new Array<betterCar>();
    public PlayerManagerState state = PlayerManagerState.ACTIVE;

    public enum PlayerManagerState
    {
        ACTIVE,
        IN_ACTIVE
    }


    public void setPlayerManagerState(PlayerManagerState setState)
    {
        this.state = setState;
    }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camera = GetNode<Node3D>("CameraPole1");
		playerScene = GD.Load<PackedScene>("res://Scenes/Car_EvenBetter.tscn");
		spawn1 = GetNode<Node3D>("Spawn1");
		spawn2 = GetNode<Node3D>("Spawn2");
		spawn3 = GetNode<Node3D>("Spawn3");
		spawn4 = GetNode<Node3D>("Spawn4");
		buildings = GetNode<Node3D>("Buildings");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		if (this.state == PlayerManagerState.IN_ACTIVE)
        {
            return;
        }

		if (player != null)
		{
			camera.GlobalPosition = player.GlobalPosition + cameraOffset;
		}

        
        
        bool playerStateFlag = false;
        if (Input.IsActionJustPressed("spawn1"))
        {
			if(numberOfPlayers == 1)
			{
				player.GlobalPosition = spawn1.GlobalPosition;
			}

            if (ambulances.Count != 0)
            {
                    
                playerStateFlag = true;
                     
            }

			if(!playerStateFlag)
			{
				var playerRoot = playerScene.Instantiate<Node3D>();
				AddChild(playerRoot);
				playerRoot.GlobalPosition = spawn1.GlobalPosition;
				player = playerRoot.GetNode("Player") as betterCar;
				player.setId(controllerID);
                player.SetState(betterCar.PlayerState.ACTIVE);
                this.ambulances.Add(player);
                numberOfPlayers++;
                GD.Print("player created, with player id: " + controllerID);
			}
		}
		if (Input.IsActionJustPressed("spawn2"))
        {

			if(numberOfPlayers == 1)
			{
				player.GlobalPosition = spawn2.GlobalPosition;
			}

            if (ambulances.Count != 0)
            {
                    
                playerStateFlag = true;
                     
            }

			if(!playerStateFlag)
			{
				var playerRoot = playerScene.Instantiate<Node3D>();
				AddChild(playerRoot);
				playerRoot.GlobalPosition = spawn1.GlobalPosition;
				player = playerRoot.GetNode("Player") as betterCar;
				player.setId(controllerID);
                player.SetState(betterCar.PlayerState.ACTIVE);
                this.ambulances.Add(player);
                numberOfPlayers++;
                GD.Print("player created, with player id: " + controllerID);
			}
		}
		if (Input.IsActionJustPressed("spawn3"))
        {
			if(numberOfPlayers == 1)
			{
				player.GlobalPosition = spawn3.GlobalPosition;
			}

            if (ambulances.Count != 0)
            {
                    
                playerStateFlag = true;
                     
            }

			if(!playerStateFlag)
			{
				var playerRoot = playerScene.Instantiate<Node3D>();
				AddChild(playerRoot);
				playerRoot.GlobalPosition = spawn1.GlobalPosition;
				player = playerRoot.GetNode("Player") as betterCar;
				player.setId(controllerID);
                player.SetState(betterCar.PlayerState.ACTIVE);
                this.ambulances.Add(player);
                numberOfPlayers++;
                GD.Print("player created, with player id: " + controllerID);
			}
		}
		if (Input.IsActionJustPressed("spawn4"))
        {
			if(numberOfPlayers == 1)
			{
				player.GlobalPosition = spawn4.GlobalPosition;
			}

            if (ambulances.Count != 0)
            {
                    
                playerStateFlag = true;
                     
            }

			if(!playerStateFlag)
			{
				var playerRoot = playerScene.Instantiate<Node3D>();
				AddChild(playerRoot);
				playerRoot.GlobalPosition = spawn1.GlobalPosition;
				player = playerRoot.GetNode("Player") as betterCar;
				player.setId(controllerID);
                player.SetState(betterCar.PlayerState.ACTIVE);
                this.ambulances.Add(player);
                numberOfPlayers++;
                GD.Print("player created, with player id: " + controllerID);
			}
		}
		if (Input.IsKeyLabelPressed(Key.Key0))
		{
			controllerID = 0;
			player.setId(controllerID);
		}
		if (Input.IsKeyLabelPressed(Key.Key1))
		{
			controllerID = 1;
			player.setId(controllerID);
		}
		if (Input.IsKeyLabelPressed(Key.Q))
		{
			buildings.Visible = true;
		}
		if (Input.IsKeyLabelPressed(Key.W))
		{
			buildings.Visible = false;
		}

	}

    public override void _UnhandledInput(InputEvent @event)
    {

        if (Input.IsJoyButtonPressed(controllerID, JoyButton.Back))
        {
            GetTree().Quit();
        }
    }
}

