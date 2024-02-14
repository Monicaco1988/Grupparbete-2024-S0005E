using Godot;
using System;

public partial class World : Node3D
{
	public override void _UnhandledInput(InputEvent @event){
		if(@event.IsActionPressed("startGame")){
			GetNode<Control>("MainMenu/menuBackground").Hide();
		}
	}
	
	

}
