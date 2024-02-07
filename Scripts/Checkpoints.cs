using Godot;
using System;

public partial class Checkpoints : Area3D
{

[Export]
public CameraFollow cameraFollow {get;set;}
public void OnPlayerEnter(Node3D playerContainer)
{
	//cameraFollow = playerContainer;
	GD.Print("Detection");
}

public void OnPlayerExit()
{
	cameraFollow = null;

}

}
