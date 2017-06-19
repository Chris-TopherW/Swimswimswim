Shader "Unlit/Trash"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_GlitchTex("Glitch Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_GlitchAmount("Glitch Amount", Range(0.0, 1.0)) = 0.0
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
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 muv : TEXCOORD0;
				float2 guv : TEXCOORD1;
				float2 nuv : TEXCOORD2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _GlitchTex;
			sampler2D _NoiseTex;
			float _GlitchAmount;
			float4 _MainTex_ST;
			float4 _GlitchTex_ST;
			float4 _NoiseTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.muv = TRANSFORM_TEX(v.uv, _MainTex);
				o.guv = TRANSFORM_TEX(v.uv, _GlitchTex);
				o.nuv = TRANSFORM_TEX(v.uv, _NoiseTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 colMain = tex2D(_MainTex, i.muv);
				fixed4 colGlitch = tex2D(_GlitchTex, i.guv);
				float cutoff = tex2D(_NoiseTex, i.nuv).r;
				fixed4 col;
				if (cutoff < _GlitchAmount)
				{
					col = colGlitch;
					//discard;
				}
				else
				{
					col = colMain;
				}
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
