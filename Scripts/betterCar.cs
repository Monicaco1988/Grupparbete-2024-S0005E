using Godot;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

public partial class betterCar : RigidBody3D
{

	public int id = -1;
	//public betterCar(int id)
	//{
	//    this.id = id;
	//}

	public void setId(int newId)
	{
		this.id = newId;
	}


	public enum PlayerState
	{
		ACTIVE,
		IN_ACTIVE
	}

    public enum PowerUp
    {
        SPEED_BOOST,
        DEFIBRILLATOR,
		P3,
		P4,
		P5,
		P6,
		P7,
		P8,
		NO_POWER
    }

    public Node3D carMesh;
	Node3D bodyMesh;
	RayCast3D groundRay;
	MeshInstance3D rightWheel;
	MeshInstance3D leftWheel;
	Vector3 sphereOffset = Vector3.Down;
	PlayerState state = PlayerState.IN_ACTIVE;
	PowerUp powerUp = PowerUp.DEFIBRILLATOR;
	PackedScene trackScene;
	GpuParticles3D smoke;
	Node3D rightSkid;
	Node3D leftSkid;
	Node3D dropPoint;
	
	//Node3D playerRoot;

	OmniLight3D lights;
	AnimationPlayer doorAnimation;

	//POWER UPS
	PackedScene defib;

	float acceleration = 3000;
	float velocity;
	float steering = 24.0f;
	float turnSpeed = 6.0f;
	float turnStopLimit = 0.75f;
	float speedInput = 0;
	float turnInput = 0;
	float bodyTilt = 35;
	float gasInput;
	float turnInput2;
	float speedBoost = 25;
	bool isDrifting = false;
	int onoff = 1;
   

	public PlayerState GetState(PlayerState state)
	{
		PlayerState returnState = PlayerState.IN_ACTIVE;
		switch (state)
		{
			case PlayerState.ACTIVE:
				returnState = PlayerState.ACTIVE;
				break;

			case PlayerState.IN_ACTIVE:
				returnState = PlayerState.IN_ACTIVE;
				break;
		}
		return returnState;
	}

	public void SetState(PlayerState newState)
	{
		this.state = newState;    
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		
        carMesh = GetParent().GetNode<Node3D>("CarMesh");
        bodyMesh = GetParent().GetNode<Node3D>("CarMesh/ambulance");
        groundRay = GetParent().GetNode<RayCast3D>("CarMesh/RayCast3D");
        rightWheel = GetParent().GetNode<MeshInstance3D>("CarMesh/ambulance/wheel_frontRight");
		leftWheel = GetParent().GetNode<MeshInstance3D>("CarMesh/ambulance/wheel_frontLeft");
		rightSkid = GetParent().GetNode<Node3D>("CarMesh/ambulance/wheel_backRight/SkidRight");
        leftSkid = GetParent().GetNode<Node3D>("CarMesh/ambulance/wheel_backLeft/SkidLeft");
		dropPoint = GetParent().GetNode<Node3D>("CarMesh/ambulance/DropPoint");

        trackScene = GD.Load<PackedScene>("res://Scenes/track_decal.tscn");
		smoke = GetParent().GetNode<GpuParticles3D>("CarMesh/ambulance/Smoke/smokeParticle");

        lights = GetParent().GetNode<OmniLight3D>("CarMesh/ambulance/body/OmniLight3D");
		doorAnimation = GetParent().GetNode<AnimationPlayer>("DoorAnimationPlayer");

		defib = GD.Load<PackedScene>("res://Scenes/Power_Ups/defibrillator.tscn");
        //pU = 
        //var smokeP = smoke.GetNode<GpuParticles3D>("SmokeParticle");
        //PlayerManager / PlayerRoot / CarMesh / ambulance / Smoke / smokeParticle
    }

	

	public void addTireTracks()
	{
		if (Mathf.Abs(turnInput2) > 0.3f)
		{
			
			var trackS = trackScene.Instantiate();
			GetParent().GetParent().AddChild(trackS);

			var trackL = trackS.GetNode<Decal>("DecalLeft");
			var trackR = trackS.GetNode<Decal>("DecalRight");
			trackL.GlobalPosition = leftSkid.GlobalPosition;
			trackR.GlobalPosition = rightSkid.GlobalPosition;
			//GD.Print(track.GlobalPosition);
			var rL = leftSkid.Rotation;
			var rR = rightSkid.Rotation;
			trackR.Rotation = rR;
			trackL.Rotation = rL;
		}
	}

	public void playDoorAnimation()
	{
        doorAnimation.Play("DoorAnimation");
    }

