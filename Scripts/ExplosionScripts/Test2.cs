using Godot;
using System;

public partial class Test2 : Marker3D
{
    //constants for identifying the different objects
    private int playerId = 0;

    private Node3D _player;

    private RigidBody3D _body;

    //made to check if "has a" logic for children of node Playermanager, checks if there are more than 1 player, if so then it will be activated
    private Node3D _VirtualPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //instantiates the attributes and objects from playermanager
        _VirtualPlayer = GetNode<Node3D>("/root/GameManager/PlayerManager");
        // check if node exists
        if (_VirtualPlayer.GetChildCount() > 7)
        {
            _player = GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(8);
            _body = _player.GetChild<RigidBody3D>(0);
            this.GlobalPosition = _body.GlobalPosition;
        }
        //GD.Print("Test1");
        //GD.Print(_player.Name);

        //GD.Print("Test2");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (_VirtualPlayer.GetChildCount() > 7)
        {
            //update position of camera and camerapivotarm to the position of the player. linear interpolation for soft follow. Delta * 5 for acceleration to player position
            this.GlobalPosition = this.GlobalPosition.Lerp(_body.GlobalPosition, (float)delta * 30f);
        }

    }
}
