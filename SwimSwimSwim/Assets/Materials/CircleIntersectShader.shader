Shader "Unlit/CircleIntersectShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader{
			ColorMask 0
			ZWrite Off
			Cull Off

			Pass{
					Stencil{
						Ref 2
						Comp always
						Pass IncrSat
					}
				CGPROGRAM
				#include "UnityCG.cginc"
				#pragma vertex vert
				#pragma fragment frag
				struct appdata {
				float4 vertex : POSITION;
			};
			struct v2f {
				float4 pos : SV_POSITION;
			};
			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			half4 frag(v2f i) : SV_Target{
				return half4(1,1,0,1);
			}
				ENDCG
		}
	}
}
