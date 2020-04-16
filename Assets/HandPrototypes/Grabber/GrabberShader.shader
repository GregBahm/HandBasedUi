Shader "Unlit/GrabberShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100

		ZWrite Off
		Blend One One
		//Cull Off
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
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 objPos : TEXCOORD1;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float3 viewDir : TEXCOORD2;
				UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.objPos = v.vertex;
				o.normal = v.normal;
				o.color = v.color;
				o.viewDir = ObjSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float fresnel = dot(normalize(i.viewDir), normalize(i.normal));
				fresnel = 1 - abs(fresnel);
				fresnel = pow(fresnel, 1) * .5;
				return fresnel;
            }
            ENDCG
        }
    }
}
