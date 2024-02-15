using Godot;
using System;
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

	Node3D carMesh;
	Node3D bodyMesh;
	RayCast3D groundRay;
	MeshInstance3D rightWheel;
	MeshInstance3D leftWheel;
	Vector3 sphereOffset = Vector3.Down;
	PlayerState state = PlayerState.IN_ACTIVE;
	PackedScene trackScene;
	Node3D rightSkid;
	Node3D leftSkid;



	float acceleration = 2500;
	float velocity;
	float steering = 18.0f;
	float turnSpeed = 4.0f;
	float turnStopLimit = 0.75f;
	float speedInput = 0;
	float turnInput = 0;
	float bodyTilt = 35;
	float gasInput;
	float turnInput2;
	bool isDrifting = false;

   

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
        trackScene = GD.Load<PackedScene>("res://Scenes/track_decal.tscn");
		
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
			}
			else
			{
				steering = 18.0f;
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







	}

	public Transform3D AlignWithY(Transform3D xForm, Vector3 newY)
	{
		xForm.Basis.Y = newY;
		xForm.Basis.X = -xForm.Basis.Z.Cross(newY);
		xForm.Basis = xForm.Basis.Orthonormalized();

		return xForm.Orthonormalized();
	}
}
