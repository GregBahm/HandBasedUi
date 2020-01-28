Shader "Unlit/AnnotationMeshShader"
{
    Properties
    {
		_ColorAHigh("Color A High", Color) = (1,1,1,1)
		_ColorALow("Color A Low", Color) = (1,1,1,1)
		_ColorBHigh("Color B High", Color) = (1,1,1,1)
		_ColorBLow("Color B Low", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

		Pass
		{
			Cull Back
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
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float4 _ColorAHigh;
			float4 _ColorALow;

			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = v.normal;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float shade = dot(i.normal, float3(0, 0, -1));
				shade = shade * .5 + .5;
				return lerp(_ColorAHigh, _ColorALow, shade);
			}
			ENDCG
		}
		Pass
		{
			Cull Front
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
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float4 _ColorBHigh;
			float4 _ColorBLow;

			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = v.normal;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float shade = dot(i.normal, float3(0, 0, 1));
				shade = shade * .5 + .5;
				return lerp(_ColorBHigh, _ColorBLow, shade);
			}
			ENDCG
		}
    }
}
