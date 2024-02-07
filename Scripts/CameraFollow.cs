using Godot;
using System;

public partial class CameraFollow : Node3D
{

	[Export]
	public Node3D leadingCar;
	[Export]
	float cameraSmoothing = 10.0f;

	[Export]
	Node3D cameraRoot;
	public override void _Ready()
	{
		this.GlobalPosition = leadingCar.GlobalPosition;

	}

	private void Area_AreaEntered(Area3D area)
	{
		leadingCar.Position = area.Position;
		//MoveCameraHere
	}

	public override void _Process(double delta)
	{
		cameraRoot.GlobalPosition = cameraRoot.GlobalPosition.Lerp(leadingCar.GlobalPosition, (float)delta * cameraSmoothing);

		//for (each checkpoint in root as area)
		//      {
		//          area.AreaEntered += Area_AreaEntered;
		//      }
	}
}
