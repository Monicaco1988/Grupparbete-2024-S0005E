using Godot;
using System;

public partial class CameraPole3 : Marker3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("zoomIn"))
		{
			Position -= Transform.Origin * 0.01f;
		}
		if(Input.IsActionPressed("zoomOut"))
		{
			Position += Transform.Origin * 0.01f;
		}
	}
}
