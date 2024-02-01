using Godot;
using System;
using System.Runtime.CompilerServices;
using Godot.Collections;
using static Godot.TextServer;
using System.Linq;

public partial class input_manager : Node3D
{

    public event EventHandler<InputEvent> inputEventHandler;
    public delegate void inputEventCallback(object sender, InputEvent e);
    public class InputEvent : EventArgs
    {
        public InputEvent()
        {
        }
        public int id;
    }
    public class JoyStickEvent : InputEvent
    {
        public JoyStickEvent(int id, Vector3 direction)
        {
            this.direction = direction;
            this.id = id;
        }
        public Vector3 direction;
    }
    public class DeviceChange : InputEvent
    {
        public DeviceChange(int id, bool connected)
        {
            this.id = id;
            this.connected = connected;
        }
        public bool connected;
    }

   


   
    public int numberOfControllers;
    public Array<int> controller_ids;

    private static input_manager instance;  
    private input_manager() {
        this.controller_ids = Input.GetConnectedJoypads();
        this.numberOfControllers = Input.GetConnectedJoypads().Count;
        instance = this;
    }

    public static input_manager Instance()
    {
        if (instance == null)
        {
            instance = new input_manager();
        }
        return instance;
    }

   

    //public delegate void test_delegate();




	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Instance();

    }



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        var pads = Input.GetConnectedJoypads();
        if (pads.Count != numberOfControllers)
        {
            foreach (var pad in pads) {
                this.controller_ids.Remove(pad);
            }
            //var remvovedId = this.controller_ids[0]; 
            //this.controller_ids = pads;
            //this.ControllerChangeCallback.Invoke("N controllers" + Input.GetConnectedJoypads().Count);
            this.inputEventHandler?.Invoke(this, new DeviceChange(0, false));
        }
    }

    public void GetController()
    {

    }

    public Vector3 Player_Input(int id) 
    {
        //input_event?.Invoke(new Object(), new JoyStickEvent(3));
        var direction = Vector3.Zero;

        if (Input.GetJoyAxis(id, JoyAxis.LeftX) > 0.3f || Input.GetJoyAxis(id, JoyAxis.LeftX) < -0.3f)
        {
            direction.X = Input.GetJoyAxis(id, JoyAxis.LeftX);
        }
        if (Input.GetJoyAxis(id, JoyAxis.LeftY) > 0.3f || Input.GetJoyAxis(id, JoyAxis.LeftY) < -0.3f)
        {
            direction.Z = Input.GetJoyAxis(id, JoyAxis.LeftY);
        }

        //var aPlayer = gladiator.GetNode<AnimationPlayer>("AnimationPlayer");

        if (direction != Vector3.Zero) {

            //aPlayer.Play("sprint");
            //direction = direction.Normalized();
            //GetNode<Node3D>("Pivot").LookAt(Position + direction, Vector3.Up);
        } else {
            //aPlayer.Stop();
        }

        //_targetVelocity.X = direction.X * speed;
        //_targetVelocity.Z = direction.Z * speed;

        //Velocity = _targetVelocity;
        this.inputEventHandler?.Invoke(this, new JoyStickEvent(id,direction));
        return direction;
        //MoveAndSlide();
    }
}
