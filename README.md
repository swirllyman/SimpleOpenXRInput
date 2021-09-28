# SimpleOpenXRInput
Super simple and easy input using OpenXR. Allows for a high level setup that can be deployed to any VR HMD without having to make any changes. All input is event driven and can be easily accessed.

Input and haptic feedback has been tested and works on Quest 2, Oculus, and SteamVR using Unity 2021.1.20f1. 

# Setup
1) Install the Unity package "OpenXR" from the package manager.
2) Go to "Project Settings/XR Plug-in Management" and make sure OpenXR is selected.
3) Go to "Project Settings/XR Plug-in Management/OpenXR" and select the interaction profiles you would like to use.
4) Download and Import the Unity Package from [Releases](https://github.com/swirllyman/SimpleOpenXRInput/releases).
5) Place the script "SimpleOpenXRInput.cs" on an object in your Hierarchy.
6) Subscribe to the methods you want to use in your VR project. 

**Refer To "BasicInputText.cs" for an example on how to use the events / haptics.*

## **For Oculus Quest Users**
Before building make sure you have **"Oculus Quest Support" enabled** in the **OpenXR Feature Groups** under the panel "Project Settings/XR Plug-in Management/OpenXR"
