using Godot;
using System;

public partial class defibrillator : Node3D
{
    Timer defibTimer;
    betterCar player;
    bool triggered = false;
    AnimationPlayer aniPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        defibTimer = GetNode<Timer>("Timer");
        aniPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        aniPlayer.Play("Throw");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void BodyEntered(Node3D Player)
    {
        if ((Player as betterCar) == null)
        {
            return;
        }
        if (triggered)
        {
            return;
        }
        triggered = true;
        this.Visible = false;
        GD.Print((Player as betterCar));

        this.player = (Player as betterCar);

        player.SetState(betterCar.PlayerState.IN_ACTIVE);
        defibTimer.Start();

    }

    public void defibTimeOut()
    {
        GD.Print("timed out");
        this.player.SetState(betterCar.PlayerState.ACTIVE);
        QueueFree();
    }
}