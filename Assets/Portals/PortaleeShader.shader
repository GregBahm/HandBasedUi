Shader "Custom/PortaleeShader" 
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader 
		{
			Pass
			{
				Stencil
				{
					Ref 1
					Comp equal
					Pass keep
					ZFail decrWrap
				}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float3 normal : NORMAL;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				v2f vert(appdata v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.normal = v.normal;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float3 color = lerp(float3(0, .5, 1), .75, i.normal.y / 2 + .5);
					return float4(color, 1);
				}
				ENDCG
		}
		Pass
		{
				Stencil
				{
					Comp always
					Pass keep
				}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				float3 _PortalNormal;
				float3 _PortalPoint;

				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float3 normal : NORMAL;
					float distToPlane : TEXCOORD0;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				float GetDistToPlane(float3 objPos)
				{


					float3 worldPos = mul(unity_ObjectToWorld, float4(objPos, 1));
					float theDot = dot(_PortalNormal, worldPos);
					float planeDot = dot(_PortalNormal, _PortalPoint);
					return planeDot - theDot;
				}

				v2f vert(appdata v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.normal = v.normal;
					o.distToPlane = GetDistToPlane(v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					clip(i.distToPlane);
					float3 color = lerp(float3(0, .5, 1), .75, i.normal.y / 2 + .5);
					float portalHighlight = 1 - i.distToPlane * 100;
					portalHighlight *= 0.2;
					color += saturate(portalHighlight);
					return float4(color, 1);
				}
				ENDCG
		}
	}
}
