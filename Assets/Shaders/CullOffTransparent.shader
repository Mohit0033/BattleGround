Shader "Unlit/CullOffTransparent"
{
	Properties
	{
		_Tint("Tint",Color) = (1,1,1,1)
	}
		SubShader
	{
		Tags { "RenderType" = "Transparent" }
		LOD 100

		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


			#include "UnityCG.cginc"

			float4 _Tint;

			float4 vert (float4 pos : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(pos);
			}
			
			fixed4 frag (void) : Color
			{
				return _Tint;
			}
			ENDCG
		}
	}
}
