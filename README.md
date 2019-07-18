# prg_event_sim [In-Progress]

## Step 1: Gathering Data
The goal is to record every frame of a driving scene in Unity. CaptureFramesAndPose.cs is a Unity camera component that completes this task. For each frame, the time and pose of the camera is recorded. The intrinsic matrix is also recorded. Event-data is either simulated concurrently using the Unity component [Event Signals](/image-synthesis-Event-Signals/) or is simulated after frame collection using [rpg_davis_simulator](https://github.com/uzh-rpg/rpg_davis_simulator)

### CaptureFramesAndPose.cs 
#### There are two properties of the component: 'Folder' and 'Frame Rate'.
  * 'Folder' specifies the name of the folder that will contain the recorded data.
    * The folder will be located within the scene directory.
  * 'Frame Rate' specifies how many frames per second are to be recorded

#### The component is located in the Assets folder of the sample scenes
  * [prg_event_sim/image-synthesis-Event-Signals/Assets/CaptureFramesAndPose/CaptureFramesAndPose.cs](/image-synthesis-Event-Signals/Assets/CaptureFramesAndPose/CaptureFramesAndPose.cs)
  
### Calculation of the intrinsic matrix
 * Unity provides the focal length and sensor size of a camera in terms of milimeters. The focal length in terms of pixels was obtained from the following equation:
   * focal_pixel = (focal_mm / sensor_width_mm) * image_width_in_pixels

#### The intrinsic matrix obtained from Unity was validated using Matlab's [Single Camera Calibrator App](https://www.mathworks.com/help/vision/ug/single-camera-calibrator-app.html)
  * Matlab's app requires images which feature a certain checkerboard pattern. The size of each square must also be known.
  * Such images where obtained in a [Unity scene made for calibration](/calibration%20scene)
    * Two sets of images were obtained, where each set had different camera settings.
      * [First Folder](/calibration%20scene/CalibrationImages1)
      * [Second Folder](/calibration%20scene/CalibrationImages2)
      * Each folder contains the images, the camera settings, Matlab's estimation, and the intrinsic matrix from Unity.
    * A 7x10 checkerboard pattern was placed as a texture on a 3.5x5 meter cube in Unity. 
      * Thus, the size of each square is .5 meters.
  * App Parameters (same as default):
    * Standard Camera Model
    * 2 Coefficients for Radial Distortion
    * Skew and Tangential Distortion not computed
    * No OptimizationOptions specified.

## Sample scenes
### [https://github.com/microsoft/AirSim](https://github.com/microsoft/AirSim/)

AirSim provides an example car and drone scene for Unity.

It is a submodule of this repo. Go to **Cloning a Project with Submodules** in this tutorial [here](https://git-scm.com/book/en/v2/Git-Tools-Submodules) to get the actual contents of the AirSim directory after cloning the prg_event_sim repository

Build and usage instructions for the Unity Demo can be found [here](https://github.com/microsoft/AirSim/tree/master/Unity)


### [prg_event_sim/image-synthesis-Event-Signals/](https://github.com/prgumd/prg_event_sim/tree/master/image-synthesis-Event-Signals)
For ImageSynthesis implementation and usage instructions, follow [here](https://bitbucket.org/Unity-Technologies/ml-imagesynthesis/src/master/)

The sample scene provided is simply a skybox with some moving cubes. Note that the image synthesis component is not integral to this project.
