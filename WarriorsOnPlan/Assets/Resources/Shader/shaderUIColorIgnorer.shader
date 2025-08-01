Shader "UI/shaderUIColorIgnorer"
{
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass{
            Blend SrcAlpha OneMinusSrcAlpha
            Cull off
            ZWrite off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t{
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f{
                float4 vertex : SV_POSITION;    
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            v2f vert (appdata_t parAppdata){
                    v2f tempResult;
                    tempResult.vertex = UnityObjectToClipPos(parAppdata.vertex);
                    tempResult.uv = parAppdata.texcoord;
                    return tempResult;
            }

            float4 frag(v2f parV) : SV_Target{
                fixed4 tempTexColorOriginal = tex2D(_MainTex, parV.uv);
                fixed3 tempTexColorWeak = lerp(fixed3(1,1,1), tempTexColorOriginal.rgb, 0.5);
                fixed4 tempTexColorFinal = fixed4(tempTexColorWeak, tempTexColorOriginal.a);
                fixed4 tempResult = tempTexColorFinal * _Color;
                return tempResult;
            }
            ENDCG
        }
    }
}
