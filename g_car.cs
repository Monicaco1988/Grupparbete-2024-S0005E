using Godot;
using System;

public partial class g_car : VehicleBody3D
{
	[Export] public const float MAX_STEER = 0.8f;
    public int id = 0;
	public int force = 10000;
	public int speedButton = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		//Steering = Input.GetJoyAxis(JoyAxis.LeftX, JoyAxis.RightX);

        var direction = Vector3.Zero;

		if (Input.GetJoyAxis(id, JoyAxis.LeftX) > 0.3f || Input.GetJoyAxis(id, JoyAxis.LeftX) < -0.3f)
		{
			Steering = -Input.GetJoyAxis(id, JoyAxis.LeftX)*((float)Math.PI)/6;
		}
		else
		{
            Steering = 0;
        }

		if(Input.IsJoyButtonPressed(id, 0) && EngineForce < 100000)
		{
            EngineForce -= 3;
        }
		else if(EngineForce > 0)
		{
            EngineForce += 0.5f;
        }

        if (Input.IsJoyButtonPressed(id, JoyButton.B) && EngineForce < 100000)
        {
            EngineForce -= 3;
        }
        else if (EngineForce > 0)
        {
            EngineForce += 0.5f;
        }

        GD.Print(EngineForce);
        
    }
}
