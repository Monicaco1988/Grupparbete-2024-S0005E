using Godot;
using System;

public partial class ExplosionMarker2 : Marker3D
{
    [Export]
    CpuParticles3D explosion;

    //constants for identifying the different objects
    //private int playerId = 0;

    
    /// <summary>
    // Stores the llokation of the children of playermanager eg. the node of the player 2.
    public Node3D _player;

    public RigidBody3D _body;

    //made to check if "has a" logic for children of node Playermanager, checks if there are more than 1 player, if so then it will be activated
    public Node3D _VirtualPlayer;

    // Called when the node enters the scene tree for the first time.


    public override void _Ready()
    {
        //instantiates the attributes and objects from playermanager
        _VirtualPlayer = GetNode<Node3D>("/root/GameManager/PlayerManager");
        // check if node exists
        GD.Print(_VirtualPlayer.GetChildCount());
        if (_VirtualPlayer.GetChildCount() > 8)
        {
            _player = GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(9);
            _body = _player.GetChild<RigidBody3D>(0);
            this.GlobalPosition = _body.GlobalPosition;
        }
        GD.Print("Test1");
        GD.Print(_player.Name);

        GD.Print("Test2");
    }

    //public override void _Ready()
    //{
    //    _VirtualPlayer = GetNode<Node3D>("/root/GameManager/PlayerManager");

    //    //instantiates the attributes and objects from playermanager
    //    // also check if node exists
    //    //GD.Print("there are " + _player.GetChildCount() + " number of children in playermanager");
    //    if (_VirtualPlayer.GetChildCount() > 9)
    //    {
    //        _player = GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(9);
    //        //_body = _player.GetChild<RigidBody3D>(0);
    //        this.GlobalPosition = _player.GlobalPosition; <------- this seemed to be the error"!"!!!
    //    }
    //    GD.Print("Test1");
    //    GD.Print(_player.Name);

    //    GD.Print("Test2");
    //}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (_VirtualPlayer.GetChildCount() > 8)//(_player.HasNode("/root/GameManager/PlayerManager/@Node3D@2")) old logic works but not if we respawn
        {
            //update position of camera and camerapivotarm to the position of the player. linear interpolation for soft follow. Delta * 5 for acceleration to player position
            this.GlobalPosition = this.GlobalPosition.Lerp(_body.GlobalPosition, (float)delta * 50f);
        }

    }

    public void OnScreenExist()
    {
        if (_VirtualPlayer.GetChildCount() > 8)//(_player.HasNode("/root/GameManager/PlayerManager/@Node3D@2"))
        {
            explosion.Emitting = true;
            GD.Print("Player exited screen");
            _player.Hide();
        }
    }
}
