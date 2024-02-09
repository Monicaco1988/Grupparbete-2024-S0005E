using Godot;
using System;
using static System.Formats.Asn1.AsnWriter;

public partial class MainScene : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _UnhandledInput(InputEvent @event)
    {
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
			{
				GetTree().Quit();
			}
		}
    }

    public void _on_start_button_down()
	{
		GD.Print("Hello there. starting World scene");

        var scene = ResourceLoader.Load<PackedScene>("res://Scenes/World.tscn").Instantiate<Node3D>(); 
        GetTree().Root.AddChild(scene);
        this.Hide();

    }

	public void _on_button_2_button_down()
	{
		GD.Print("Quit Game");
		GetTree().Quit();

	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
