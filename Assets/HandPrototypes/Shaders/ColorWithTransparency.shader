Shader "Unlit/ColorWithTransparency"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
            };
			fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 edgeDist = abs(i.uv - .5) * 2;
				edgeDist = pow(edgeDist, 50);
				float edgeAlpha = 1 - saturate(edgeDist.x + edgeDist.y);
				return float4(_Color.rgb, _Color.a * edgeAlpha);
            }
            ENDCG
        }
    }
}
