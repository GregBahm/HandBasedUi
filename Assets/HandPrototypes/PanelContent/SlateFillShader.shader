Shader "Unlit/SlateFillShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100

        Cull Off
        ZWrite Off
        Blend One One
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : NORMAL;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float3 _LeftThumbTip;
            float3 _LeftIndexTip;
            float3 _RightThumbTip;
            float3 _RightIndexTip;

            float _Grabbedness;

            float GetFingerContribution(float3 fingerPos, float3 worldPos, float3 normal)
            {
                float3 toFinger = fingerPos - worldPos;
                float dist = length(toFinger);
                float theDot = dot(normalize(toFinger), normal);
                theDot = saturate(abs(theDot));
                float distShade = 1 - saturate(dist * .5);
                distShade = pow(distShade, 10);
                float ret = theDot * distShade;
                return ret;
            }

            float GetFingerLighting(float3 worldPos, float3 normal)
            {
                float leftIndex = GetFingerContribution(_LeftIndexTip, worldPos, normal);
                float rightIndex = GetFingerContribution(_RightIndexTip, worldPos, normal);
                return leftIndex + rightIndex;
            }

            float GetGrabFill(float2 uvs)
            {
                float2 dists = abs(uvs);
                dists = pow(dists, 6);
                float ret = dists.x + dists.y;
                ret *= 20;
                return ret;
            }

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                float4 vPosition = UnityObjectToClipPos(v.vertex); 
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldNormal = normalize(mul(unity_ObjectToWorld, v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float fingerLighting = GetFingerLighting(i.worldPos, i.worldNormal);
                float baseFill = fingerLighting * .4;
                float grabFill = GetGrabFill(i.uv);
                float ret = lerp(baseFill, grabFill, _Grabbedness);
                return ret;
            }
            ENDCG
        }
    }
}
