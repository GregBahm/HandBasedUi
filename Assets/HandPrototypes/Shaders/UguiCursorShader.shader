Shader "Unlit/UguiCursorShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0

		_Progression("Progression", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				float3 actualWorldPosition : TEXCOORD2;
				float3 worldNormal : NORMAL;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float4 _MainTex_ST;
			float3 _CursorPos;
			float3 _PulsePos;
			float _Progression;

			v2f vert(appdata_t v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = v.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				OUT.actualWorldPosition = mul(unity_ObjectToWorld, v.vertex);
				OUT.worldNormal = mul(unity_ObjectToWorld, v.normal);
				OUT.color = v.color * _Color;
				return OUT;
			}

#define CursorMaxSize 10
#define CursorMinSize 400

			float GetDiscAlpha(float baseCursor, float cursorSize)
			{
				float alpha = 1 - (baseCursor * cursorSize);
				alpha = saturate(alpha * 10);
				return alpha;
			}

			float GetPulseEffect(float3 worldPos)
			{
				float toCursor = length(_PulsePos - worldPos);
				toCursor = pow(toCursor * 50, .5);
				float ring = abs(toCursor - _Progression);
				float outerDisc = saturate(ring * 20);
				outerDisc = 1 - saturate(outerDisc);
				outerDisc = saturate(outerDisc * 2);
				outerDisc *= 1 - _Progression;
				return outerDisc;
			}

			float GetCursorEffect(float3 norm, float3 worldPos)
			{
				float3 toCursor = _CursorPos - worldPos;

				float3 toCursorNormal = normalize(toCursor);
				float baseCursor = length(cross(norm, toCursor));
				
				float baseDist = length(norm * toCursor);

				float dotToCursor = dot(norm, toCursor);
				float alpha;
				float cursorSizeLerp;
				if (dotToCursor > 0)
				{
					cursorSizeLerp = saturate(baseDist * 2);
					float cursorSize = lerp(CursorMinSize, CursorMaxSize, cursorSizeLerp);

					alpha = GetDiscAlpha(baseCursor, cursorSize);
				}
				else
				{
					cursorSizeLerp = saturate(baseDist * 10);
					float cursorSize = lerp(CursorMinSize, CursorMaxSize, cursorSizeLerp);
					float innerCursorSize = lerp(1000, 10, cursorSizeLerp * 1.5);

					alpha = GetDiscAlpha(baseCursor, cursorSize);
					float innerAlpha = GetDiscAlpha(baseCursor, innerCursorSize);
					alpha = alpha - innerAlpha;
				}

				float col = (1 - cursorSizeLerp) * .5;
				return saturate(alpha) * col;

			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

				#ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				#endif

				#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
				#endif

				float3 norm = normalize(IN.worldNormal);
				float cursorEffect = GetCursorEffect(norm, IN.actualWorldPosition);
				float pulseEffect = GetPulseEffect(IN.actualWorldPosition);
				float finalEffect = max(cursorEffect, pulseEffect);
				color = lerp(color, 1, finalEffect);
				return color;
			}
		ENDCG
		}
	}
}
