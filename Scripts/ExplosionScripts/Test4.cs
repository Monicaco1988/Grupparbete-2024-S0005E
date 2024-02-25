using Godot;
using System;

public partial class Test4 : Marker3D
{
    //constants for identifying the different objects
    private int playerId = 0;

    public Node3D _player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //instantiates the attributes and objects from playermanager
        _player = GetNode<Node3D>("/root/GameManager/PlayerManager");
        // check if node exists
        if (_player.HasNode("/root/GameManager/PlayerManager/@Node3D@4"))
        {
            _player = GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@4");
            this.GlobalPosition = _player.GetChild<RigidBody3D>(playerId).GlobalPosition;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_player.HasNode("/root/GameManager/PlayerManager/@Node3D@4"))
        {
            //update position of camera and camerapivotarm to the position of the player. linear interpolation for soft follow. Delta * 5 for acceleration to player position
            this.GlobalPosition = this.GlobalPosition.Lerp(_player.GetChild<RigidBody3D>(playerId).GlobalPosition, (float)delta * 30f);
        }

    }
}