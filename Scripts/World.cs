using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class World : Node3D
{

    //[Export]
    //private PackedScene playerScene;


    public override void _Ready()
    {

        var scene = ResourceLoader.Load<PackedScene>("res://Scenes/Car.tscn").Instantiate();
        AddChild(scene);

        // instantiating all the players from "player_manager" list. See player:manager.cs
        //foreach (var item in player_manager.players)
        //{

            //RigidBody3D currentPlayer = playerScene.Instantiate<RigidBody3D>();
            //AddChild(currentPlayer);


            // this script is used to spawn players in different locations on the map
            //foreach (Node3D spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoints"))
            //{
            //    if (int.Parse(spawnPoint.Name) == index)
            //    {
            //        currentPlayer.GlobalPosition = spawnPoint.GlobalPosition;
            //        //currentCamera.GlobalPosition = spawnPoint.GlobalPosition;
            //    }

            //}
            //index++;

        //}


    }
}
