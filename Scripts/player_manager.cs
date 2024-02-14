using Godot;
using System;
using System.Collections.Generic;

public partial class player_manager : Node
{
    // making a list (off dyn all array)of how many players should be instantiated in the level, this is because i remove the playermanager scene later and wont need it but i need the
    //program to remember how many players to instantiate to the level, it would be good to bring along the ID of the player but for this verison i will skip that.
    public static List<player_info> players = new List<player_info>();
    // i put this in autoload not sure why tho, probably so it can find the player_info class?



  


}
