// Standard shader with stipple transparency by Alex Ocias - https://ocias.com
// based on an article by Digital Rune: https://www.digitalrune.com/Blog/Post/1743/Screen-Door-Transparency
Shader "Unlit/shaderOutline&Stipple" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_OutlineVolume ("Outline Volume", Float) = 0.0
		_OutlineColor ("Outline Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Back

		Pass{
			Name "Outline"
			Cull Front
			Tags { "LightMode"="Always" "IgnoreProjector"="True" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile __ OUTLINE
			#include "UnityCG.cginc"

			struct appdata{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f{
				float4 position : SV_POSITION;
			};

			float _OutlineVolume;
			fixed4 _OutlineColor;

			v2f vert(appdata parAD){
				v2f tempResult;
				tempResult.position = UnityObjectToClipPos(parAD.vertex);
				#if defined(OUTLINE)
					// 법선 벡터 >>> 클립 공간 벡터 >>> 클립 공간 표준화 벡터 >>> 화면 공간 표준화 벡터 >>> 두께일정/깊이무시 화면 공간 표준화 벡터
					float3 tempClipNormal = mul((float3x3)UNITY_MATRIX_MVP, parAD.normal).xyz;
					float2 tempOffset = normalize(tempClipNormal.xy) / _ScreenParams.xy * _OutlineVolume * parAD.vertex.w;

					tempResult.position.xy += tempOffset;
				#endif
				return tempResult;
			}

			fixed4 frag(v2f parV) : SV_TARGET{
				#if defined(OUTLINE)
					return _OutlineColor;
				#else
					return fixed4(1,1,1,1);
				#endif
			}
			ENDCG
		}
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			// Screen-door transparency: Discard pixel if below threshold.
			float4x4 thresholdMatrix =
			{  1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
			  13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
			   4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
			  16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
			};
			float4x4 _RowAccess = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
			float2 pos = IN.screenPos.xy / IN.screenPos.w;
			pos *= _ScreenParams.xy; // pixel position
				clip(c.a - thresholdMatrix[fmod(pos.x, 4)] * _RowAccess[fmod(pos.y, 4)]);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
