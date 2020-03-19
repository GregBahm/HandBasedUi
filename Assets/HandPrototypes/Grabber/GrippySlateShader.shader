Shader "Unlit/GrippySlateShader"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
			ZWrite Off
			Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float3 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 color : COLOR;
				float3 viewDir : TEXCOORD1;
				float3 objSpace : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				UNITY_VERTEX_OUTPUT_STEREO
            };

			float4 _Color;
			float3 _GripPos;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.normal = v.normal;
				o.viewDir = ObjSpaceViewDir(v.vertex);
				o.objSpace = v.vertex;
				o.color = v.color;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float fresnel = dot(normalize(i.viewDir), normalize(i.normal));
				fresnel = 1 - fresnel;
				//fresnel = fresnel / 2 + .5;
				float dist = length(i.worldPos - _GripPos);
				dist = saturate(1 - dist);
				dist = pow(dist, 4);
				float colorPow = pow(i.color.x, 2);
				//colorPow *= dist;
				float shade = (colorPow * fresnel + colorPow) * dist;
				shade += dist;
				return _Color * shade;
            }
            ENDCG
        }
    }
}
