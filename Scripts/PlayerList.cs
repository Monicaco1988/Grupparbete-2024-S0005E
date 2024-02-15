using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerList : Godot.Node
{
    //placeholder for list of how many players there will spawn in the different levels
    // this info comes from playermanager

    public static List<playerinfo> Players = new List<playerinfo>();

    public override void _Ready()
    {
        //debugging
        GD.Print(Players);
    }

}
