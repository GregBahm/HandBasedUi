Shader "Unlit/TetherShader"
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

			float _Highlight;

            fixed4 frag (v2f i) : SV_Target
            {
                float alpha = abs(i.uv.y - .5) * 2;
                alpha = 1 - pow(alpha, 2);
				float dottedAlpha = (i.uv.x * 5) % 1;
				dottedAlpha = abs(dottedAlpha - .5) * 2;
				dottedAlpha = 1 - dottedAlpha;
				dottedAlpha = dottedAlpha > .5;
				alpha *= dottedAlpha;
				alpha *= 1 - pow(1 - i.uv.x, 4);
				alpha *= _Highlight;
				return alpha;
            }
            ENDCG
        }
    }
}
