using Godot;
using Godot.Collections;
using System;
using System.Threading;

public partial class player_manager : Node3D
{
    //Adding Class to be able to listen to GameManager and change GameManager State. See GameManager Script.
    private GameManager _GetStateGameManager;

    PackedScene playerScene;
    int numberOfPlayers;

    Array<betterCar> ambulances = new Array<betterCar>();
    [Export] Array<Marker3D> Spawnpoints = new Array<Marker3D>();
    public PlayerManagerState state = PlayerManagerState.ACTIVE;

    public enum PlayerManagerState
    {
        ACTIVE,
        IN_ACTIVE
    }


    public void setPlayerManagerState(PlayerManagerState setState)
    {
        this.state = setState;
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        playerScene = GD.Load<PackedScene>("res://Scenes/Car_EvenBetter.tscn");
        GD.Print("ready");
        
        //Gets the class information from GameManager to the variable _GetStateGameManager 
        _GetStateGameManager = GetNode<GameManager>("/root/GameManager");
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

        if (this.state == PlayerManagerState.IN_ACTIVE)
        {
            return;
        }

        var controllers = Input.GetConnectedJoypads();

        foreach (var controller in controllers)
        {
            bool playerStateFlag = false;
            if (Input.IsJoyButtonPressed(controller, JoyButton.Start))
            {
                if (ambulances.Count != 0)
                {
                    foreach (var ambulance in ambulances)
                    {
                        if (ambulance.id == controller)
                        {
                            playerStateFlag = true;
                        }
                    }
                }

                if (!playerStateFlag)
                {

                    GD.Print(playerScene.ResourcePath);

                    var playerRoot = playerScene.Instantiate<Node3D>();
                    AddChild(playerRoot);
                    playerRoot.GlobalPosition = Spawnpoints[controller].GlobalPosition;
                    var player = playerRoot.GetNode("Player") as betterCar;

                    player.setId(controller);
                    player.SetState(betterCar.PlayerState.ACTIVE);
                    this.ambulances.Add(player);
                    numberOfPlayers++;
                    GD.Print("player created, with player id: " + controller);
                }

            }

            // same as pushing start with the mouse on the button but its the A-button on the x-box controller instead
<<<<<<< Updated upstream
            if (Input.IsJoyButtonPressed(controller, JoyButton.A) && numberOfPlayers >= 0)
=======
            if (Input.IsJoyButtonPressed(controller, JoyButton.A) && numberOfPlayers >= 0 && lockAButton == 0) // if you want to change amount of players necessary to start game change >= 2.
>>>>>>> Stashed changes
            {
                _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 2);


                GetNode<StaticBody3D>("/root/GameManager/PlayerManager/SpawnPlatform").QueueFree();
                GetNode<Node>("/root/GameManager/PlayerManager/Node").QueueFree();
                GetNode<Node3D>("/root/GameManager/PlayerManager/road_straightBarrier").QueueFree();
                GetNode<Node3D>("/root/GameManager/PlayerManager/road_straightBarrier2").QueueFree();
                GetNode<Camera3D>("/root/GameManager/PlayerManager/Camera3D").QueueFree();
                GetNode<Button>("/root/GameManager/PlayerManager/Button").QueueFree();
                //QueueFree();
            }

        }

    }

    //When "Start Game" is pressed GameState in GameManager will change to LevelManager and the Level Scene will get loaded
    public void OnButtonPressed()
    {
        if (numberOfPlayers >= 0) // "Start Game" only works if there are atleast 2 players
        {
            _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 2); // changes Manager State to LevelManager
            

            GetNode<StaticBody3D>("/root/GameManager/PlayerManager/SpawnPlatform").QueueFree();
            GetNode<Node>("/root/GameManager/PlayerManager/Node").QueueFree();
            GetNode<Node3D>("/root/GameManager/PlayerManager/road_straightBarrier").QueueFree();
            GetNode<Node3D>("/root/GameManager/PlayerManager/road_straightBarrier2").QueueFree();
            GetNode<Camera3D>("/root/GameManager/PlayerManager/Camera3D").QueueFree();
            GetNode<Button>("/root/GameManager/PlayerManager/Button").QueueFree();

            //QueueFree();//removes PlayerManager Scene

            
        }
    }

    private void OnTimerTimeout()
    {
        //Moved location of spawnLocation so that the cars are visible when spawned in map
        moveToSpawnLocation();
        _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 2);


        GetNode<StaticBody3D>("/root/GameManager/PlayerManager/SpawnPlatform").QueueFree();
        //GetNode<CollisionShape3D>("/root/GameManager/PlayerManager/SpawnPlatform/CollisionShape3D").QueueFree();
        //GetNode<CollisionShape3D>("/root/GameManager/PlayerManager/SpawnPlatform/CollisionShape3D6").QueueFree();
        GetNode<Node>("/root/GameManager/PlayerManager/Node").QueueFree();
        GetNode<Node3D>("/root/GameManager/PlayerManager/road_straightBarrier").QueueFree();
        GetNode<Node3D>("/root/GameManager/PlayerManager/road_straightBarrier2").QueueFree();
        GetNode<Camera3D>("/root/GameManager/PlayerManager/Camera3D").QueueFree();
        GetNode<Button>("/root/GameManager/PlayerManager/Button").QueueFree();

        //QueueFree();
        lockAButton++;
        // Add timer so start scene can run and 3 sec countdown...
    }

}

