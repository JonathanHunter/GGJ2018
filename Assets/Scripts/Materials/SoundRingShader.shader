Shader "Custom/SoundRingShader" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Tint("Tint", Color) = (1, 1, 1, .5)
		_NoiseTex("Extra Wave Noise", 2D) = "white" {}
	_Speed("Wave Speed", Range(0,1)) = 0.5
		_Amount("Wave Amount", Range(0,1)) = 0.5
		_Height("Wave Height", Range(0,1)) = 0.5
		_Foam("Foamline Thickness", Range(0,3)) = 0.5
	}
		SubShader{
		Tags{ "RenderType" = "Transparent" }
		LOD 100

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f {
		half2 uv : TEXCOORD0;
		UNITY_FOG_COORDS(2)
			float4 vertex : SV_POSITION;
		float4 scrPos : TEXCOORD1;//
	};

	sampler2D _MainTex, _NoiseTex;
	float4 _MainTex_ST;
	float _Speed, _Amount, _Height, _Foam;// 
	float4 _Tint;
	uniform sampler2D _CameraDepthTexture; //Depth Texture

	v2f vert(appdata_t v)
	{
		v2f o;
		float4 tex = tex2Dlod(_NoiseTex, float4(v.uv.xy, 0, 0));//extra noise tex
		v.vertex.y += sin(_Time.z * _Speed + (v.vertex.x * v.vertex.z * _Amount * tex)) * _Height;//movement
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		o.scrPos = ComputeScreenPos(o.vertex); // grab position on screen
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		half4 col = tex2D(_MainTex, i.uv) * _Tint;// texture times tint;
		half depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos))); // depth
		half4 foamLine = 1 - saturate(_Foam * (depth - i.scrPos.w));// foam line by comparing depth and screenposition
		col += foamLine * _Tint; // add the foam line and tint to the texture
		UNITY_APPLY_FOG(i.fogCoord, col);
		UNITY_OPAQUE_ALPHA(col.a);
		return col;
	}
		ENDCG
	}
	}

}
