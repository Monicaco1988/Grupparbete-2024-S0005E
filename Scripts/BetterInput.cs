using Godot;
using System;
using System.Collections.Generic;

public partial class BetterInput : Node
{
    private static Dictionary<StringName, Dictionary<int, InputActionData>> ActionData { get; }

    static BetterInput()
    {
        ActionData = new Dictionary<StringName, Dictionary<int, InputActionData>>();
    }

    public override void _Input(InputEvent inputEvent)
    {
        foreach (var action in InputMap.GetActions())
        {
            if (InputMap.EventIsAction(inputEvent, action))
            {
                var actionData = GetActionData(action, inputEvent.Device);

                if (inputEvent.IsPressed())
                {
                    actionData.IsPressed = true;
                    actionData.PressedFrame = Engine.GetFramesDrawn();
                }
                else if (inputEvent.IsActionReleased(action))
                {
                    actionData.IsPressed = false;
                    actionData.ReleasedFrame = Engine.GetFramesDrawn();
                }

                actionData.Strength = inputEvent.GetActionStrength(action);
            }
        }
    }

    public static bool IsActionPressed(StringName action, int device)
    {
        var actionData = GetActionData(action, device);
        return actionData.IsPressed;
    }

    public static bool IsActionReleased(StringName action, int device)
    {
        var actionData = GetActionData(action, device);
        return !actionData.IsPressed && actionData.ReleasedFrame == Engine.GetFramesDrawn() - 1;
    }

    public static bool IsActionJustPressed(StringName action, int device)
    {
        var actionData = GetActionData(action, device);
        if (actionData.IsPressed && actionData.PressedFrame == Engine.GetFramesDrawn() - 1)
        {
            actionData.IsPressed = false;
            return true;
        }
        return false;
    }

    public static float GetAxis(StringName negativeAction, StringName positiveAction, int device)
    {
        return GetActionData(positiveAction, device).Strength - GetActionData(negativeAction, device).Strength;
    }

    private static InputActionData GetActionData(StringName action, int device)
    {
        if (!ActionData.TryGetValue(action, out var innerDict))
        {
            innerDict = ActionData[action] = new Dictionary<int, InputActionData>();
        }

        if (!innerDict.TryGetValue(device, out var deviceActionData))
        {
            deviceActionData = innerDict[device] = new InputActionData();
        }

        return deviceActionData;
    }

    private class InputActionData
    {
        public bool IsPressed { get; set; } = false;
        public int PressedFrame { get; set; } = -1;
        public int ReleasedFrame { get; set; } = int.MaxValue;
        public float Strength { get; set; } = 0f;
    }
}