using Godot;
using Godot.Collections;
using System;

public partial class player_manager : Node3D
{
	PackedScene playerScene;
	int numberOfPlayers;
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
		playerScene = GD.Load<PackedScene>("res://Scenes/Car.tscn");
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
                
                
                //CreatePlayer(controller);
                if (!playerStateFlag)
                {
                    var player = playerScene.Instantiate<betterCar>();
                    AddChild(player);
                    player.setId(controller);
                    player.SetState(betterCar.PlayerState.ACTIVE);
                    this.ambulances.Add(player);
                    GD.Print("player created, with player id: " + controller);
                }  
            }
        }
       
        
	}

	public void CreatePlayer(int playerId)
	{
		var player = this.playerScene.Instantiate<betterCar>();
		player.setId(playerId);
		this.ambulances.Add(player);
	}
	
}
