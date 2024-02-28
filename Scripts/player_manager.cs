using Godot;
using Godot.Collections;
using System;

public partial class player_manager : Node3D
{
    private int Score = 0;
    public static player_manager instance {  get; private set; }
    private int lockAButton = 0;

    //Adding Class to be able to listen to GameManager and change GameManager State. See GameManager Script.
    private GameManager _GetStateGameManager;
    //Marker3D lastCollision;

    PackedScene playerScene;
    PackedScene player_gui;
    public int numberOfPlayers;

    public Array<betterCar> ambulances = new Array<betterCar>();
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
        instance = this;
        playerScene = GD.Load<PackedScene>("res://Scenes/Car_EvenBetter.tscn");
        GD.Print("ready");
        
        //Gets the class information from GameManager to the variable _GetStateGameManager 
        _GetStateGameManager = GetNode<GameManager>("/root/GameManager");
        //lastCollision = GetNode<Marker3D>("/root/GameManager/PlayerManager/World/CollisionaraDestroy");
        HandelPlayerGui();
    }

    private void HandelPlayerGui() 
    {
        player_gui = GD.Load<PackedScene>("res://Scenes/player_gui.tscn");
        var playerGUI = player_gui.Instantiate();
        AddChild(playerGUI);
        
    }

    public void moveToSpawnLocation()
    {
        foreach (var ambulance in ambulances)
        {
            ambulance.GlobalPosition = Spawnpoints[ambulance.id].GlobalPosition;
            ambulance.carMesh.Rotation = Vector3.Zero;
        }
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

        if (this.state == PlayerManagerState.IN_ACTIVE)
        {
            return;
        }

        var controllers = Input.GetConnectedJoypads();
        //GD.Print(controllers);

        foreach (var controller in controllers)
        {
            
            bool playerStateFlag = false;
            if (Input.IsJoyButtonPressed(controller, JoyButton.Start))
            {
                if (ambulances.Count != 0)
                {
                    //GD.Print("Bajskorv");
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

                    //GD.Print(playerScene.ResourcePath);

                    var playerRoot = playerScene.Instantiate<Node3D>();
                    AddChild(playerRoot);
                    playerRoot.GlobalPosition = Spawnpoints[controller].GlobalPosition;
                    var player = playerRoot.GetNode("Player") as betterCar;
                    //GD.Print(numberOfPlayers);
                    player.setId(controller);
                    player.SetState(betterCar.PlayerState.ACTIVE);
                    this.ambulances.Add(player);
                    //GD.Print(ambulances);
                    numberOfPlayers++;
                    GD.Print("player created, with player id: " + controller);
                }

            }

            // same as pushing start with the mouse on the button but its the A-button on the x-box controller instead
            if (Input.IsJoyButtonPressed(controller, JoyButton.A) && numberOfPlayers >= 0 && lockAButton == 0) // if you want to change amount of players necessary to start game change >= 2.
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

            //trying to add simple function to reset players if a player is hidden
            if (lockAButton > 0)
            {
                if (GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot").Visible == false || GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@2").Visible == false || GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@3").Visible == false)// || !GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@2").Visible == true)
                {
                    Score++;

                    var offset = new Vector3(0, 0, 0);
                    foreach (var ambulance in ambulances)
                    {
                        ambulance.GlobalPosition = GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(0).GlobalPosition + offset;
                        ambulance.LinearVelocity = Vector3.Zero;
                        //ambulance.Visible = true;
                        offset += new Vector3(-5,0,0);
                    }
                    Visibility();

                    GetTree().Paused = true;

                    if (Score < 4)
                    {
                        GetNode<Timer>("/root/GameManager/World/Countdown/Timer").Start();
                    }
                    //moveToSpawnLocation();
                    
                    if(Score == 4)
                    {
                        _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 5);
                    }
                }
            }
        }

    }

    public void Visibility()
    {
        //if (GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot").Visible == false)
        GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot").Visible = true;

        //if (GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@2").Visible == false)
        GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@2").Visible = true;

        GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@3").Visible = true;

    }

    //When "Start Game" is pressed GameState in GameManager will change to LevelManager and the Level Scene will get loaded
    public void OnButtonPressed()
    {
        if (numberOfPlayers >= 2) // "Start Game" only works if there are atleast 2 players
        {
            moveToSpawnLocation();
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

    public int GetPNum() 
    {
        return numberOfPlayers;
    }
}