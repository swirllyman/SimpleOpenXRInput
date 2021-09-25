using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class BasicInput : MonoBehaviour
{
    public static BasicInput singleton;
    public InputActionAsset actionAsset;
    public Haptics myHaptics;
    public InputActions inputActions;

    public delegate void AxisUpdated(int controllerID, Vector2 axis);
    public delegate void FloatUpdated(int controllerID, float value);
    public delegate void ButtonUpdated(int controllerID);

    //Subscribe to these events for simple input usage
    public static event AxisUpdated onJoystickUpdate;

    public static event FloatUpdated onGripPullUpdate;
    public static event FloatUpdated onTriggerPullUpdate;

    public static event ButtonUpdated onJoystickClicked;
    public static event ButtonUpdated onTriggerClicked;
    public static event ButtonUpdated onPrimaryButtonUpdate;
    public static event ButtonUpdated onSecondaryButtonUpdate;
    public static event ButtonUpdated onMenuButtonUpdate;

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

    #region Callback Handling
    //Joystick
    internal void JoystickPositionUpdate(int controllerID, Vector2 axis)
    {
        onJoystickUpdate?.Invoke(controllerID, axis);
    }

    internal void JoystickClickUpdate(int controllerID)
    {
        onJoystickClicked?.Invoke(controllerID);
    }
    
    //Trigger
    internal void TriggerPullUpdate(int controllerID, float pullAmount)
    {
        onTriggerPullUpdate?.Invoke(controllerID, pullAmount);
    }

    internal void TriggerClickUpdate(int controllerID)
    {
        onTriggerClicked?.Invoke(controllerID);
    }

    //Grip
    internal void GripPullUpdate(int controllerID, float pullAmount)
    {
        onGripPullUpdate?.Invoke(controllerID, pullAmount);
    }

    //Primary, Secondary and Menu
    internal void PrimaryButtonUpdate(int controllerID)
    {
        onPrimaryButtonUpdate?.Invoke(controllerID);
    }

    internal void SecondaryButtonUpdate(int controllerID)
    {
        onSecondaryButtonUpdate?.Invoke(controllerID);
    }

    internal void MenuButtonUpdate(int controllerID)
    {
        onMenuButtonUpdate?.Invoke(controllerID);
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
    InputActionMap map;

    #region Joystick
    [Header("Joystick")]
    public InputActionReference joystickAction_Right;
    public InputActionReference joystickAction_Left;
    public InputActionReference joystickClick_Right;
    public InputActionReference joystickClick_Left;

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

    private void JoystickClick_Right(InputAction.CallbackContext obj)
    {
        myInput.JoystickClickUpdate(0);
    }

    private void JoystickClick_Left(InputAction.CallbackContext obj)
    {
        myInput.JoystickClickUpdate(1);
    }
    #endregion

    #region Grip
    [Header("Grip")]
    public InputActionReference gripPull_Right;
    public InputActionReference gripPull_Left;

    private void GripPull_Right(InputAction.CallbackContext obj)
    {
        float pullAmount = obj.ReadValue<float>();
        myInput.GripPullUpdate(0, pullAmount);
    }

    private void GripPull_Left(InputAction.CallbackContext obj)
    {
        float pullAmount = obj.ReadValue<float>();
        myInput.GripPullUpdate(1, pullAmount);
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
        float pullAmount = obj.ReadValue<float>();
        myInput.TriggerPullUpdate(0, pullAmount);
    }

    private void TriggerPull_Left(InputAction.CallbackContext obj)
    {
        float pullAmount = obj.ReadValue<float>();
        myInput.TriggerPullUpdate(1, pullAmount);
    }

    private void TriggerClick_Right(InputAction.CallbackContext obj)
    {
        myInput.TriggerClickUpdate(0);
    }

    private void TriggerClick_Left(InputAction.CallbackContext obj)
    {
        myInput.TriggerClickUpdate(1);
    }
    #endregion

    #region Basic Buttons
    public InputActionReference primaryButton_Right;
    public InputActionReference primaryButton_Left;

    public InputActionReference secondaryButton_Right;
    public InputActionReference secondaryButton_Left;

    public InputActionReference menuButton_Right;
    public InputActionReference menuButton_Left;

    private void PrimaryButton_Right(InputAction.CallbackContext obj)
    {
        myInput.PrimaryButtonUpdate(0);
    }

    private void PrimaryButton_Left(InputAction.CallbackContext obj)
    {
        myInput.PrimaryButtonUpdate(1);
    }

    private void SecondaryButton_Right(InputAction.CallbackContext obj)
    {
        myInput.SecondaryButtonUpdate(0);
    }

    private void SecondaryButton_Left(InputAction.CallbackContext obj)
    {
        myInput.SecondaryButtonUpdate(1);
    }

    private void MenuButton_Right(InputAction.CallbackContext obj)
    {
        myInput.MenuButtonUpdate(0);
    }

    private void MenuButton_Left(InputAction.CallbackContext obj)
    {
        myInput.MenuButtonUpdate(1);
    }
    #endregion

    internal void SetupActions(BasicInput input)
    {
        myInput = input;
        map = new InputActionMap();
        map.AddAction("Joystick_Right", binding: "<XRController>{RightHand}/{primary2DAxis}");

        map.Enable();

        //Joystick
        InputAction rightStickAction = map.FindAction("Joystick_Right");
        if(rightStickAction != null)
            rightStickAction.performed += Joystick_Right;

        joystickAction_Left.action.performed += Joystick_Left;
        joystickClick_Right.action.performed += JoystickClick_Right;
        joystickClick_Left.action.performed += JoystickClick_Left;

        //Trigger
        triggerPull_Right.action.performed += TriggerPull_Right;
        triggerPull_Left.action.performed += TriggerPull_Left;
        triggerClick_Right.action.performed += TriggerClick_Right;
        triggerClick_Left.action.performed += TriggerClick_Left;

        //Grip
        gripPull_Right.action.performed += GripPull_Right;
        gripPull_Left.action.performed += GripPull_Left;

        //Primary Button
        primaryButton_Right.action.performed += PrimaryButton_Right;
        primaryButton_Left.action.performed += PrimaryButton_Left;

        //Secondary Button
        secondaryButton_Right.action.performed += SecondaryButton_Right;
        secondaryButton_Left.action.performed += SecondaryButton_Left;

        //Menu Button
        menuButton_Right.action.performed += MenuButton_Right;
        menuButton_Left.action.performed += MenuButton_Left;
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
        if (xrControllers[controllerID] != null && amplitude > .05f)
        {
            Debug.Log("Playing Haptics: " + controllerID + ", " + amplitude + ", " + duration);
            xrControllers[controllerID].SendImpulse(amplitude, duration);
        }
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