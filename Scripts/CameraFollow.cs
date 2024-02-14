using Godot;
using System;
using System.Numerics;

public partial class CameraFollow : Node3D
{


	private int leadingCarId = 0;

	private int collisionShapes = 0;

	// The CollisionShape3D node to check for collisions
	[Export]
	public Area3D checkpoint;

	// imports the attributes of the player
	[Export]
	public Node3D leadingCar;

	// makes the camera follow the target faster och slower
	[Export]
	float cameraSmoothing = 5.0f;

	//Takes the node of the camera
	[Export]
	Node3D cameraRoot;


	public void Awake()
	{
		// Here is an example that i can get any variable from GameManager. Then i can use it inside this subclass if i want. I might want to change the State from inside the MenuManager and then send the State back with the EMIT function.
		//GameManager._instance.State.;

	}

	// enter tree = Awake fast för godot
	public override void _Ready()
	{
		//sets the camera pivot to the leading car position (the id has to be set later but for now it should find the index 0,1,2,3,4,etc)
		this.GlobalPosition = leadingCar.GetChild<Godot.RigidBody3D>(leadingCarId).GlobalPosition;
	}

	public void OnPlayerEnter(Node3D playerContainer)
	{
		GD.Print("Collision detected with: " + playerContainer.Name);

		if (playerContainer.Name == leadingCar.GetChild<RigidBody3D>(leadingCarId).Name)
		{
			leadingCarId = 1;
		}
		else if (playerContainer.Name == leadingCar.GetChild<RigidBody3D>(leadingCarId).Name)
		{
			leadingCarId = 0;
		}
		checkpoint.GetChild<CollisionShape3D>(collisionShapes).QueueFree();
	}


	public override void _PhysicsProcess(double delta)
	{
		this.GlobalPosition = this.GlobalPosition.Lerp(leadingCar.GetChild<RigidBody3D>(leadingCarId).GlobalPosition, (float)delta * cameraSmoothing);
	}
}




