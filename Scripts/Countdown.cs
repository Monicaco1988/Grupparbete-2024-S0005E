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
            
            GetTree().Paused = false;
            this.GetChild<Label>(scene).Show();
            timeToStart.Start();
        }
        else if (scene == 5)
        {
            timeToStart.Stop();
            scene = 0;
        }
        else {
            
            this.GetChild<Label>(scene).Show();
            timeToStart.Start(); 
        }

       

    }
}
