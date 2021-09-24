using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem.XR.Haptics;

public class BasicInput : MonoBehaviour
{
    public static BasicInput singleton;
    public InputActionAsset actionAsset;
    public Haptics myHaptics;
    public InputActions inputActions;

    public delegate void JoystickPositionUpdated(int controllerID, Vector2 axis);
    public static event JoystickPositionUpdated onJoystickUpdate;

    public delegate void TriggerPullUpdated(int controllerID, Vector2 axis);
    public static event TriggerPullUpdated onTriggerUpdate;

    public delegate void TriggerClickedUpdated(int controllerID, bool clicked);
    public static event TriggerClickedUpdated onTriggerClicked;

    #region Mono
    void Awake()
    {
        if(singleton != null)
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;
        myHaptics.SetupHaptics();
        inputActions.SetupActions(this);
    }

    private void OnEnable()
    {
        if (actionAsset != null)
        {
            actionAsset.Enable();
        }
    }
    #endregion

    #region Callbacks
    internal void JoystickPositionUpdate(int controllerID, Vector2 axis)
    {
        onJoystickUpdate?.Invoke(controllerID, axis);
    }

    internal void TriggerPullUpdate(int controllerID, Vector2 axis)
    {
        onTriggerUpdate?.Invoke(controllerID, axis);
    }

    internal void TriggerClickUpdate(int controllerID, bool clicked)
    {
        onTriggerClicked?.Invoke(controllerID, clicked);
    }
    #endregion

    #region Static Methods
    public static void PlayHaptics(int controllerID, float amplitude, float duration)
    {
        singleton.myHaptics.PlayHaptics(controllerID, amplitude, duration);
    }
    #endregion
}

#region Input Actions
[System.Serializable]
public class InputActions
{
    BasicInput myInput;
    #region Joystick
    [Header("Joystick")]
    public InputActionReference joystickAction_Right;
    public InputActionReference joystickAction_Left;

    private void Joystick_Right(InputAction.CallbackContext obj)
    {
        Vector2 turnVector = obj.ReadValue<Vector2>();
        myInput.JoystickPositionUpdate(0, turnVector);
    }

    private void Joystick_Left(InputAction.CallbackContext obj)
    {
        Vector2 turnVector = obj.ReadValue<Vector2>();
        myInput.JoystickPositionUpdate(1, turnVector);
    }
    #endregion

    #region Trigger
    [Header("Trigger")]
    public InputActionReference triggerPull_Right;
    public InputActionReference triggerPull_Left;
    public InputActionReference triggerClick_Right;
    public InputActionReference triggerClick_Left;

    private void TriggerPull_Right(InputAction.CallbackContext obj)
    {
        Vector2 turnVector = obj.ReadValue<Vector2>();
        myInput.JoystickPositionUpdate(0, turnVector);
    }

    private void TriggerPull_Left(InputAction.CallbackContext obj)
    {
        Vector2 turnVector = obj.ReadValue<Vector2>();
        myInput.JoystickPositionUpdate(1, turnVector);
    }

    private void TriggerClick_Right(InputAction.CallbackContext obj)
    {
        Vector2 turnVector = obj.ReadValue<Vector2>();
        myInput.JoystickPositionUpdate(0, turnVector);
    }

    private void TriggerClick_Left(InputAction.CallbackContext obj)
    {
        Vector2 turnVector = obj.ReadValue<Vector2>();
        myInput.JoystickPositionUpdate(1, turnVector);
    }
    #endregion

    internal void SetupActions(BasicInput input)
    {
        myInput = input;

        //Joystick
        joystickAction_Right.action.performed += Joystick_Right;
        joystickAction_Left.action.performed += Joystick_Left;

        //Trigger
        triggerPull_Right.action.performed += TriggerPull_Right;
        triggerPull_Right.action.performed += TriggerPull_Left;
        triggerClick_Right.action.performed += TriggerClick_Right;
        triggerClick_Left.action.performed += TriggerClick_Left;
    }
}
#endregion

#region Haptics
[System.Serializable]
public class Haptics
{
    [Header("Assign All Right Hand and Left Hand Input Actions")]
    public InputActionReference[] rightHandActions;
    public InputActionReference[] leftHandActions;
    XRControllerWithRumble[] xrControllers = new XRControllerWithRumble[2];
    bool[] controllersAssigned = new bool[2];

    internal void PlayHaptics(int controllerID, float amplitude, float duration)
    {
        if(xrControllers[controllerID] != null)
            xrControllers[controllerID].SendImpulse(amplitude, duration);
    }

    internal void SetupHaptics()
    {
        for (int i = 0; i < rightHandActions.Length; i++)
        {
            rightHandActions[i].action.performed += SetupRightController;
        }

        for (int i = 0; i < leftHandActions.Length; i++)
        {
            leftHandActions[i].action.performed += SetupLeftController;
        }
    }

    void SetupRightController(InputAction.CallbackContext obj) 
    {
        if (!controllersAssigned[0]) controllersAssigned[0] = AssignController(obj.action, 0);
        if (controllersAssigned[0])
        {
            for (int i = 0; i < rightHandActions.Length; i++)
            {
                rightHandActions[i].action.performed -= SetupRightController;
            }
        }
    }

    void SetupLeftController(InputAction.CallbackContext obj)
    {
        if (!controllersAssigned[1]) controllersAssigned[1] = AssignController(obj.action, 1);
        if (controllersAssigned[1])
        {
            for (int i = 0; i < leftHandActions.Length; i++)
            {
                leftHandActions[i].action.performed -= SetupRightController;
            }
        }
    }

    bool AssignController(InputAction inputAction, int handID)
    {
        var control = inputAction.activeControl;
        if (null == control)
            return false;
        else
        {
            if (control.device is XRControllerWithRumble rumble)
            {
                xrControllers[handID] = rumble;
                return true;
            }
        }
        return false;
    }
}
#endregion