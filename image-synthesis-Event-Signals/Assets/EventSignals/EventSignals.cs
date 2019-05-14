using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.IO;


[RequireComponent (typeof(Camera))]
public class EventSignals : MonoBehaviour {

    private Material eventMaterial; // Wraps the shader
    private Shader eventShader;

    private Material storeFrameMaterial;
    private Shader storeFrameShader;

    private RenderTexture pastFrame;

    private int cameraWidth;
    private int cameraHeight;

    private float lastUpdateTime = 0;
    public float updateInterval = .1f; // The more frames per second, the more accurate this will be

    public float diffThreshold = 0;

    void Start() {
	eventShader = Shader.Find("Hidden/EventShader");
	eventMaterial = new Material(eventShader);
        
        storeFrameShader = Shader.Find("Hidden/StoreFrameShader");
        storeFrameMaterial = new Material(storeFrameShader);

        var mainCamera = GetComponent<Camera>();
        cameraWidth = mainCamera.pixelWidth;
        cameraHeight = mainCamera.pixelHeight;
	}

    // code based on https://answers.unity.com/questions/1345126/rendering-to-a-specific-texture2darray-slice.html
    void OnRenderImage (RenderTexture source, RenderTexture destination) {

        if (pastFrame == null) {
            pastFrame = new RenderTexture(cameraWidth, cameraHeight, 0);
            pastFrame.Create();
        }

        // Store the current frame as a texture called pastFrame
        // - But only if a certain time interval has passed 
        //   - so pastFrame will be 'behind' subsequent frames until pastFrame is updated again) 
        if (Time.time > lastUpdateTime + updateInterval) {
            lastUpdateTime = Time.time;
            
            // Setting the render target to be pastFrame
            Graphics.SetRenderTarget(pastFrame, 0, CubemapFace.Unknown);

            GL.PushMatrix();
            
            GL.LoadOrtho();
            
            storeFrameMaterial.SetTexture("_MainTex", source);
            storeFrameMaterial.SetPass( 0 ); // using storeFrameMaterial's shader to render pastFrame
            
            GL.Begin( GL.QUADS ); // Drawing four corners
            
            // For each corner: drawing a vertex and a texture coordinate
            // So pastFrame will be configured properly as a texture that can be mapped onto (?)
            GL.TexCoord2( 0, 0 );
            GL.Vertex3( 0, 0, 0 );
            
            GL.TexCoord2( 1, 0 );
            GL.Vertex3( 1, 0, 0 );
            
            GL.TexCoord2( 1, 1 );
            GL.Vertex3( 1, 1, 0 );
            
            GL.TexCoord2( 0, 1 );
            GL.Vertex3( 0, 1, 0 );
            
            GL.End();
            
            GL.PopMatrix();

        }
        
        // Finally, post-process the previous frame.
        eventMaterial.SetTexture("_PastFrame", pastFrame);
        eventMaterial.SetFloat("_DiffThreshold", diffThreshold);
        Graphics.Blit(source, destination, eventMaterial);
    }

}
