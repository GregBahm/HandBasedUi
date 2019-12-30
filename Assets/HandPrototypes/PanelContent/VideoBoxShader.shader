Shader "Unlit/VideoBoxShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_BorderThickness("Border", Range(0, 1)) = .5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
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
				float3 normal : NORMAL;
				float3 objSpace : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
			float _BorderThickness;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.normal = v.normal;
				o.objSpace = v.vertex;
				o.worldNormal = normalize(mul(unity_ObjectToWorld, v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float xDist = 1 - abs(i.uv.x - .5) * 2;
				float yDist = 1 - abs(i.uv.y - .5) * 2;
				xDist *= 1.666;
				float dist = min(xDist, yDist);
				dist -= _BorderThickness;
				dist *= 200;
				float borderAlpha = 1 - saturate(dist);
				borderAlpha = lerp(1, borderAlpha, saturate(-i.normal.z));

                fixed4 cardTex = tex2D(_MainTex, i.objSpace.xy + .5);
				float4 sideCol = dot(i.worldNormal, float3(0, 1, 0)) * .5 + .5;
				sideCol = lerp(sideCol, i.objSpace.y + 1, .5);
				sideCol = lerp(sideCol, i.objSpace.y + 1, .5);
				sideCol = pow(sideCol, .5);
				//return sideCol;
				//sideCol += (1 - (i.objSpace.z + .5)) * .5;
				float4 ret = lerp(cardTex, sideCol, borderAlpha);
                return ret;
            }
            ENDCG
        }
    }
}
