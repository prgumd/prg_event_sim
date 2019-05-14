// code based on https://answers.unity.com/questions/1403485/how-would-you-efficiently-access-the-last-n-render.html
// Simply returns current frame as a texture

Shader "Hidden/StoreFrameShader"
 {
     Properties
     {
        _MainTex ("Current Frame", 2D ) = "white" {}
     }
     SubShader
     {
         // No culling or depth
         Cull Off ZWrite Off ZTest Always
         Pass
         {
             CGPROGRAM
             #pragma vertex   vert
             #pragma fragment frag
             #include "UnityCG.cginc"
             
             struct vertInput
             {
                 float4 vertex : POSITION;
                 float2 uv     : TEXCOORD0;
             };
             
             struct vertOutput
             {
                 float4 screenPos : SV_POSITION;
                 float2 uvs       : TEXCOORD0;
             };
             
             sampler2D _MainTex; // the current frame
             
             vertOutput vert ( vertInput v ) {
                 vertOutput o;

                 // Transforms a point from object space to the camera’s clip space in homogeneous coordinates.
                 o.screenPos = UnityObjectToClipPos( v.vertex ); 
                 o.uvs = v.uv;
                 
                 return o;
             }

             fixed4 frag ( vertOutput i ) : SV_Target {
                 fixed4 col = tex2D( _MainTex, i.uvs ); // getting color value based on texture coordinates
                 return col;
             }
             ENDCG
         }
     }
 }