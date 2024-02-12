using Godot;
using System;
using System.Numerics;

public partial class CameraPrototyp : Marker3D
{

	
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("zoomIn"))
		{
			
		}
		if(Input.IsActionJustPressed("zoomOut"))
		{
			
		}
		if(Input.IsActionJustPressed("pivotForward"))
		{
			this.RotateX(-0.1f); 
		}
		if(Input.IsActionJustPressed("pivotBack"))
		{
			this.RotateX(0.1f);
		}
		if(Input.IsActionJustPressed("pivotRight"))
		{
			this.RotateY(-0.1f);
		}
		if(Input.IsActionJustPressed("pivotLeft"))
		{
			this.RotateY(0.1f);
		}
	}
}
