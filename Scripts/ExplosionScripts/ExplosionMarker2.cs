using Godot;
using System;

public partial class ExplosionMarker2 : Marker3D
{
	//need an if statement if second controller is activated ---------------------
	
	[Export]
	CpuParticles3D explosion;

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

	public void OnScreenExist()
	{
		explosion.Emitting = true;
		GD.Print("Player exited screen");
		GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@2").Hide();
	}
	
}
