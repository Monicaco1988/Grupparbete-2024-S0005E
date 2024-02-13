using Godot;
using System;

public partial class CameraPole2 : Marker3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("pivotBack"))
		{
			this.RotateX(0.1f);
		}
		if(Input.IsActionJustPressed("pivotForward"))
		{
			this.RotateX(-0.1f);
		}
	}
}
