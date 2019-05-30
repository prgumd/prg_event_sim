using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent (typeof(Camera))]
public class ScreenShotter : MonoBehaviour {

	private Camera cam;
	private int width;
	private int height;
	private int imageCounter = 1;
	private Rect rect;
	private RenderTexture renderTexture;
	private Texture2D screenshot;

    void Start()
    {
			cam = GetComponent<Camera>();
			cam.usePhysicalProperties = true;

			width = cam.pixelWidth;
			height = cam.pixelHeight;

			// Create the folder
			System.IO.Directory.CreateDirectory("Calibration");
    }

		void OnGUI ()
		{
			if (GUILayout.Button("Captcha!!! (" + imageCounter + ")")) {
				if (renderTexture == null) {
					// creates off-screen render texture that can rendered into
					rect = new Rect(0, 0, width, height);
					renderTexture = new RenderTexture(width, height, 24);
					screenshot = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
				}

				cam.targetTexture = renderTexture;
				cam.Render();
				RenderTexture.active = renderTexture;
				screenshot.ReadPixels(rect, 0, 0);

				cam.targetTexture = null;
				RenderTexture.active = null;

				System.IO.File.WriteAllBytes(Application.dataPath + "/../Calibration/" + (imageCounter++) + ".png", screenshot.EncodeToPNG());
			}
	}
}
