Shader "Unlit/GripperRingShader"
{
    Properties
    {
        _Shine("Test", Range(-1, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        ZWrite Off
        BlendOp Max

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD2;
                float scale : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            }; 

            float _Shine;
            float _Fade;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.uv = v.uv;
                o.viewDir = ObjSpaceViewDir(v.vertex);
                o.scale = o.vertex.w;
                return o;
            }

#define LowVal .2

            fixed4 frag(v2f i) : SV_Target
            {
                float baseAlpha = 1 - (abs(i.uv.y - .5) * 2);

                float flatVal = (_Shine > 0) ? 1 : LowVal;

                float perpendicularVal = 1 - abs(i.uv.x - .5) * 2;
                perpendicularVal = lerp(LowVal, 1, pow(1 - perpendicularVal, 3));

                float perpendicularity = abs(_Shine);
                float ret = lerp(perpendicularVal, flatVal, perpendicularity);
                ret *= baseAlpha;
                ret *= _Fade;
                return ret;
            }
            ENDCG
        }
    }
}
