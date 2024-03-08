using Godot;
using Godot.Collections;
using System;
using static System.Formats.Asn1.AsnWriter;

public partial class player_manager : Node3D
{
    // tracking playerscore first to 3 wins
    public int player1Score = 0;
    public int player2Score = 0;
    public int player3Score = 0;
    public int player4Score = 0;

    public string leader;
    public int Score = 0;
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
        //GD.Print(ambulances[0].GetPath());
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
            ambulance.GlobalPosition = Spawnpoints[ambulance.controllerId].GlobalPosition;
            ambulance.carMesh.Rotation = Vector3.Zero;
            ambulance.LinearVelocity = Vector3.Zero;

            ambulance.pwrUpDefib = 1;
            ambulance.pwrUpSpeed = 1;
            ambulance.pwrUpSwitch = 1;
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
                        if (ambulance.controllerId == controller)
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
                    player.setControllerId(controller);
                    
                    player.SetState(betterCar.PlayerState.ACTIVE);
                    this.ambulances.Add(player);
                    player.setId(ambulances.IndexOf(player) + 1);
                    //GD.Print(ambulances);
                    numberOfPlayers++;
                    GD.Print("player created, with player id: " + controller);
                }

            }

            // same as pushing start with the mouse on the button but its the A-button on the x-box controller instead
            if (Input.IsJoyButtonPressed(controller, JoyButton.Y) && numberOfPlayers >= 1 && lockAButton == 0) // if you want to change amount of players necessary to start game change >= 2.
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
                    var player_1 = (ambulances[0].GetParent() as Node3D);
                    var player_2 = (ambulances[1].GetParent() as Node3D);

                    if (player_1.Visible == false || player_2.Visible == false)
                    {
                        //Score++;
                        if (player_1.Visible == true) player1Score++;
                        else if (player_2.Visible == true) player2Score++;
                        else { }

                        if (player1Score == 3 || player2Score == 3)//Score == 3)// player2Score == 3 || player1Score == 3 || GetNode<CollisionShape3D>("/root/GameManager/PlayerManager/World/CollisionaraDestroy/CollisionShape3D237") == null) // next fix is score system (added the logic if a player get to last node it triggers winning screen)
                        {
                            if (player1Score == 3)
                                leader = "Player 1";
                            else if (player2Score == 3)
                                leader = "Player 2";

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
                            ambulance.carMesh.LookAt(GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(1).GlobalPosition);
                            offset += new Vector3(-5, 0, 0);

                            ambulance.pwrUpDefib = 1;
                            ambulance.pwrUpSpeed = 1;
                            ambulance.pwrUpSwitch = 1;
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
                    var player_1 = (ambulances[0].GetParent() as Node3D);
                    var player_2 = (ambulances[1].GetParent() as Node3D);
                    var player_3 = (ambulances[2].GetParent() as Node3D);

                    if ((player_1.Visible == false && player_2.Visible == false) || (player_3.Visible == false && player_2.Visible == false) || (player_1.Visible == false && player_3.Visible == false) ) // this works
                    {
                        if (player_1.Visible == true) player1Score++;
                        else if (player_2.Visible == true) player2Score++;
                        else if (player_3.Visible == true) player3Score++;
                        else { }
                        
                        if (player1Score == 3 || player2Score == 3 || player3Score == 3)// || GetNode<CollisionShape3D>("/root/GameManager/PlayerManager/World/CollisionaraDestroy/CollisionShape3D237") == null)
                        {
                            if (player1Score == 3)
                                leader = "Player 1";
                            else if (player2Score == 3)
                                leader = "Player 2";
                            else if (player3Score == 3)
                                leader = "Player 3";

                            GetNode<Node3D>("/root/GameManager/World").QueueFree();

                            this.QueueFree();
                            _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 5);
                        }
                        var offset = new Vector3(7.5f, 0, 0);

                        foreach (var ambulance in ambulances)
                        {
                            ambulance.GlobalPosition = GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(0).GlobalPosition + offset;
                            ambulance.LinearVelocity = Vector3.Zero;
                            ambulance.LookAt(GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(1).GlobalPosition);
                            //ambulance.Visible = true;
                            offset += new Vector3(-5, 0, 0);

                            ambulance.pwrUpDefib = 1;
                            ambulance.pwrUpSpeed = 1;
                            ambulance.pwrUpSwitch = 1;
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
                    var player_1 = (ambulances[0].GetParent() as Node3D);
                    var player_2 = (ambulances[1].GetParent() as Node3D);
                    var player_3 = (ambulances[2].GetParent() as Node3D);
                    var player_4 = (ambulances[3].GetParent() as Node3D);

                    if ((player_1.Visible == false && player_2.Visible == false && player_3.Visible == false ) ||
                        (player_1.Visible == false && player_2.Visible == false && player_4.Visible == false) || 
                        (player_4.Visible == false && player_2.Visible == false && player_3.Visible == false) || 
                        (player_1.Visible == false && player_4.Visible == false && player_3.Visible == false))
                    {
                        if (player_1.Visible == true) player1Score++;
                        else if (player_2.Visible == true) player2Score++;
                        else if (player_3.Visible == true) player3Score++;
                        else if (player_4.Visible == true) player4Score++;
                        else { }

                        if (player1Score == 3 || player2Score == 3 || player3Score == 3 || player4Score == 3)// || GetNode<CollisionShape3D>("/root/GameManager/PlayerManager/World/CollisionaraDestroy/CollisionShape3D237") == null)
                        {
                            if (player1Score == 3)
                                leader = "Player 1";
                            else if (player2Score == 3)
                                leader = "Player 2";
                            else if (player3Score == 3)
                                leader = "Player 3";
                            else if (player4Score == 3)
                                leader = "Player 4";


                            GetNode<Node3D>("/root/GameManager/World").QueueFree();

                            this.QueueFree();
                            _GetStateGameManager.EmitSignal(nameof(_GetStateGameManager.UpdateGameState2), 5);
                        }
                        var offset = new Vector3(7.5f, 0, 0);

                        foreach (var ambulance in ambulances)
                        {
                            ambulance.GlobalPosition = GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(0).GlobalPosition + offset;
                            ambulance.LinearVelocity = Vector3.Zero;
                            ambulance.LookAt(GetNode<Area3D>("/root/GameManager/World/CollisionaraDestroy").GetChild<CollisionShape3D>(1).GlobalPosition);
                            //ambulance.Visible = true;
                            offset += new Vector3(-5, 0, 0);

                            ambulance.pwrUpDefib = 1;
                            ambulance.pwrUpSpeed = 1;
                            ambulance.pwrUpSwitch = 1;
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

        foreach (var ambulance in ambulances)
        {
            if(ambulance.pwrUpDefib != 1)
            {
                
            }
            if(ambulance.pwrUpSpeed != 1)
            {

            }
            if(ambulance.pwrUpSwitch != 1)
           {

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
        foreach (var player in ambulances)
        {
            (player as betterCar).SetState(betterCar.PlayerState.ACTIVE);
        }
    }


    public int GetPNum() 
    {
        return numberOfPlayers;
    }
}