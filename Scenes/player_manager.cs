using Godot;
using Godot.Collections;
using System;

public partial class player_manager : Node3D
{
	PackedScene playerScene;
	int numberOfPlayers;
	Array<betterCar> ambulances;

    enum PlayerState
    {
        ACTIVE,
		IN_ACTIVE
    }

	public PlayerState GetState(PlayerState state)
	{
		switch (state)
		{
            case 0:
                return PlayerState.ACTIVE;
            case 1:
                return PlayerState.IN_ACTIVE;
        }		
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		playerScene = GD.Load<PackedScene>("res://Scenes/Car.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var controllers = Input.GetConnectedJoypads();


		playerScene.Instantiate();
	}

	public void CreatePlayer(int playerId)
	{
		var player = this.playerScene.Instantiate<betterCar>();
		player.id = playerId;
		this.ambulances.Add(player);
	}
	
}
