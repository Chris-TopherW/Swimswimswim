//TODO: Tweak fadeout on explode
	//	Remove stencil and engineer better collision detection with grid..

Shader "Unlit/Trash"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_GlitchTex("Glitch Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_GlitchAmount("Glitch Amount", Range(0.0, 1.0)) = 0.0
		_ExplodeAmount("Explode Amount", Range(0.0, 2.0)) = 0.0
		_ExplodeFadeOffset("Explode FadeOffset", Range(0.0, 2.0)) = 0.0
		_ExplodeScale("ExplodeScale", Range(0.0,100.0)) = 10.0
		_ScreenPrams("Screen Resolution", Vector) = (0,0,0,0)
		_Transparency("Transparency", Range(0.0, 1.0)) = 0.0
	}
	SubShader
	{
		Cull Off
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass{
			Stencil{
			Ref 1
			Comp equal
			}


			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 muv : TEXCOORD0;
				float2 guv : TEXCOORD1;
				float2 nuv : TEXCOORD2;
				float3 normal : NORMAL;
				UNITY_FOG_COORDS(1)
				float3 worldPosition : TEXCOORD3;
				float4 vertex : SV_POSITION;
				float4 screenPos : COLOR;
				float explAmount : RANDO;
			};

			uniform float4 _ScreenPrams;
			sampler2D _MainTex;
			sampler2D _GlitchTex;
			sampler2D _NoiseTex;
			float _GlitchAmount;
			float _ExplodeAmount;
			float _ExplodeScale;
			float _Transparency;
			float _ExplodeFadeOffset;
			float4 _MainTex_ST;
			float4 _GlitchTex_ST;
			float4 _NoiseTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = v.vertex;
				o.normal = v.normal;
				o.muv = TRANSFORM_TEX(v.uv, _MainTex);
				o.guv = TRANSFORM_TEX(v.uv, _GlitchTex);
				o.nuv = TRANSFORM_TEX(v.uv, _NoiseTex);
				o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.screenPos = ComputeScreenPos(o.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.explAmount = 0;
				return o;
			}

			[maxvertexcount(6)]
			void geom(triangle v2f input[3], inout TriangleStream<v2f> OutputStream)
			{
				v2f test = (v2f)0;
				float3 pos = mul(unity_ObjectToWorld, (input[0].vertex.xyz + input[1].vertex.xyz + input[2].vertex.xyz)/3.0);
				float3 dir = normalize(pos);
				dir.y += 2;
				dir.y -= _ExplodeAmount *  _ExplodeAmount * 2;
				float3 worldPos = (input[0].worldPosition.xyz + input[1].worldPosition.xyz + input[2].worldPosition.xyz) / 3.0;
				float explodeAmount = clamp((_ExplodeAmount - tex2Dlod(_NoiseTex, float4(input[0].nuv, 0, 0)).r), 0, 2);
				float explodeFinal = (explodeAmount * _ExplodeScale);
				if ((worldPos + float3(dir* explodeFinal)).y <0) {
				//	dir.x *= 0.4;
				//	dir.z *= 0.4;
				}
				for (int i = 0; i < 3; i++)
				{
					test.normal = input[i].normal;
					float3 vert = input[i].worldPosition + float3(dir* explodeFinal);
					if (vert.y < 0)
					{
						vert.y = vert.y * 0.001f;
					}
					test.vertex = mul(UNITY_MATRIX_VP, float4(vert,1.0));
					test.explAmount = explodeAmount;
					test.screenPos = ComputeScreenPos(test.vertex);
					test.muv = input[i].muv;
					test.guv = input[i].guv;
					test.nuv = input[i].muv;
					OutputStream.Append(test);
				}
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
				float4x4 thresholdMatrix =
				{ 1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
					13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
					4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
					16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
				};
				float4x4 _RowAccess = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
				float2 pos = i.screenPos.xy / i.screenPos.w;
				pos *= _ScreenParams.xy/2; // pixel position
				float explode = clamp(_ExplodeAmount - _ExplodeFadeOffset, 0, 1);
				float transparency = clamp(_Transparency + explode, 0, 1);
				clip(1-transparency - thresholdMatrix[fmod(pos.x, 4)] * _RowAccess[fmod(pos.y, 4)]);

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}

		Pass{


			CGPROGRAM
#pragma vertex vert
#pragma geometry geom
#pragma fragment frag

			// make fog work
#pragma multi_compile_fog

#include "UnityCG.cginc"

			struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 muv : TEXCOORD0;
			float2 guv : TEXCOORD1;
			float2 nuv : TEXCOORD2;
			float3 normal : NORMAL;
			UNITY_FOG_COORDS(1)
				float3 worldPosition : TEXCOORD3;
			float4 vertex : SV_POSITION;
			float4 screenPos : COLOR;
			float explAmount : RANDO;
		};

		uniform float4 _ScreenPrams;
		sampler2D _MainTex;
		sampler2D _GlitchTex;
		sampler2D _NoiseTex;
		float _GlitchAmount;
		float _ExplodeAmount;
		float _ExplodeScale;
		float _Transparency;
		float _ExplodeFadeOffset;
		float4 _MainTex_ST;
		float4 _GlitchTex_ST;
		float4 _NoiseTex_ST;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = v.vertex;
			o.normal = v.normal;
			o.muv = TRANSFORM_TEX(v.uv, _MainTex);
			o.guv = TRANSFORM_TEX(v.uv, _GlitchTex);
			o.nuv = TRANSFORM_TEX(v.uv, _NoiseTex);
			o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.screenPos = ComputeScreenPos(o.vertex);
			UNITY_TRANSFER_FOG(o,o.vertex);
			o.explAmount = 0;
			return o;
		}

		[maxvertexcount(6)]
		void geom(triangle v2f input[3], inout TriangleStream<v2f> OutputStream)
		{
			v2f test = (v2f)0;
			float3 pos = mul(unity_ObjectToWorld, (input[0].vertex.xyz + input[1].vertex.xyz + input[2].vertex.xyz) / 3.0);
			float3 dir = normalize(pos);
			dir.y += 2;
			dir.y -= _ExplodeAmount *  _ExplodeAmount * 2;
			float3 worldPos = (input[0].worldPosition.xyz + input[1].worldPosition.xyz + input[2].worldPosition.xyz) / 3.0;
			float explodeAmount = clamp((_ExplodeAmount - tex2Dlod(_NoiseTex, float4(input[0].nuv, 0, 0)).r), 0, 2);
			float explodeFinal = (explodeAmount * _ExplodeScale);

			for (int i = 0; i < 3; i++)
			{
				test.normal = input[i].normal;
				float3 vert = input[i].worldPosition + float3(dir* explodeFinal);
				if (vert.y < 0) return;
				test.vertex = mul(UNITY_MATRIX_VP, float4(vert,1.0));
				test.explAmount = explodeAmount;
				test.screenPos = ComputeScreenPos(test.vertex);
				test.muv = input[i].muv;
				test.guv = input[i].guv;
				test.nuv = input[i].muv;
				OutputStream.Append(test);
			}
		}

		fixed4 frag(v2f i) : SV_Target
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
		float4x4 thresholdMatrix =
		{ 1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
			13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
			4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
			16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
		};
		float4x4 _RowAccess = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
		float2 pos = i.screenPos.xy / i.screenPos.w;
		pos *= _ScreenParams.xy / 2; // pixel position
		clip(1 - _Transparency - thresholdMatrix[fmod(pos.x, 4)] * _RowAccess[fmod(pos.y, 4)]);

		// apply fog
		UNITY_APPLY_FOG(i.fogCoord, col);
		return col;
		}
			ENDCG
		}
	}
}
