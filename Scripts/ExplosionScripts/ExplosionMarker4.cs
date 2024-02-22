using Godot;
using System;

public partial class ExplosionMarker4 : Marker3D
{
    [Export]
    CpuParticles3D explosion;


    public Node3D _player;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = GetNode<Node3D>("/root/GameManager/PlayerManager");

        //instantiates the attributes and objects from playermanager
        // also check if node exists
        if (_player.HasNode("/root/GameManager/PlayerManager/@Node3D@4"))
        {
            _player = GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@4");
            this.GlobalPosition = _player.GetChild<RigidBody3D>(0).GlobalPosition;
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_player.HasNode("/root/GameManager/PlayerManager/@Node3D@4"))
        {
            //update position of camera and camerapivotarm to the position of the player. linear interpolation for soft follow. Delta * 5 for acceleration to player position
            this.GlobalPosition = this.GlobalPosition.Lerp(_player.GetChild<RigidBody3D>(0).GlobalPosition, (float)delta * 50f);
        }

    }

    public void OnScreenExist()
    {
        if (_player.HasNode("/root/GameManager/PlayerManager/@Node3D@4"))
        {
            explosion.Emitting = true;
            GD.Print("Player exited screen");
            _player.GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@4").Hide();
        }
    }
}
