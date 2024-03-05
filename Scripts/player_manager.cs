using Godot;
using Godot.Collections;
using System;
using static System.Formats.Asn1.AsnWriter;

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
            ambulance.LinearVelocity = Vector3.Zero;
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
            if (Input.IsJoyButtonPressed(controller, JoyButton.Y) && numberOfPlayers >= 2 && lockAButton == 0) // if you want to change amount of players necessary to start game change >= 2.
            {
                //Moved location of spawnLocation so that the cars are visible when spawned in map
                moveToSpawnLocation();
                _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 2);


                GetNode<StaticBody3D>("/root/GameManager/PlayerManager/SpawnPlatform").QueueFree();
                GetNode<Node>("/root/GameManager/PlayerManager/Node").QueueFree();

                GetNode<Camera3D>("/root/GameManager/PlayerManager/Camera3D").QueueFree();
                GetNode<Control>("/root/GameManager/PlayerManager/Control").QueueFree();

                //QueueFree();
                lockAButton++;
                // Add timer so start scene can run and 3 sec countdown...
            }

            //trying to add simple function to reset players if a player is hidden
            if (lockAButton > 0)
            {
                if (numberOfPlayers == 2)// if there are 2 player
                {
                    var player_1 = GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot");
                    var player_2 = GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(9);

                    if (player_1.Visible == false || player_2.Visible == false)
                    {
                        Score++;
                        if (Score == 3) // next fix is score system
                        {


                            GetNode<Node3D>("/root/GameManager/World").QueueFree();

                            this.QueueFree();
                            _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 5);
                        }
                        var offset = new Vector3(10, 0, 0);

                        foreach (var ambulance in ambulances)
                        {
                            ambulance.GlobalPosition = GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(0).GlobalPosition + offset;
                            ambulance.LinearVelocity = Vector3.Zero;
             //ambulance.carMesh.LookAtFromPosition(GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(0).GlobalPosition, GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(1).GlobalPosition); // this works fine
                            ambulance.carMesh.LookAt(GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(2).GlobalPosition);
                            offset += new Vector3(-5, 0, 0);
                        }
                        Visibility();

                        GetTree().Paused = true;

                        if (Score < 3)
                        {
                            GetNode<Control>("/root/GameManager/World/Countdown").GetChild<Label>(0).Show();
                            GetNode<Timer>("/root/GameManager/World/Countdown/Timer").Start();
                        }
                    }
                }
                else if (numberOfPlayers == 3) // if there are 3 player
                {
                    var player_1 = GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot");
                    var player_2 = GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(9);
                    var player_3 = GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(10);

                    if ((player_1.Visible == false && player_2.Visible == false) || (player_3.Visible == false && player_2.Visible == false) || (player_1.Visible == false && player_3.Visible == false) ) // this works
                    {
                        Score++;
                        if (Score == 3)
                        {
                            GetNode<Node3D>("/root/GameManager/World").QueueFree();

                            this.QueueFree();
                            _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 5);
                        }
                        var offset = new Vector3(7.5f, 0, 0);

                        foreach (var ambulance in ambulances)
                        {
                            ambulance.GlobalPosition = GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(0).GlobalPosition + offset;
                            ambulance.LinearVelocity = Vector3.Zero;
                            ambulance.LookAt(GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(2).GlobalPosition);
                            //ambulance.Visible = true;
                            offset += new Vector3(-5, 0, 0);
                        }
                        Visibility();

                        GetTree().Paused = true;

                        if (Score < 3)
                        {
                            GetNode<Control>("/root/GameManager/World/Countdown").GetChild<Label>(0).Show();
                            GetNode<Timer>("/root/GameManager/World/Countdown/Timer").Start();
                        }
                    }
                }
                else if (numberOfPlayers == 4) // if there are 4 player
                {
                    var player_1 = GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot");
                    var player_2 = GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(9);
                    var player_3 = GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(10);
                    var player_4 = GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(11);

                    if ((player_1.Visible == false && player_2.Visible == false && player_3.Visible == false ) || (player_1.Visible == false && player_2.Visible == false && player_4.Visible == false) || (player_4.Visible == false && player_2.Visible == false && player_3.Visible == false) || (player_1.Visible == false && player_4.Visible == false && player_3.Visible == false))
                    {
                        Score++;
                        if (Score == 3)
                        {
                            GetNode<Node3D>("/root/GameManager/World").QueueFree();

                            this.QueueFree();
                            _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 5);
                        }
                        var offset = new Vector3(7.5f, 0, 0);

                        foreach (var ambulance in ambulances)
                        {
                            ambulance.GlobalPosition = GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(0).GlobalPosition + offset;
                            ambulance.LinearVelocity = Vector3.Zero;
                            ambulance.LookAt(GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(2).GlobalPosition);
                            //ambulance.Visible = true;
                            offset += new Vector3(-5, 0, 0);
                        }
                        Visibility();

                        GetTree().Paused = true;

                        if (Score < 3)
                        {
                            GetNode<Control>("/root/GameManager/World/Countdown").GetChild<Label>(0).Show();
                            GetNode<Timer>("/root/GameManager/World/Countdown/Timer").Start();
                        }
                    }
                }
            }
        }
    }

    public void Visibility()
    {
        //if (GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot").Visible == false)
        GetNode<Node3D>("/root/GameManager/PlayerManager/PlayerRoot").Visible = true;
        GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(9).Visible = true;

        if (numberOfPlayers > 2)
        {
            GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(10).Visible = true;
        }
        if (numberOfPlayers > 3)
        {
            GetNode<Node3D>("/root/GameManager/PlayerManager").GetChild<Node3D>(11).Visible = true;
        }
        //if (GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@2").Visible == false)
        //GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@3").Visible = true;
        //GetNode<Node3D>("/root/GameManager/PlayerManager/@Node3D@4").Visible = true;
    }


    public int GetPNum() 
    {
        return numberOfPlayers;
    }
}