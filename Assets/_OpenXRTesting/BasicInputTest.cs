using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInputTest : MonoBehaviour
{
    void Start()
    {
        BasicInput.onJoystickUpdate += BasicInput_onJoystickUpdate;
        BasicInput.onGripPullUpdate += GripPull;
        BasicInput.onTriggerPullUpdate += TriggerPull;

        BasicInput.onPrimaryButtonUpdate += BasicInput_onPrimaryButtonUpdate;
        BasicInput.onSecondaryButtonUpdate += BasicInput_onSecondardButtonUpdate;
        BasicInput.onMenuButtonUpdate += BasicInput_onMenuButtonUpdate;
        BasicInput.onJoystickClicked += BasicInput_onJoystickClickUpdate;
    }

    private void BasicInput_onJoystickClickUpdate(int controllerID)
    {
        BasicInput.PlayHaptics(controllerID, .5f, .15f);
    }

    private void BasicInput_onMenuButtonUpdate(int controllerID)
    {
        BasicInput.PlayHaptics(controllerID, 1.0f, .25f);
    }

    private void BasicInput_onSecondardButtonUpdate(int controllerID)
    {
        BasicInput.PlayHaptics(controllerID, 1.0f, .5f);
    }

    private void BasicInput_onPrimaryButtonUpdate(int controllerID)
    {
        BasicInput.PlayHaptics(controllerID, 1.0f, 1.0f);
    }

    private void GripPull(int controllerID, float value)
    {
        BasicInput.PlayHaptics(controllerID, value, .1f);
    }

    private void TriggerPull(int controllerID, float value)
    {
        BasicInput.PlayHaptics(controllerID, value, .1f);
    }

    private void BasicInput_onJoystickUpdate(int controllerID, Vector2 axis)
    {
        BasicInput.PlayHaptics(controllerID, Mathf.Abs(axis.x), .1f);
    }
}
