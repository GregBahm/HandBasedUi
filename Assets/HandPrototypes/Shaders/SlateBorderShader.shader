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
            float _Length;

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


            fixed4 frag (v2f i) : SV_Target
            {

                float xAlpha = (i.uv.x * 300) % 1;
                xAlpha = abs(xAlpha - .5) * 2;
                xAlpha = (xAlpha - .5) * 4;
                float yAlpha = 1 - abs(i.uv.y - .5) * 2;
                float ret = yAlpha * xAlpha;

                float normalizedU = i.uv.x / _Length;
                float fadeEnd = pow(normalizedU, 1);
                float fadeStart = 1 - pow(normalizedU, 10);
                ret *= fadeEnd;
                ret *= fadeStart;
                ret *= _Highlight;
                return ret;
            }
            ENDCG
        }
    }
}
