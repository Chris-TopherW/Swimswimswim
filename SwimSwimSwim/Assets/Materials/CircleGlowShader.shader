Shader "Unlit/CircleGlowShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_GlowColour("Glow Colour", Color) = (0.5, 1.0, 1.0, 1.0)
		_GlowColour2("Glow Colour", Color) = (0.5, 1.0, 1.0, 1.0)
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 100
		ZWrite Off
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
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	fixed4 _GlowColour;
	fixed4 _GlowColour2;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 c = 0;
	// normal is a 3D vector with xyz components; in -1..1
	// range. To display it as color, bring the range into 0..1
	// and put into red, green, blue components=
	float glowMod = 0.8 + (sin(_Time*15*2*3.141) * 0.2);
	c.rgb = lerp(_GlowColour,_GlowColour2,glowMod);
	c.a = tex2D(_MainTex, i.uv).a * _GlowColour.a * glowMod;
	return c;
	}
		ENDCG
	}
	}
}
