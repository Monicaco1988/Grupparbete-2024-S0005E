using Godot;
using System;

public partial class Follow_Camera : Marker3D
{
	private int playerId = 0;
	private int collisionshapes = 0;

	public Node3D _player;

	[Export]
	public Area3D areaNodeToDequeue;


	//[Export]
	//public Node3D _player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot");
		this.GlobalPosition = _player.GetChild<RigidBody3D>(playerId).GlobalPosition;
	}


	public void OnPlayerEnter(Node3D playerContainer)
	{
		//debugg
		GD.Print("Player entered:" + playerContainer.Name);




        //destroy the collisionshape after a player passes through it!
        areaNodeToDequeue.GetChild<CollisionShape3D>(collisionshapes).QueueFree();
    }



    public override void _Process(double delta)
	{
		//update position of camera and camerapivotarm to the position of the player. linear interpolation for soft follow. Delta * 5 for acceleration to player position
        this.GlobalPosition = this.GlobalPosition.Lerp(_player.GetChild<RigidBody3D>(playerId).GlobalPosition, (float)delta * 5f);

    }
}
