using Godot;
using System;

public partial class CameraFollow : Node3D
{


	// // import the checkpoints from the Area3D to detect which player should hav ethe camera on it
	 [Export]
	 public CollisionShape3D checkpoint;

	// [Export]
	// public Area3D[] checkpointArea; // array of Areas?

	// imports the attributes of the player
	[Export]
	public Node3D leadingCar;

	// makes the camera follow the target faster och slower
	[Export]
	float cameraSmoothing = 10.0f;

	//Takes the node of the camera
	[Export]
	Node3D cameraRoot;
	public override void _Ready()
	{
		//sets the camera pivot to the leading car position
		this.GlobalPosition = leadingCar.GlobalPosition;
	
	}



	// private void Area_AreaEntered(Area3D area)
	// {
	// 	leadingCar.Position = area.Position;
	// 	//MoveCameraHere
	// }

	public override void _Process(double delta)
	{
		cameraRoot.GlobalPosition = cameraRoot.GlobalPosition.Lerp(leadingCar.GlobalPosition, (float)delta * cameraSmoothing);

		// foreach (checkpoint in checkpointArea) // do i have to make checkpoints as array?
		// {
		// 	checkpointArea.AreaEntered += Area_AreaEntered;
		// }

		GD.Print(checkpoint.Position.X);


		//foreach (checkpoint in root as area)
		//      {
		//          area.AreaEntered += Area_AreaEntered;
		//      }
		//LookAt(leadingCar.Position);
	}
}




