Shader "Unlit/CursorShader"
{
    Properties
    {
		_StartColor("Start Color", Color) = (1,1,1,1)
		_EndColor("End Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100

		BlendOp Max
		ZTest Always
		ZWrite Off
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
            };

			float3 _CursorStart;
			float3 _CursorEnd;
			float4 _StartColor;
			float4 _EndColor;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

			float GetCursorParam(float3 worldPos)
			{
				float distToStart = length(worldPos - _CursorStart);
				float distToEnd = length(worldPos - _CursorEnd);
				return saturate(distToStart / (distToEnd));
			}

            fixed4 frag (v2f i) : SV_Target
            {
				float cursorParam = GetCursorParam(i.worldPos);
				return lerp(_StartColor, _EndColor, cursorParam);
            }
            ENDCG
        }
    }
}
