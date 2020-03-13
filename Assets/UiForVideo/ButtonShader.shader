Shader "Unlit/ButtonShader"
{
    Properties
    {
		_Color("Color", Color) = (.5, .5, .5, 1)
    }
    SubShader
    {
		Blend SrcAlpha OneMinusSrcAlpha
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
				float3 worldNormal : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
            };

			float4 _Color;
			float _Disabledness;
			float3 _FingerPosition;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = mul(unity_ObjectToWorld, v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 toFinger = i.worldPos - _FingerPosition;
				float dist = length(toFinger);
				float distAlpha = 1 - dist * 5;

				float dotToFinger = dot(normalize(i.worldNormal), normalize(toFinger));
				float dotShade = pow(saturate(-dotToFinger), 100);
				float antiShade = pow(saturate(dotToFinger), 100);
				float4 antiShadeColor = antiShade * float4(1, .5, .5, 1);
				float4 fingerShade = dotShade + antiShadeColor;
				fingerShade *= 1 - _Disabledness;
				float4 ret = _Color;
				ret += fingerShade * .2 * distAlpha;
				return ret;
            }
            ENDCG
        }
    }
}
