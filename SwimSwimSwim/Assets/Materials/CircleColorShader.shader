Shader "Unlit/CircleColorShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass{
			Stencil{
				Ref 1
				Comp equal
			}

			CGPROGRAM
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
				return half4(1,0,0,0.5);
			}
			ENDCG
		}
		Pass{
				Stencil{
				Ref 2
				Comp equal
			}

				CGPROGRAM
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
				return half4(0,1,0,0.7);
			}
				ENDCG
			}
				Pass{
				Stencil{
				Ref 3
				Comp equal
			}

				CGPROGRAM
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
				return half4(0,0,1,0.8);
			}
				ENDCG
			}
				Pass{
				Stencil{
				Ref 4
				Comp equal
			}

				CGPROGRAM
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
				return half4(0,1,1,0.8);
			}
				ENDCG
			}

	}
}
