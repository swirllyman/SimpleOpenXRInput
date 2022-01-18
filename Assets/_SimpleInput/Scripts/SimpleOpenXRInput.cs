using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace SimpleInput
{
    /// <summary>Provides functionality for basic OpenXR Functions.</summary>
    /// <remarks>
    /// Uses callbacks and static methods for a simple high level OpenXR Input manager.
    /// Check BasicInputTest.cs for a better understanding on how to use.
    /// </remarks>
    public class SimpleOpenXRInput : MonoBehaviour
    {
        #region -- Public Interface ---------------------------------

        #region -- Events -------------------------------------------
        /// <summary> Invoked anytime joystick position is updated -- JoystickPositionUpdate(int controllerID, Vector2 axis) 
        /// </summary>
        public static event AxisUpdated onJoystickUpdate;

        /// <summary> Invoked anytime Grip is updated -- GripPullUpdate(int controllerID, float value)
        /// </summary>
        public static event FloatUpdated onGripPullUpdate;

        public static event ButtonUpdated onGripClicked;

        /// <summary> Invoked anytime Trigger is updated -- TriggerPullUpdate(int controllerID, float value)
        /// </summary>
        public static event FloatUpdated onTriggerPullUpdate;

        /// <summary> Invoked when Joystick is clicked (pressed in) -- JoystickClickUpdate(int controllerID)
        /// </summary>
        public static event ButtonUpdated onJoystickClicked;

        /// <summary> Invoked when Trigger is clicked -- TriggerClickUpdate(int controllerID, bool down)
        /// </summary>
        public static event ButtonUpdated onTriggerClicked;

        /// <summary> Invoked when Primary Button is pressed -- PrimaryButtonUpdate(int controllerID, bool down)
        /// </summary>
        public static event ButtonUpdated onPrimaryButtonUpdate;

        /// <summary> Invoked when Secondary Button is pressed -- SecondaryButtonUpdate(int controllerID, bool down)
        /// </summary>
        public static event ButtonUpdated onSecondaryButtonUpdate;

        /// <summary> Invoked when Menu Button is pressed -- MenuButtonUpdate(int controllerID, bool down)
        /// </summary>
        public static event ButtonUpdated onMenuButtonUpdate;
        #endregion

        #region --  Static Getters
        /// <summary> Call to make controllers vibrate / rumble / etc...
        /// </summary>
        /// <param name="controllerID">Controller to Vibrate</param>
        /// <param name="amplitude">Vibrate Amount (0.0f - 1.0f)</param>
        /// <param name="duration">Time in seconds vibration will happen for</param>
        public static void PlayHaptics(int controllerID, float amplitude, float duration)
        {
            Haptics.PlayHaptics(controllerID, amplitude, duration);
        }

        /// <summary>
        /// Get a Value between 0.0 and 1.0 to determine the amount the Grip is currently squeezed
        /// </summary>
        /// <param name="handID">Hand ID correlated</param>
        /// <returns>Current Grip Value</returns>
        public static float GetGripValue(int handID)
        {
            float gripValue;
            if (handID == 0)
            {
                gripValue = leftHandInputActions[4].ReadValue<float>();
            }
            else
            {
                gripValue = rightHandInputActions[4].ReadValue<float>();
            }
            return gripValue;
        }

        /// <summary>
        /// Get a Value between 0.0 and 1.0 to determine the amount the Trigger is currently squeezed
        /// </summary>
        /// <param name="handID">Hand ID correlated</param>
        /// <returns>Current Trigger Value</returns>
        public static float GetTriggerValue(int handID)
        {
            float triggerValue;
            if (handID == 0)
            {
                triggerValue = leftHandInputActions[2].ReadValue<float>();
            }
            else
            {
                triggerValue = rightHandInputActions[2].ReadValue<float>();
            }
            return triggerValue;
        }

        /// <summary>
        /// Get the Vector2 between 0.0 and 1.0 to determine the current Joystick amount
        /// </summary>
        /// <param name="handID">Hand ID correlated</param>
        /// <returns>X and Y values of current Joystick</returns>
        public static Vector2 GetJoystickValue(int handID)
        {
            Vector2 joystickValue;
            if (handID == 0)
            {
                joystickValue = leftHandInputActions[0].ReadValue<Vector2>();
            }
            else
            {
                joystickValue = rightHandInputActions[0].ReadValue<Vector2>();
            }
            return joystickValue;
        }
        #endregion

        #endregion

        #region -- Mono --------------------------------------------

        // Can be initialized elsewhere if you don't want to use the Awake Method.
        void Awake()
        {
            SetupOpenXRInput();
        }

        #endregion

        #region -- Internal ----------------------------------------

        #region Delegates and Variables

        public delegate void AxisUpdated(int controllerID, Vector2 axis);
        public delegate void FloatUpdated(int controllerID, float value);
        public delegate void ButtonUpdated(int controllerID, bool down);
        public static List<InputAction> leftHandInputActions = new List<InputAction>();
        public static List<InputAction> rightHandInputActions = new List<InputAction>();
        Haptics myHaptics;
        #endregion

        #region Action Map

        //Build Action Map based on Unity OpenXR standard for binding configurations
        void SetupOpenXRInput()
        {
            leftHandInputActions = new List<InputAction>();
            rightHandInputActions = new List<InputAction>();
            InputActionMap actionMap = new InputActionMap();

            //Setup Left Hand Actions And Add To Map
            leftHandInputActions.Add(actionMap.AddAction("Joystick_Left", binding: "<XRController>{LeftHand}/{primary2DAxis}"));
            leftHandInputActions.Add(actionMap.AddAction("JoystickClick_Left", binding: "<XRController>{LeftHand}/{primary2DAxisClick}"));
            leftHandInputActions.Add(actionMap.AddAction("Trigger_Left", binding: "<XRController>{LeftHand}/{trigger}"));
            leftHandInputActions.Add(actionMap.AddAction("TriggerClick_Left", binding: "<XRController>{LeftHand}/{triggerButton}"));
            leftHandInputActions.Add(actionMap.AddAction("Grip_Left", binding: "<XRController>{LeftHand}/{grip}"));
            leftHandInputActions.Add(actionMap.AddAction("Primary_Left", binding: "<XRController>{LeftHand}/{primaryButton}"));
            leftHandInputActions.Add(actionMap.AddAction("Secondary_Left", binding: "<XRController>{LeftHand}/{secondaryButton}"));
            leftHandInputActions.Add(actionMap.AddAction("Menu_Left", binding: "<XRController>{LeftHand}/menu"));

            //Setup Right Hand Actions And Add To Map
            rightHandInputActions.Add(actionMap.AddAction("Joystick_Right", binding: "<XRController>{RightHand}/{primary2DAxis}"));
            rightHandInputActions.Add(actionMap.AddAction("JoystickClick_Right", binding: "<XRController>{RightHand}/{primary2DAxisClick}"));
            rightHandInputActions.Add(actionMap.AddAction("Trigger_Right", binding: "<XRController>{RightHand}/{trigger}"));
            rightHandInputActions.Add(actionMap.AddAction("TriggerClick_Right", binding: "<XRController>{RightHand}/{triggerButton}"));
            rightHandInputActions.Add(actionMap.AddAction("Grip_Right", binding: "<XRController>{RightHand}/{grip}"));
            rightHandInputActions.Add(actionMap.AddAction("Primary_Right", binding: "<XRController>{RightHand}/{primaryButton}"));
            rightHandInputActions.Add(actionMap.AddAction("Secondary_Right", binding: "<XRController>{RightHand}/{secondaryButton}"));
            rightHandInputActions.Add(actionMap.AddAction("Menu_Right", binding: "<XRController>{RightHand}/menu"));

            //IMPORTANT: Make sure you enable the Map once created.
            actionMap.Enable();

            myHaptics = new Haptics(leftHandInputActions.ToArray(), rightHandInputActions.ToArray());
            new InputActions(this, actionMap);
        }

        #endregion

        #region Callbacks

        //Joystick
        internal void JoystickPositionUpdate(int controllerID, Vector2 axis)
        {
            onJoystickUpdate?.Invoke(controllerID, axis);
        }

        internal void JoystickClickUpdate(int controllerID, bool down)
        {
            onJoystickClicked?.Invoke(controllerID, down);
        }

        //Trigger
        internal void TriggerPullUpdate(int controllerID, float value)
        {
            onTriggerPullUpdate?.Invoke(controllerID, value);
        }

        internal void TriggerClickUpdate(int controllerID, bool down)
        {
            onTriggerClicked?.Invoke(controllerID, down);
        }

        //Grip
        internal void GripPullUpdate(int controllerID, float value)
        {
            onGripPullUpdate?.Invoke(controllerID, value);
        }

        internal void GripButtonUpdate(int controllerID, bool down)
        {
            onGripClicked?.Invoke(controllerID, down);
        }

        //Primary, Secondary and Menu
        internal void PrimaryButtonUpdate(int controllerID, bool down)
        {
            onPrimaryButtonUpdate?.Invoke(controllerID, down);
        }

        internal void SecondaryButtonUpdate(int controllerID, bool down)
        {
            onSecondaryButtonUpdate?.Invoke(controllerID, down);
        }

        //Menu
        internal void MenuButtonUpdate(int controllerID, bool down)
        {
            onMenuButtonUpdate?.Invoke(controllerID, down);
        }

        #endregion

        #region Helper Classes

        #region Input Actions

        /// <summary> Class used for converting all the Input actions
        /// into events.
        /// </summary>
        class InputActions
        {
            SimpleOpenXRInput myInput;

            public InputActions(SimpleOpenXRInput input, InputActionMap actionMap)
            {
                myInput = input;

                //Joystick
                InputAction leftStickAction = actionMap.FindAction("Joystick_Left");
                if (leftStickAction != null) leftStickAction.performed += Joystick_Left;

                InputAction rightStickAction = actionMap.FindAction("Joystick_Right");
                if (rightStickAction != null) rightStickAction.performed += Joystick_Right;

                InputAction leftStickClick = actionMap.FindAction("JoystickClick_Left");
                if (leftStickClick != null) leftStickClick.started += JoystickClickDown_Left;
                if (leftStickClick != null) leftStickClick.canceled += JoystickClickUp_Left;

                InputAction rightStickClick = actionMap.FindAction("JoystickClick_Right");
                if (rightStickClick != null) rightStickClick.started += JoystickClickDown_Right;
                if (rightStickClick != null) rightStickClick.canceled += JoystickClickUp_Right;


                //Trigger
                InputAction leftTriggerPull = actionMap.FindAction("Trigger_Left");
                if (leftTriggerPull != null) leftTriggerPull.performed += TriggerPull_Left;

                InputAction rightTriggerPull = actionMap.FindAction("Trigger_Right");
                if (rightTriggerPull != null) rightTriggerPull.performed += TriggerPull_Right;

                InputAction leftTriggerClick = actionMap.FindAction("TriggerClick_Left");
                if (leftTriggerClick != null) leftTriggerClick.started += TriggerClickDown_Left;
                if (leftTriggerClick != null) leftTriggerClick.canceled += TriggerClickUp_Left;

                InputAction rightTriggerClick = actionMap.FindAction("TriggerClick_Right");
                if (rightTriggerClick != null) rightTriggerClick.started += TriggerClickDown_Right;
                if (rightTriggerClick != null) rightTriggerClick.canceled += TriggerClickUp_Right;


                //Grip
                InputAction leftGripPull = actionMap.FindAction("Grip_Left");
                if (leftGripPull != null) leftGripPull.performed += Grip_Left;

                InputAction rightGripPull = actionMap.FindAction("Grip_Right");
                if (rightGripPull != null) rightGripPull.performed += Grip_Right;

                InputAction gripRight = actionMap.FindAction("Grip_Right");
                if (gripRight != null)
                {
                    gripRight.started += GripButtonDown_Right;
                    gripRight.canceled += GripButtonUp_Right;
                }

                InputAction gripLeft = actionMap.FindAction("Grip_Left");
                if (gripLeft != null)
                {
                    gripLeft.started += GripButtonDown_Left;
                    gripLeft.canceled += GripButtonUp_Left;
                }

                //Basic Buttons
                InputAction primaryLeft = actionMap.FindAction("Primary_Left");
                if (primaryLeft != null) primaryLeft.started += PrimaryButtonDown_Left;
                if (primaryLeft != null) primaryLeft.canceled += PrimaryButtonUp_Left;

                InputAction primaryRight = actionMap.FindAction("Primary_Right");
                if (primaryRight != null) primaryRight.started += PrimaryButtonDown_Right;
                if (primaryRight != null) primaryRight.canceled += PrimaryButtonUp_Right;

                InputAction secondaryLeft = actionMap.FindAction("Secondary_Left");
                if (secondaryLeft != null) secondaryLeft.started += SecondaryButtonDown_Left;
                if (secondaryLeft != null) secondaryLeft.canceled += SecondaryButtonUp_Left;

                InputAction secondaryRight = actionMap.FindAction("Secondary_Right");
                if (secondaryRight != null) secondaryRight.started += SecondaryButtonDown_Right;
                if (secondaryRight != null) secondaryRight.canceled += SecondaryButtonUp_Right;

                //Menu
                InputAction menuLeft = actionMap.FindAction("Menu_Left");
                if (menuLeft != null) menuLeft.started += MenuButtonDown_Left;
                if (menuLeft != null) menuLeft.canceled += MenuButtonUp_Left;

                InputAction menuRight = actionMap.FindAction("Menu_Right");
                if (menuRight != null) menuRight.started += MenuButtonDown_Right;
                if (menuRight != null) menuRight.canceled += MenuButtonUp_Right;
            }

            #region Joystick
            private void Joystick_Left(InputAction.CallbackContext obj)
            {
                Vector2 turnVector = obj.ReadValue<Vector2>();
                myInput.JoystickPositionUpdate(0, turnVector);
            }

            private void Joystick_Right(InputAction.CallbackContext obj)
            {
                Vector2 turnVector = obj.ReadValue<Vector2>();
                myInput.JoystickPositionUpdate(1, turnVector);
            }

            private void JoystickClickDown_Left(InputAction.CallbackContext obj)
            {
                myInput.JoystickClickUpdate(0, true);
            }

            private void JoystickClickUp_Left(InputAction.CallbackContext obj)
            {
                myInput.JoystickClickUpdate(0, false);
            }

            private void JoystickClickDown_Right(InputAction.CallbackContext obj)
            {
                myInput.JoystickClickUpdate(1, true);
            }

            private void JoystickClickUp_Right(InputAction.CallbackContext obj)
            {
                myInput.JoystickClickUpdate(1, false);
            }
            #endregion

            #region Trigger
            //Trigger Pull -- 0-1 float
            private void TriggerPull_Left(InputAction.CallbackContext obj)
            {
                float pullAmount = obj.ReadValue<float>();
                myInput.TriggerPullUpdate(0, pullAmount);
            }

            private void TriggerPull_Right(InputAction.CallbackContext obj)
            {
                float pullAmount = obj.ReadValue<float>();
                myInput.TriggerPullUpdate(1, pullAmount);
            }

            //Click -- Bool
            private void TriggerClickUp_Left(InputAction.CallbackContext obj)
            {
                myInput.TriggerClickUpdate(0, false);
            }

            private void TriggerClickDown_Left(InputAction.CallbackContext obj)
            {
                myInput.TriggerClickUpdate(0, true);
            }

            private void TriggerClickUp_Right(InputAction.CallbackContext obj)
            {
                myInput.TriggerClickUpdate(1, false);
            }

            private void TriggerClickDown_Right(InputAction.CallbackContext obj)
            {
                myInput.TriggerClickUpdate(1, true);
            }

            #endregion

            #region Grip
            private void Grip_Left(InputAction.CallbackContext obj)
            {
                float pullAmount = obj.ReadValue<float>();
                myInput.GripPullUpdate(0, pullAmount);
            }

            private void Grip_Right(InputAction.CallbackContext obj)
            {
                float pullAmount = obj.ReadValue<float>();
                myInput.GripPullUpdate(1, pullAmount);
            }

            private void GripButtonUp_Right(InputAction.CallbackContext obj)
            {
                myInput.GripButtonUpdate(1, false);
            }

            private void GripButtonDown_Right(InputAction.CallbackContext obj)
            {
                myInput.GripButtonUpdate(1, true);
            }

            private void GripButtonUp_Left(InputAction.CallbackContext obj)
            {
                myInput.GripButtonUpdate(0, false);
            }

            private void GripButtonDown_Left(InputAction.CallbackContext obj)
            {
                myInput.GripButtonUpdate(0, true);
            }
            #endregion

            #region Basic Buttons
            //Primary Buttons
            private void PrimaryButtonDown_Left(InputAction.CallbackContext obj)
            {
                myInput.PrimaryButtonUpdate(0, true);
            }
            private void PrimaryButtonUp_Left(InputAction.CallbackContext obj)
            {
                myInput.PrimaryButtonUpdate(0, false);
            }

            private void PrimaryButtonDown_Right(InputAction.CallbackContext obj)
            {
                myInput.PrimaryButtonUpdate(1, true);
            }
            private void PrimaryButtonUp_Right(InputAction.CallbackContext obj)
            {
                myInput.PrimaryButtonUpdate(1, false);
            }

            //Secondary Buttons
            private void SecondaryButtonDown_Left(InputAction.CallbackContext obj)
            {
                myInput.SecondaryButtonUpdate(0, true);
            }
            private void SecondaryButtonUp_Left(InputAction.CallbackContext obj)
            {
                myInput.SecondaryButtonUpdate(0, false);
            }

            private void SecondaryButtonDown_Right(InputAction.CallbackContext obj)
            {
                myInput.SecondaryButtonUpdate(1, true);
            }
            private void SecondaryButtonUp_Right(InputAction.CallbackContext obj)
            {
                myInput.SecondaryButtonUpdate(1, false);
            }

            //Menu Buttons
            private void MenuButtonDown_Left(InputAction.CallbackContext obj)
            {
                myInput.MenuButtonUpdate(0, true);
            }
            private void MenuButtonUp_Left(InputAction.CallbackContext obj)
            {
                myInput.MenuButtonUpdate(0, false);
            }

            private void MenuButtonDown_Right(InputAction.CallbackContext obj)
            {
                myInput.MenuButtonUpdate(1, true);
            }
            private void MenuButtonUp_Right(InputAction.CallbackContext obj)
            {
                myInput.MenuButtonUpdate(1, false);
            }
            #endregion
        }

        #endregion

        #region Haptics

        /// <summary> Simple class used to access haptic feedback.
        /// </summary>
        /// <remarks> A somewhat hacky solution until there is a better way to detect both "XRControllerWithRumble"
        /// </remarks>
        class Haptics
        {
            static XRControllerWithRumble[] xrControllers = new XRControllerWithRumble[2];
            InputAction[] leftHandActions, rightHandActions;
            bool[] controllersAssigned = new bool[2];

            //Initialized wtih all possible input actions so we can detect when a controller was used.
            public Haptics(InputAction[] newLeftHandActions, InputAction[] newRightHandActions)
            {
                leftHandActions = newLeftHandActions;
                rightHandActions = newRightHandActions;
                for (int i = 0; i < leftHandActions.Length; i++)
                {
                    leftHandActions[i].performed += SetupLeftController;
                }

                for (int i = 0; i < rightHandActions.Length; i++)
                {
                    rightHandActions[i].performed += SetupRightController;
                }
            }

            static internal void PlayHaptics(int controllerID, float amplitude, float duration)
            {
                if (xrControllers[controllerID] != null && amplitude > .05f)
                {
                    xrControllers[controllerID].SendImpulse(amplitude, duration);
                }
            }

            void SetupLeftController(InputAction.CallbackContext obj)
            {
                if (!controllersAssigned[0]) controllersAssigned[0] = AssignController(obj.action, 0);

                // Make sure to Unsubscribe from Setup
                if (controllersAssigned[0])
                {
                    for (int i = 0; i < leftHandActions.Length; i++)
                    {
                        leftHandActions[i].performed -= SetupLeftController;
                    }
                }
            }

            void SetupRightController(InputAction.CallbackContext obj)
            {
                if (!controllersAssigned[1]) controllersAssigned[1] = AssignController(obj.action, 1);

                // Make sure to Unsubscribe from Setup
                if (controllersAssigned[1])
                {
                    for (int i = 0; i < rightHandActions.Length; i++)
                    {
                        rightHandActions[i].performed -= SetupRightController;
                    }
                }
            }

            bool AssignController(InputAction inputAction, int handID)
            {
                var control = inputAction.activeControl;
                if (control == null)
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
        #endregion
        #endregion
    }
}