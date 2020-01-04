Shader "Unlit/SphereButtonShader"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
		_ShineRamp("Shine Ramp", Float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

		Pass
		{
			Cull Off

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
				float3 worldNormal : NORMAL;
				float3 objSpace : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float4 _Color;

			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				if (v.vertex.z > .2)
				{
					v.vertex.z = .2;
				}

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.worldNormal = v.normal;
				o.objSpace = v.vertex;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				clip(i.objSpace.z);
				float shade = i.objSpace.y + 1;
				shade = lerp(shade, 1, .5);
				return _Color * shade;
			}
			ENDCG
		}
		/*
        Pass
        {
			ZWrite Off
			Blend OneMinusDstColor One
			Cull Off

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
				float3 worldNormal : NORMAL;
				float3 objSpace : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
				UNITY_VERTEX_OUTPUT_STEREO
            };

			float4 _Color;
			float _ShineRamp;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.worldNormal = mul(unity_ObjectToWorld, v.normal);
				o.objSpace = v.vertex;
				o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				i.viewDir = normalize(i.viewDir);
				float3 norm = normalize(i.worldNormal);
				float3 light = normalize(float3(0, 1, 0));
				float3 reflected = normalize(light + i.viewDir);
				float theDot = dot(norm, reflected);
				theDot = theDot * .5 + .5;
				theDot = pow(theDot, _ShineRamp);
				theDot = 1 - theDot;
				return theDot * _Color;
            }
            ENDCG
        }
		*/
    }
}
