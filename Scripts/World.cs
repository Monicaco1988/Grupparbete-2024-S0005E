using Godot;
using Godot.Collections;
using System;

public partial class World : Node3D
{
	PackedScene playerScene;

	public override void _UnhandledInput(InputEvent @event){
		if(@event.IsActionPressed("startGame")){
			GetNode<Control>("MainMenu/menuBackground").Hide();
		}
	}
	public override void _Ready()
	{
		//playerScene = GD.Load<PackedScene>("res://Scenes/Car_EvenBetter.tscn");
		//var playerRoot = playerScene.Instantiate<Node3D>();
		//AddChild(playerRoot);
	}

}
