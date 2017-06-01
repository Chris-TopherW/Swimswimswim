// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/CoolImageCutoutShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(.002, 0.5)) = .005
		_Tiling("Tiling", Vector) = (0,0,0)
	}
		CGINCLUDE
#include "UnityCG.cginc"

		float _Outline;
		float4 _OutlineColor;

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float3 normal : NORMAL;

		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
			float4 screenpos : TEXCOORD1;
			float4 color : COLOR;
		};

		ENDCG

			SubShader{
			Tags{ "Queue" = "Transparent" }

			// note that a vertex shader is specified here but its using the one above
		Pass{
			Name "OUTLINE"
			Tags{ "LightMode" = "Always" }
			Cull Off
			ZWrite Off
			ZTest Always
			ColorMask RGB // alpha not used

						  // you can choose what kind of blending mode you want for the outline
			Blend SrcAlpha OneMinusSrcAlpha // Normal
											//Blend One One // Additive
											//Blend One OneMinusDstColor // Soft Additive
											//Blend DstColor Zero // Multiplicative
											//Blend DstColor SrcColor // 2x Multiplicative

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			v2f vert(appdata v) {
			// just make a copy of incoming vertex data but scaled according to normal direction
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);

			float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
			float2 offset = TransformViewToProjection(norm.xz);

			o.vertex.xy += offset * _Outline;
			o.color = _OutlineColor;
			return o;
		}

			half4 frag(v2f i) :COLOR{
			return i.color;
		}
			ENDCG
		}
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog


				float2 _Tiling;


			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.screenpos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float2 pos = i.screenpos / i.screenpos.w;
				fixed4 col = tex2D(_MainTex, pos * _Tiling);
				return col;
			}
			ENDCG
		}
	}
}
