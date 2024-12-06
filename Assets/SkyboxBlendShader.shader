Shader "Custom/SkyboxBlend" {
    Properties {
        _Cubemap1 ("Skybox 1", Cube) = "" {}
        _Cubemap2 ("Skybox 2", Cube) = "" {}
        _Blend ("Blend Factor", Range(0, 1)) = 0.0
    }
    SubShader {
        Tags {"Queue" = "Background"}
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 pos : POSITION;
                float3 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            samplerCUBE _Cubemap1;
            samplerCUBE _Cubemap2;
            float _Blend;

            v2f vert (appdata v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = normalize(mul((float3x3)unity_ObjectToWorld, v.vertex.xyz));
                
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                half4 color1 = texCUBE(_Cubemap1, i.uv);
                half4 color2 = texCUBE(_Cubemap2, i.uv);
                return lerp(color1, color2, _Blend);
            }
            ENDCG
        }
    }
}
