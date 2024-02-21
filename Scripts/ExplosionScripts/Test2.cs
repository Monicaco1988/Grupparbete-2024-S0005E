using Godot;
using System;

public partial class Test2 : Marker3D
{
	//constants for identifying the different objects
	private int playerId = 0;

	public RigidBody3D _player;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//instantiates the attributes and objects from playermanager
		_player = GetNode<RigidBody3D>("/root/GameManager/PlayerManager/@Node3D@2/Player");
		this.GlobalPosition = _player.GlobalPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//update position of camera and camerapivotarm to the position of the player. linear interpolation for soft follow. Delta * 5 for acceleration to player position
		this.GlobalPosition = this.GlobalPosition.Lerp(_player.GlobalPosition, (float)delta * 30f);


	}
}
