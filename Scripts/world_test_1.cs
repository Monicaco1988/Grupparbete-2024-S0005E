using Godot;
using System;
using System.Collections.Generic;

public partial class world_test_1 : Node3D
{
    //instantiate a placeholder for PackedScenes
    PackedScene PlayerScene;

    public override void _Ready()
    {
          RigidBody3D currentPlayer = PlayerScene.Instantiate<RigidBody3D>(); 
          AddChild(currentPlayer);
    }




    //    // giving the PackedScene - Car_Better scene to the placeholder playerScene

    //PlayerScene = GD.Load<PackedScene>("res://Scenes/Car_EvenBetter.tscn");
    //    var playerRoot = PlayerScene.Instantiate<Node3D>();
    //    //instantiating the scene as a subclass to this class, calls the scene.
    //    AddChild(playerRoot);


}

