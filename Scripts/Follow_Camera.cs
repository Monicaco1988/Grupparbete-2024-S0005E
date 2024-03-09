using Godot;
using System;

public partial class Follow_Camera : Marker3D
{
	public static Follow_Camera instance { get; private set; }

    int timer = 0;
	
	//constants for identifying the different objects
	static private int playerId = 0;
	//static private int collisionshapes = 0;

	public Node3D _player;
    
    public Node3D playerContainer1;

	// used for collisioncheck with checkpoints
	[Export]
	public Area3D areaNodeToDequeue;

    //Adding Class to be able to listen to GameManager and change GameManager State. See GameManager Script.
    private GameManager _GetStateGameManager;

    public override void _Ready()
	{
		instance = this;
        
        //instantiates the attributes and objects from playermanager
        _player = GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot");
		this.GlobalPosition = _player.GetChild<RigidBody3D>(playerId).GlobalPosition;
		GD.Print(GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChildCount() + "amount of nodes left in the world");
        _GetStateGameManager = GetNode<GameManager>("/root/GameManager");
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
        if (playerContainer1 == null)
        {
			return;
        }
        GD.Print(this);
        if (timer > 3)
        {
            (playerContainer1 as betterCar).setFirst(true);
        foreach (var ambulance in player_manager.instance.ambulances)
        {
			if (ambulance != (playerContainer1 as betterCar))
			{
				ambulance.setFirst(false);
			}
        }
        //GD.Print(timer);
  //      if (timer > 3)
		//{

		// if a player gets to the hospital this code will run ending the gamesession
		if(GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChildCount() == 0)
			{
                GetNode<Node3D>("/root/GameManager/World").QueueFree();

                this.QueueFree();
                _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 5);
            }

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
