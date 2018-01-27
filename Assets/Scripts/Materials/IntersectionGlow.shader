// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Intersection Glow"
{
	Properties
	{
		_Color ("Color", Color) = (1,0,0,1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100
		Blend One One // additive blending for a simple "glow" effect
		Cull Off // render backfaces as well
		ZWrite Off // don't write into the Z-buffer, this effect shouldn't block objects
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 screenPos : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _CameraDepthTexture; // automatically set up by Unity. Contains the scene's depth buffer
			fixed4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float screenDepth = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture, i.screenuv).zw);
			float diff = screenDepth - i.depth;
			float intersect = 0;

			if (diff > 0) 
			{
				interact = 1 - smoothstep(0, _ProjectParams.w, diff);
			}

			fixed4 col = _Col * _Color.a + intersect;
			return col;

			}
			ENDCG
		}
	}
}
