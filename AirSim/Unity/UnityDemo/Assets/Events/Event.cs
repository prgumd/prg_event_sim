using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.IO;


[RequireComponent (typeof(Camera))]
public class Event : MonoBehaviour {

    private Material eventMaterial; // Wraps the shader

    public Shader eventShader;

    private RenderTexture pastFrame;

    private RenderTexture eventFrame;

    private Camera mainCamera;

    private Camera eventCamera;

    private float lastUpdateTime = 0;

    private float updateInterval = .1f;

    private Material storeFrameMaterial;

    private Shader storeFrameShader;

    void Start() {
		eventShader = Shader.Find("Hidden/Event");
		eventMaterial = new Material(eventShader);
        
        mainCamera = GetComponent<Camera>();
        
        storeFrameShader = Shader.Find("Hidden/StoreFrameShader");
        storeFrameMaterial = new Material(storeFrameShader);
	}

    // code based on https://answers.unity.com/questions/1345126/rendering-to-a-specific-texture2darray-slice.html
    void OnRenderImage (RenderTexture source, RenderTexture destination) {

        if (pastFrame == null) {
            pastFrame = new RenderTexture(mainCamera.pixelWidth, mainCamera.pixelHeight, 0);
            pastFrame.Create();
        }

        if (Time.time > lastUpdateTime + updateInterval ) {
            lastUpdateTime = Time.time;
            // Setting the render target to the pastFrame
            Graphics.SetRenderTarget( pastFrame, 0, CubemapFace.Unknown);

            GL.PushMatrix();
            GL.LoadOrtho();
            storeFrameMaterial.SetTexture( "_CamTex", source );
            storeFrameMaterial.SetPass( 0 );
            GL.Begin( GL.QUADS );
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
        eventMaterial.SetTexture( "_PastFrame", pastFrame );
        Graphics.Blit(source, destination, eventMaterial); // 'source' not used directly
    }

}