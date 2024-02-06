using Godot;
using System;

public partial class CameraHelper2 : CharacterBody3D
{
	[Export]
	public RigidBody3D player;

	[Export]
	public int Speed { get; set; } = 10;

	private Vector3 _cameraVelocity = Vector3.Zero;

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;

		GD.Print(Position.DistanceTo(player.Position));
		if (Position.DistanceTo(player.Position) < 1326) // distance to the ambulance (Player) might change Position to the position of the collision checkpoints?
		{
			direction.Z = player.LinearVelocity.Z;
		}
		

		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();

		}

		_cameraVelocity.Z = direction.Z * Speed;

		Velocity = _cameraVelocity;
		MoveAndSlide();

		//LookAt(player.Position);
	}

}









//Vector3 playerPosition = GetNode<RigidBody3D>("/root/Player").Position; (First way to call the objects from another class)


/*
	[Export]
	public RigidBody3D player; // (second way to call objects from another class)


	private Vector3 position;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//player = GetNode<RigidBody3D>("/root/Player");
		//checkpoint = GetNode<Node3D>("FlagsCheckpoint");

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print(position.X);

		if(player.Position.X > -850)
		{
			position.X += 800;

		}
	}


	*/