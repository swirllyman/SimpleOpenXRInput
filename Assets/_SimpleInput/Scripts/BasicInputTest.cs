using UnityEngine;
using SimpleInput;

public class BasicInputTest : MonoBehaviour
{
    void Start()
    {
        SimpleOpenXRInput.onJoystickUpdate += BasicInput_onJoystickUpdate;
        SimpleOpenXRInput.onGripPullUpdate += GripPull;
        SimpleOpenXRInput.onTriggerPullUpdate += TriggerPull;

        SimpleOpenXRInput.onPrimaryButtonUpdate += BasicInput_onPrimaryButtonUpdate;
        SimpleOpenXRInput.onSecondaryButtonUpdate += BasicInput_onSecondardButtonUpdate;
        SimpleOpenXRInput.onMenuButtonUpdate += BasicInput_onMenuButtonUpdate;
        SimpleOpenXRInput.onJoystickClicked += BasicInput_onJoystickClickUpdate;
    }

    private void BasicInput_onJoystickClickUpdate(int controllerID, bool down)
    {
        if (down)
            SimpleOpenXRInput.PlayHaptics(controllerID, .5f, .15f);
    }

    private void BasicInput_onMenuButtonUpdate(int controllerID, bool down)
    {
        if (down)
            SimpleOpenXRInput.PlayHaptics(controllerID, 1.0f, .25f);
    }

    private void BasicInput_onSecondardButtonUpdate(int controllerID, bool down)
    {
        if(down)
            SimpleOpenXRInput.PlayHaptics(controllerID, 1.0f, .5f);
    }

    private void BasicInput_onPrimaryButtonUpdate(int controllerID, bool down)
    {
        if (down)
            SimpleOpenXRInput.PlayHaptics(controllerID, 1.0f, 1.0f);
    }

    private void GripPull(int controllerID, float value)
    {
        SimpleOpenXRInput.PlayHaptics(controllerID, value, .1f);
    }

    private void TriggerPull(int controllerID, float value)
    {
        SimpleOpenXRInput.PlayHaptics(controllerID, value, .1f);
    }

    private void BasicInput_onJoystickUpdate(int controllerID, Vector2 axis)
    {
        SimpleOpenXRInput.PlayHaptics(controllerID, (Mathf.Abs(axis.x) + Mathf.Abs(axis.y)) / 2, .1f);
    }
}