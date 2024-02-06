using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class betterCar : RigidBody3D
{
    //[Export]
    //betterCar.Position;


    Node3D carMesh;
    Node3D bodyMesh;
    RayCast3D groundRay;
    MeshInstance3D rightWheel;
    MeshInstance3D leftWheel;
    Vector3 sphereOffset = Vector3.Down;


    float acceleration = 50;
    float steering = 18.0f;
    float turnSpeed = 4.0f;
    float turnStopLimit = 0.75f;
    float speedInput = 0;
    float turnInput = 0;
    float bodyTilt = 35;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        carMesh = GetNode<Node3D>("CarMesh");
        bodyMesh =GetNode<Node3D>("CarMesh/ambulance");
        groundRay = GetNode<RayCast3D>("CarMesh/RayCast3D");
        rightWheel = GetNode<MeshInstance3D>("CarMesh/ambulance/wheel_frontRight");
        leftWheel = GetNode<MeshInstance3D>("CarMesh/ambulance/wheel_frontLeft");

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        carMesh.Position = Position + sphereOffset;
        if (groundRay.IsColliding())//
        {
            ApplyCentralForce(-carMesh.GlobalBasis.Z * speedInput);
        }
        //GD.Print(carMesh.Position);

    }

    public override void _Process(double delta)
    {
        if (!groundRay.IsColliding()) //
        {
            return;
        }
        speedInput = Input.GetAxis("brake", "accelerate") * acceleration;
        turnInput = Input.GetAxis("steerRight", "steerLeft") * Mathf.DegToRad(steering);
        GD.Print(turnInput);
        rightWheel.Rotation = new Vector3(0,turnInput,Mathf.DegToRad(-180));
        leftWheel.Rotation = new Vector3(0, turnInput, 0);
        //if (0.13 > rightWheel.Rotation.Y || -0.13 < rightWheel.Rotation.Y)
        //{
        //    rightWheel.RotateY(-turnInput);
        //    leftWheel.RotateY(turnInput);
        //}
        //else
        //{
        //    rightWheel.Rotation = Vector3.Zero;
        //    leftWheel.Rotation = Vector3.Zero;
        //}
        //rightWheel.RotateY(turnInput);
        //leftWheel.RotateY(turnInput);


        if (LinearVelocity.Length() > turnStopLimit)
        {
            var newBasis = carMesh.GlobalBasis.Rotated(carMesh.GlobalBasis.Y, turnInput);
            carMesh.GlobalBasis = carMesh.GlobalBasis.Slerp(newBasis, (float)(turnSpeed*delta));
            carMesh.GlobalTransform = carMesh.GlobalTransform.Orthonormalized();

            var tilted = -turnInput * LinearVelocity.Length() / bodyTilt;
            bodyMesh.Rotation = new Vector3(0, 0, tilted);
            //if (bodyMesh.Rotation.Z < 35 || bodyMesh.Rotation.Z > -35)
            //{
            //    bodyMesh.RotateZ(tilted);
            //}
            
            
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