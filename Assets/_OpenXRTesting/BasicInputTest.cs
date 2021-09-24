using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInputTest : MonoBehaviour
{
    void Start()
    {
        BasicInput.onJoystickUpdate += BasicInput_onJoystickUpdate;
    }

    private void BasicInput_onJoystickUpdate(int controllerID, Vector2 axis)
    {
        BasicInput.PlayHaptics(controllerID, Mathf.Abs(axis.x), .1f);
    }
}
