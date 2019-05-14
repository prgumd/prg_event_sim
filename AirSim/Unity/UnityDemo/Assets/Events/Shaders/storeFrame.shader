// from https://answers.unity.com/questions/1403485/how-would-you-efficiently-access-the-last-n-render.html

Shader "Hidden/StoreFrameShader"
 {
     Properties
     {
         _CamTex ( "CamTex", 2D ) = "white" {}
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
             sampler2D _CamTex;
             vertOutput vert ( vertInput v )
             {
                 vertOutput o;
                 o.screenPos = UnityObjectToClipPos( v.vertex );
                 o.uvs = v.uv;
                 return o;
             }
             fixed4 frag ( vertOutput i ) : SV_Target
             {
                 fixed4 col = tex2D( _CamTex, i.uvs );
                 return col;
             }
             ENDCG
         }
     }
 }