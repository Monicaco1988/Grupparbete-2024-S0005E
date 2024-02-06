using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{

	//Vector3 playerPosition = GetNode<RigidBody3D>("/root/Player").Position; (First way to call the objects from another class)



	[Export]
	public RigidBody3D player; // (second way to call objects from another class)

	//[Export]
	//public Node3D checkpoint;

	private Node3D checkpoint;

	private Vector3 position;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//player = GetNode<RigidBody3D>("/root/Player");
		checkpoint = GetNode<Node3D>("/root/FlagsCheckpoint");

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(player == checkpoint)
		{
			


		}
	}
}
