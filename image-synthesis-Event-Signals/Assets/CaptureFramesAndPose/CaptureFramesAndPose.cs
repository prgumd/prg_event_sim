using UnityEngine;
using System.Collections;
using System.IO;

/* 
    For every frame:
    1. create an EXR file
    2. record pose
    3. record current frame and associated EXR file
*/
[RequireComponent (typeof(Camera))]
public class CaptureFramesAndPose : MonoBehaviour {

    public string folder = "FramesAndPose";
    public int frameRate = 1000;
    public float xDimPixelsToDistance = 1;
    public float yDimPixelsToDistance = 1;
    public enum OnSwitch {On, Off};
    public OnSwitch captureOn = OnSwitch.On;

    private string folderPath;
    private Camera cam;
    private int width;
    private int height;
    private StreamWriter matrixWriter;
    private StreamWriter poseWriter;
    private StreamWriter timeToFileWriter;

    void Start()
    {
        if (captureOn == OnSwitch.On) {
            cam = GetComponent<Camera>();
            cam.usePhysicalProperties = true;

            width = cam.pixelWidth;
            height = cam.pixelHeight;

            // Set the playback framerate (real time will not relate to game time after this).
            Time.captureFramerate = frameRate;

            // Create the folder
            System.IO.Directory.CreateDirectory(folder);
            // Create the subfolder containing the exr files 
            System.IO.Directory.CreateDirectory(folder + "/Frames");

            // Application.dataPath contains the path to Assets
            folderPath = Application.dataPath + "/../" + folder; 

            // For writing intrinsic matrix
            matrixWriter = new StreamWriter(folderPath + "/k-matrix.txt", false);
            writeIntrinsicMatrix();
            matrixWriter.Close();

            // The following streams are closed on application quit
            // ----------------------------------------------------
            // For writing pose for each frame
            poseWriter = new StreamWriter(folderPath + "/pose.txt", false);

            // For writing time to file map for frame
            timeToFileWriter = new StreamWriter(folderPath + "/time-to-file-map.txt", false);
        }
    }

    public void writeIntrinsicMatrix() {
        float focalLength = cam.focalLength;
        float xFocalLengthInPixels = xDimPixelsToDistance * focalLength;
        float yFocalLengthInPixels = yDimPixelsToDistance * focalLength;

        Vector2 lensShift = cam.lensShift; // should be in center of screen
        float centerX = (width/2) + lensShift[0];
        float centerY = (height/2) + lensShift[1];

        // Assuming skew coefficient is 0
        int skewCoefficient = 0;

        matrixWriter.WriteLine(xFocalLengthInPixels + " " + skewCoefficient + " " + centerX);
        matrixWriter.WriteLine(0 + " " + yFocalLengthInPixels + " " + centerY);
        matrixWriter.WriteLine(0 + " " + 0 + " " + 1);
        matrixWriter.WriteLine("");

        // Writing projection matrix for testing purposes
        Matrix4x4 projectionMatrix = cam.projectionMatrix;
        string[] rows = new string[4];
        for (int i=0; i<4; i++) {
            for (int j=0; j<4; j++) {
                rows[i] += projectionMatrix[i, j].ToString("F") + " ";
            }
        }
        for (int i=0; i<rows.Length; i++) {
            matrixWriter.WriteLine(rows[i]);
        }
    }

    IEnumerator RecordFrame()
    {
        yield return new WaitForEndOfFrame();

        writeExrFile();
        writeCameraPose();
        writeTimeToFileMap();

    }

    public void LateUpdate()
    {
        if (captureOn == OnSwitch.On) {
            StartCoroutine(RecordFrame());
        }
    }

    private void writeExrFile() {
        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        
        // Need to convert the texture from the screenshot into the format needed for EncodeToEXR()
        Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
        newTexture.SetPixels(texture.GetPixels());
        newTexture.Apply();

        byte[] bytes = newTexture.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
        string fileName = string.Format("{0}/Frames/{1:D04}.exr", folderPath, Time.frameCount);
        File.WriteAllBytes(fileName, bytes);

        Object.DestroyImmediate(texture);
        Object.DestroyImmediate(newTexture);
    }

    private void writeCameraPose() {
        Vector3 position = cam.transform.position;
        Quaternion rotation = cam.transform.rotation;

        poseWriter.WriteLine(position[0] + " " + position[1] + " " + position[2] + " " + 
            rotation[0] + " " + rotation[1] + " " + rotation[2] + " " + rotation[3]);
    }

    private void writeTimeToFileMap() {
        float secondsPassed = (float)Time.frameCount / 1000;
        string line = string.Format("{0} {1:D04}.exr", secondsPassed, Time.frameCount);
        timeToFileWriter.WriteLine(line);
    }

    void OnApplicationQuit()
    {
        if (captureOn == OnSwitch.On) {
            poseWriter.Close();
            timeToFileWriter.Close();
        }
    }

}