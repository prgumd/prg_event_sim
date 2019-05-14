Shader "Hidden/Event"
{
	Properties
	{
		_MainTex ("Current Frame", 2D) = "white" { }
		_PastFrame ("Past Frame", 2D) = "black" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
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
                 float2 uv       : TEXCOORD0;
             };

			sampler2D _MainTex;
			sampler2D _PastFrame;

             vertOutput vert ( vertInput v )
             {
                 vertOutput o;
                 o.screenPos = UnityObjectToClipPos( v.vertex );
                 o.uv = v.uv;
                 return o;
             }

			fixed4 frag (vertOutput i) : COLOR 
			{
                float4 curFrame = tex2D(_MainTex, i.uv);
                float4 pastFrame = tex2D(_PastFrame, i.uv);
				float4 diff = abs(curFrame - pastFrame);
				float avgDiff = (diff.r + diff.g + diff.b) / 4; 
				
				float4 ret;

				if (avgDiff > .1) {
					ret = float4(1, 1, 1, 1);
				} else {
					ret = float4(0, 0, 0, 0);
				}
 
                return ret;
			}
			ENDCG
		}
	}
}
