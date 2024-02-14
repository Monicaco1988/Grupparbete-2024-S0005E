using Godot;
using Godot.Collections;
using System;

public partial class player_manager : Node3D
{
	PackedScene playerScene;
	int numberOfPlayers;

	Array<betterCar> ambulances = new Array<betterCar>();
    [Export] Array<Marker3D> Spawnpoints = new Array<Marker3D>();
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
		playerScene = GD.Load<PackedScene>("res://Scenes/Car_EvenBetter.tscn");
		GD.Print("ready");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
        if (this.state == PlayerManagerState.IN_ACTIVE)
        {
			return;
        }
		
        var controllers = Input.GetConnectedJoypads();

		foreach ( var controller in controllers )
		{
			bool playerStateFlag = false;
            if (Input.IsJoyButtonPressed(controller, JoyButton.Start))
            {
                if (ambulances.Count != 0)
                {
                    foreach (var ambulance in ambulances)
                    {
                        if (ambulance.id == controller)
                        {
                            playerStateFlag = true;
                        }
                    }
                }
                
                if (!playerStateFlag)
                {

                    GD.Print(playerScene.ResourcePath);

                    var playerRoot = playerScene.Instantiate<Node3D>();
                    AddChild(playerRoot);
                    playerRoot.GlobalPosition = Spawnpoints[controller].GlobalPosition;
                    var player = playerRoot.GetNode("Player") as betterCar;
                    
                    player.setId(controller);
                    player.SetState(betterCar.PlayerState.ACTIVE);
                    this.ambulances.Add(player);
                    numberOfPlayers++;
                    GD.Print("player created, with player id: " + controller);
                }  
            }
        }
       
        
	}
	
}
