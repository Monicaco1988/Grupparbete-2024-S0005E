using Godot;
using System;
using System.Numerics;

public partial class CameraFollow : Node3D
{
    private int collisionshapes = 0;

    private int playerId = 1;

    // The CollisionShape3D node to check for collisions
    [Export]
	 public Area3D checkpoint;

	// [Export]
	// public Area3D[] checkpointArea; // array of Areas?

	// imports the attributes of the player
	//[Export]
	public Node3D leadingCar;

	// makes the camera follow the target faster och slower
	[Export]
	float cameraSmoothing = 10.0f;

	//Takes the node of the camera
	[Export]
	Node3D cameraRoot;
	public override void _Ready()
	{
		leadingCar = GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot");
        //sets the camera pivot to the leading car position
        this.GlobalPosition = leadingCar.GlobalPosition;
	}

	public void OnPlayerEnter(Node3D playerContainer)
	{
		GD.Print("entered area");

        //debugg and ID of body exiting checkpoint
        GD.Print("Detection Entering" + leadingCar.GetChild<RigidBody3D>(playerId).Name);
        //GD.Print(playerContainer.Name);

        GD.Print(playerContainer.Name);

        //if (playerContainer.Name == _player.GetChild<Godot.CharacterBody3D>(1).Name) // for some reason it wants explicitly use Godot.CharacterBody3D instead of CharacterBody3D. probably because there are several CharacterBody3Ds
        //{//this.GlobalPosition = _player.GetChild<CharacterBody3D>(1).GlobalPosition; Not needed for smooooth transition!!!
        //    playerId = 1;
        //}
        //else if (playerContainer.Name == _player.GetChild<Godot.CharacterBody3D>(0).Name)// for some reason it wants explicitly use Godot.CharacterBody3D instead of CharacterBody3D. probably because there are several CharacterBody3Ds
        //{//this.GlobalPosition = _player.GetChild<CharacterBody3D>(0).GlobalPosition; Not needed for smooooth transition!!!
        //    playerId = 0;
        //}

        ////destroy the collisionshape after a player passes through it!
        //areaNodeToDequeue.GetChild<CollisionShape3D>(collisionshapes).QueueFree();

    }

    public void OnPlayerExit(Node3D playerContainer)
{	
GD.Print("Collision exited with: " + playerContainer,Name);
}

	// private void Area_AreaEntered(Area3D area)
	// {
	// 	leadingCar.Position = area.Position;
	// 	//MoveCameraHere
	// }

	public override void _Process(double delta)
	{
		this.GlobalPosition = this.GlobalPosition.Lerp(leadingCar.GetChild<RigidBody3D>(playerId).GlobalPosition, (float)delta * cameraSmoothing);

		// foreach (checkpoint in checkpointArea) // do i have to make checkpoints as array?
		// {
		// 	checkpointArea.AreaEntered += Area_AreaEntered;
		// }

		//GD.Print(checkpoint.Position.X);


		//foreach (checkpoint in root as area)
		//      {S
		//          area.AreaEntered += Area_AreaEntered;
		//      }
		//LookAt(leadingCar.Position, Godot.Vector3.Up);

		//GetNode<RigidBody3D>("/root/World/Player").LookAt(Position, Godot.Vector3.Up);
	}
}




