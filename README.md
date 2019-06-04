# prg_event_sim

## Step 1: Gathering Data
The goal is to record every frame of a driving scene in Unity. CaptureFramesAndPose.cs is a Unity camera component that completes this task. For each frame, the time and pose of the camera is recorded. The intrinsic matrix is also recorded.  

### There are two properties of the component: 'Folder' and 'Frame Rate'.
  * 'Folder' specifies the name of the folder that will contain the recorded data.
    * The folder will be located within the scene directory.
  * 'Frame Rate' specifies how many frames per second are to be recorded

### CaptureFramesAndPose.cs is located in the Assets folder of the sample scenes
  * [prg_event_sim/AirSim/Unity/UnityDemo/Assets/CaptureFramesAndPose/CaptureFramesAndPose.cs](/AirSim/Unity/UnityDemo/Assets/CaptureFramesAndPose/CaptureFramesAndPose.cs)
  * [prg_event_sim/image-synthesis-Event-Signals/Assets/CaptureFramesAndPose/CaptureFramesAndPose.cs](/image-synthesis-Event-Signals/Assets/CaptureFramesAndPose/CaptureFramesAndPose.cs)
  
### Calculation of the intrinsic matrix
 * Unity provides the focal length and sensor size of a camera in terms of milimeters. The focal length in terms of pixels was obtained from the following equation:
   * focal_pixel = (focal_mm / sensor_width_mm) * image_width_in_pixels

### The intrinsic matrix obtained from Unity was validated using Matlab's [Single Camera Calibrator App](https://www.mathworks.com/help/vision/ug/single-camera-calibrator-app.html)
  * Matlab's app requires images which feature a certain checkerboard pattern. The size of each square must also be known.
  * Such images where obtained in a [Unity scene made for calibration](/calibration%20scene)
    * [Folder containing the images](/calibration%20scene/CalibrationImages)
    * A 7x10 checkerboard pattern was placed as a texture on a 3.5x5 meter cube in Unity. 
      * Thus, the size of each square is .5 meters.
  * App Parameters (same as default):
    * Standard Camera Model
    * 2 Coefficients for Radial Distortion
    * Skew and Tangential Distortion not computed
    * No OptimizationOptions specified.

## Sample scenes
### [prg_event_sim/AirSim/Unity/UnityDemo/](https://github.com/prgumd/prg_event_sim/tree/master/AirSim/Unity/UnityDemo)
Microsoft's [AirSim](https://github.com/microsoft/AirSim) with Unity's [ML-ImageSynthesis](https://bitbucket.org/Unity-Technologies/ml-imagesynthesis/src/master/) component. 

More specificially, AirSim provides an example car and drone scene for Unity. This example scene was modified to have an ImageSynthesis component integrated into it.

For AirSim build and usage instructions, follow [here](https://github.com/microsoft/AirSim/tree/master/Unity).

### [prg_event_sim/image-synthesis-Event-Signals/](https://github.com/prgumd/prg_event_sim/tree/master/image-synthesis-Event-Signals)
For ImageSynthesis implementation and usage instructions, follow [here](https://bitbucket.org/Unity-Technologies/ml-imagesynthesis/src/master/)
