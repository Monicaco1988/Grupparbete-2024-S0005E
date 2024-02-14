using Godot;
using System;

public partial class track_decal : Node3D
{
	public Timer skidTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		skidTimer = GetNode<Timer>("Timer");
		StartTimer();
	}

	public void StartTimer()
	{
		skidTimer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void SkidMarkTimeOut()
	{
		QueueFree();
	}
}
