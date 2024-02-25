using Godot;
using System;

public partial class Countdown : Control
{
    Control Node1;

    int scene = 0;

    public override void _Ready()
    {
        this.GetChild<TextEdit>(scene).Show();
        Node1 = GetNode<Control>("/root/GameManager/World/Countdown");//.QueueFree();
    }

    public void OnTimerTimeout()
    {
        this.GetChild<TextEdit>(scene).Hide();
        scene++;
        GD.Print(scene);
        if (scene == 4)
        {
            GetTree().Paused = false;

            Node1.QueueFree();
        }
        if (scene < 4)
        {
            this.GetChild<TextEdit>(scene).Show();
        }
    }
}