	public void usePowerUp(double delta)
	{
        if (Input.IsJoyButtonPressed(id, JoyButton.X))
        {
			switch (powerUp)
			{
				case PowerUp.SPEED_BOOST:
<<<<<<< Updated upstream
					ApplyCentralImpulse(-carMesh.GlobalBasis.Z * speedBoost * (float)delta);
					playDoorAnimation();
=======
					if (pwrUpSpeed == 1)
						{
						ApplyCentralImpulse(-carMesh.GlobalBasis.Z * speedBoost * 0.5f);// * (float)delta);
						//playDoorAnimation();
						//bodyMesh.Rotation = new Vector3(0.2f, 0, 0);
                        fireSpeedRight.Emitting = true;
                        fireSpeedLeft.Emitting = true;
                        //smoke.Emitting = true;

						pwrUpSpeed--;
						}
					//powerUp = PowerUp.DEFIBRILLATOR;
>>>>>>> Stashed changes
					break;

				case PowerUp.DEFIBRILLATOR:
					var d = defib.Instantiate();
					GetParent().GetParent().AddChild(d);

					playDoorAnimation();
					var obj = d.GetNode<Node3D>("Pivot");
					obj.GlobalPosition = dropPoint.GlobalPosition;
					//obj.ApplyCentralImpulse(carMesh.GlobalBasis.Z * 2 * (float)delta);
					break;

                case PowerUp.P3:
                    //Do P3 shit
                    break;

                case PowerUp.P4:
                    //Do P4 shit
                    break;

                case PowerUp.P5:
                    //Do P5 shit
                    break;

                case PowerUp.P6:
                    //Do P6 shit
                    break;

                case PowerUp.P7:
                    //Do P7 shit
                    break;

                case PowerUp.P8:
                    //Do P8 shit
                    break;

                default: break;
            }
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		carMesh.Position = Position + sphereOffset;
		if (groundRay.IsColliding())//
		{
			//if (Mathf.Abs(this.LinearVelocity.Length()) < 4)
			//{
			//    ApplyCentralForce(-carMesh.GlobalBasis.Z * gasInput);
			//}

			ApplyCentralForce(-carMesh.GlobalBasis.Z * gasInput * (float)delta);
		}
		//GD.Print(carMesh.Position);

	}

	public override void _Process(double delta)
	{
        if (state == PlayerState.IN_ACTIVE)
        {
			return;
        }
        usePowerUp(delta);
		//GD.Print(Mathf.Abs(this.LinearVelocity.Length()));
		velocity = this.LinearVelocity.Length();

		if (!groundRay.IsColliding()) //
		{
			return;
		}
		gasInput = (Input.GetJoyAxis(id, JoyAxis.TriggerRight) - Input.GetJoyAxis(id, JoyAxis.TriggerLeft)) * acceleration;
		//turnInput = Input.GetAxis("steerRight", "steerLeft") * Mathf.DegToRad(steering);
		turnInput2 = -Input.GetJoyAxis(id, JoyAxis.LeftX) * Mathf.DegToRad(steering);
		isDrifting = Input.IsJoyButtonPressed(id, JoyButton.LeftShoulder);

		rightWheel.Rotation = new Vector3(0, turnInput2, Mathf.DegToRad(-180));
		leftWheel.Rotation = new Vector3(0, turnInput2, 0);


		if (LinearVelocity.Length() > turnStopLimit)
		{
			var newBasis = carMesh.GlobalBasis.Rotated(carMesh.GlobalBasis.Y, turnInput2);
			carMesh.GlobalBasis = carMesh.GlobalBasis.Slerp(newBasis, (float)(turnSpeed * delta));
			carMesh.GlobalTransform = carMesh.GlobalTransform.Orthonormalized();

			if (isDrifting)
			{
				steering = 30.0f;
				addTireTracks();
                smoke.Emitting = true;
            }
			else
			{
				steering = 18.0f;
                smoke.Emitting = false;
                isDrifting = false;
			}

			var tilted = -turnInput2 * LinearVelocity.Length() / bodyTilt;
			bodyMesh.Rotation = new Vector3(0, 0, tilted);



			if (groundRay.IsColliding())
			{
				var normal = groundRay.GetCollisionNormal();
				var xForm = AlignWithY(carMesh.GlobalTransform, normal);
				carMesh.GlobalTransform = carMesh.GlobalTransform.InterpolateWith(xForm, (float)(10 * delta));
			}
		}
		TurnLightsOnOff();
    }

	public void TurnLightsOnOff()
	{
		////switches lights on/offInput.IsJoyButtonPressed(id, JoyButton.X)
		//if (Input.IsJoyButtonPressed(id, JoyButton.RightShoulder) && lights.Visible == true && onoff == 1)
		if(Input.IsJoyButtonPressed(id, JoyButton.RightShoulder) && lights.Visible == true && onoff == 1)
		{
			lights.Visible = false;
			onoff--;
		}

		//else if (Input.IsJoyButtonPressed(id, JoyButton.RightShoulder) && lights.Visible == false && onoff == 0)
		else if (Input.IsJoyButtonPressed(id, JoyButton.RightShoulder) && lights.Visible == false && onoff == 0)

        {
			lights.Visible = true;
			onoff++;
		}
	}

	public Transform3D AlignWithY(Transform3D xForm, Vector3 newY)
	{
		xForm.Basis.Y = newY;
		xForm.Basis.X = -xForm.Basis.Z.Cross(newY);
		xForm.Basis = xForm.Basis.Orthonormalized();

		return xForm.Orthonormalized();
	}
}
