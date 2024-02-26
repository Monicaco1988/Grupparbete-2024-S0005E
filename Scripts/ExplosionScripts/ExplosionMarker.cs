using Godot;
using System;

public partial class ExplosionMarker : Marker3D
{

	[Export]
	CpuParticles3D explosion;

	//constants for identifying the different objects
	//private int playerId = 0;

	public Node3D _player;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//instantiates the attributes and objects from playermanager
		_player = GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot");
		this.GlobalPosition = _player.GetChild<RigidBody3D>(0).GlobalPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(double delta)
	//{
	//	//update position of camera and camerapivotarm to the position of the player. linear interpolation for soft follow. Delta * 5 for acceleration to player position
	//	this.GlobalPosition = this.GlobalPosition.Lerp(_player.GetChild<RigidBody3D>(playerId).GlobalPosition, (float)delta * 30f);


	//}

    public override void _PhysicsProcess(double delta)
    {
        this.GlobalPosition = this.GlobalPosition.Lerp(_player.GetChild<RigidBody3D>(0).GlobalPosition, (float)delta * 30f);

    }

    public void OnScreenExist()
	{
		explosion.Emitting = true;
		GD.Print("Player exited screen");
		_player.GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot").Hide();
	}

}
