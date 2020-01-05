Shader "Unlit/SlateBorder"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100

		BlendOp Max
		//Blend OneMinusDstColor One
        ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Highlight;
			float _Fade;

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
                float scale : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                float4 vPosition = UnityObjectToClipPos(v.vertex);
                o.vertex = vPosition;
                o.uv = v.uv;
                o.scale = vPosition.w;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }

			float3 _LeftThumbTip;
			float3 _LeftIndexTip;
			float3 _RightThumbTip;
			float3 _RightIndexTip;

			float GetFingerContribution(float3 fingerPos, float3 worldPos)
			{
				float3 toFinger = fingerPos - worldPos;
				float dist = length(toFinger);
				float distShade = 1 - saturate(dist * .5);
				distShade = pow(distShade, 10);
				return distShade;
			}


			float GetFingerLighting(float3 worldPos)
			{
				float leftIndex = GetFingerContribution(_LeftIndexTip, worldPos);
				float rightIndex = GetFingerContribution(_RightIndexTip, worldPos);
				return leftIndex + rightIndex;
			}

            fixed4 frag (v2f i) : SV_Target
            {
				float fingerLighting = GetFingerLighting(i.worldPos);
                float alpha = abs(i.uv.y - .5) * 2;
                alpha = 1 - pow(alpha, 2);

                float ret = alpha / (i.scale * 5);
				ret *= fingerLighting * 2;
                ret *= 1 - _Highlight;
                return ret;
            }
            ENDCG
        }
    }
}
