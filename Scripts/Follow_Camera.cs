using Godot;
using System;

public partial class Follow_Camera : Marker3D
{
	int timer = 0;
	
	//constants for identifying the different objects
	static private int playerId = 0;
	//static private int collisionshapes = 0;

	public Node3D _player;

	public Node3D playerContainer1;

	// used for collisioncheck with checkpoints
	[Export]
	public Area3D areaNodeToDequeue;


	public override void _Ready()
	{
		//instantiates the attributes and objects from playermanager
        _player = GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot");
		this.GlobalPosition = _player.GetChild<RigidBody3D>(playerId).GlobalPosition;
	}


	public void OnPlayerEnter(Node3D playerContainer)
	{
		playerContainer1 = playerContainer;

		if (timer > 3)
		{
			//destroy the collisionshape after a player passes through it!
			areaNodeToDequeue.GetChild<CollisionShape3D>(0).QueueFree();

		}
		
	}


	public override void _PhysicsProcess(double delta)
	{
		//GD.Print(timer);
		if (timer > 3)
		{
			//update position of camera and camerapivotarm to the position of the player. linear interpolation for soft follow. Delta * 5 for acceleration to player position
			this.GlobalPosition = this.GlobalPosition.Lerp(playerContainer1.GlobalPosition, (float)delta * 5f);
		}
	}
	private void OnTimerTimeout()
	{
        if (timer<4)
			timer++;
        //_player = GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot");
        //this.GlobalPosition = _player.GetChild<RigidBody3D>(playerId).GlobalPosition;
		//GD.Print(timer);
	}
}
