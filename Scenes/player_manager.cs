using Godot;
using Godot.Collections;
using System;

public partial class player_manager : Node3D
{
	PackedScene playerScene;
	int numberOfPlayers;
	Array<betterCar> ambulances;

    public enum PlayerManagerState
    {
        ACTIVE,
        IN_ACTIVE
    }

	public PlayerManagerState state = PlayerManagerState.IN_ACTIVE;
	public void setPlayerManagerState(PlayerManagerState setState)
	{
		this.state = setState;
	}


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		playerScene = GD.Load<PackedScene>("res://Scenes/Car.tscn");
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
            if (Input.IsJoyButtonPressed(controller, JoyButton.Start))
            {

            }
        }
       
        playerScene.Instantiate();
	}

	public void CreatePlayer(int playerId)
	{
		var player = this.playerScene.Instantiate<betterCar>();
		player.setId(playerId);
		this.ambulances.Add(player);
	}
	
}
