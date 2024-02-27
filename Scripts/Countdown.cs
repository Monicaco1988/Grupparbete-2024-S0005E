using Godot;
using System;

public partial class Countdown : Control
{
    [Export]
    Timer timeToStart;

    private int scene = 0;

    public override void _Ready()
    {
        this.GetChild<Label>(scene).Show();
        timeToStart.Start();
    }



    public void OnTimerTimeout()
    {

        scene++;
        GD.Print(scene);
        this.GetChild<Label>(scene-1).Hide();
        if (scene == 4)
        {
            scene = 0;
            GetTree().Paused = false;
            timeToStart.Stop();
        }
        else {
            
            this.GetChild<Label>(scene).Show();
            timeToStart.Start(); 
        }
        //if (scene < 4)
        //{
        //    this.GetChild<Label>(scene).Show();
        //}

    }
}
