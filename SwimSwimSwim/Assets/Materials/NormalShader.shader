Shader "Unlit/NormalShader"
{
	Properties
	{
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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float3 screenNormal : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenNormal = normalize(mul(UNITY_MATRIX_IT_MV, v.normal.xyzz).xyz);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 c = 0;
				// normal is a 3D vector with xyz components; in -1..1
				// range. To display it as color, bring the range into 0..1
				// and put into red, green, blue components
				c.rgb = i.screenNormal*0.5 + 0.5;
				return c;
			}
			ENDCG
		}
	}
}
