// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/GridShader"
{
	Properties{
		_GridThickness("Grid Thickness", Float) = 0.01
		_ZoneThickness("Zone Thickness", Float) = 0.01
		_ZoneScalar("Zone Scalar", Float) = 1
		_Falloff("Falloff", Float) = 1
		_GridSpacing("Grid Spacing", Float) = 1.0
		_ZonePosition("Zone Position", Float) = 4
		_ZoneWidth("Zone Width", Float) = 4
		_GridColour("Grid Colour", Color) = (0.5, 1.0, 1.0, 1.0)
		_GridColour2("Grid Colour", Color) = (0.5, 1.0, 1.0, 1.0)
		_GridColour3("Grid Colour", Color) = (0.5, 1.0, 1.0, 1.0)
		_GridColour4("Grid Colour", Color) = (0.5, 1.0, 1.0, 1.0)
		_GridColour5("Grid Colour", Color) = (0.5, 1.0, 1.0, 1.0)
		_ZoneColour("Zone  Colour", Color) = (0.5, 1.0, 1.0, 1.0)
	}

		SubShader{
		Tags{ "Queue" = "Transparent" }
		Pass{
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

		// Define the vertex and fragment shader functions
#pragma vertex vert
#pragma fragment frag 

		// Access Shaderlab properties
		uniform float _GridThickness;
	uniform float _ZoneThickness;
	uniform float _ZoneScalar;
	uniform float _GridSpacing;
	uniform float _Falloff;
	uniform float _GridOffsetX;
	uniform float _GridOffsetY;
	uniform float _ZonePosition;
	uniform float _ZoneWidth;
	uniform fixed4 _GridColour;
	uniform fixed4 _ZoneColour;

	// Input into the vertex shader
	struct vertexInput {
		float4 vertex : POSITION;
	};


	// Output from vertex shader into fragment shader
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 worldPos : TEXCOORD0;
		float4 localPos : TEXCOORD1;
	};


	// VERTEX SHADER
	vertexOutput vert(vertexInput input) {
		vertexOutput output;
		output.pos = UnityObjectToClipPos(input.vertex);
		// Calculate the world position coordinates to pass to the fragment shader
		output.worldPos = mul(unity_ObjectToWorld, input.vertex);
		output.localPos = input.vertex;
		return output;
	}
	// FRAGMENT SHADER
	fixed4 frag(vertexOutput input) : SV_TARGET{
		float zoneMix = smoothstep(_ZonePosition - _ZoneWidth / 2, _ZonePosition, input.worldPos.z) - smoothstep(_ZonePosition, _ZonePosition + _ZoneWidth / 2, input.worldPos.z);
		float finalThickness = lerp(_GridThickness, _ZoneThickness, zoneMix);
		float4 f = abs(frac(input.worldPos * lerp(1, _ZoneScalar, zoneMix) * _GridSpacing) - 0.5);
		float4 df = fwidth(input.worldPos * lerp(1, _ZoneScalar, zoneMix) * _GridSpacing);
		float mi = max(0.0, finalThickness - 1.0);
		float ma = max(1.0, finalThickness);
		float4 g = clamp((f - df*mi) / (df*(ma - mi)), max(0.0, 1.0- finalThickness), 1.0);
		float val = g.x*g.y*g.z;
		float a = lerp(0, 1,1-val) *_GridColour.a;
		fixed4 c = lerp(_GridColour, _ZoneColour, zoneMix);
		float falloff = 1- smoothstep(_ZonePosition, _ZonePosition + _ZoneWidth / 2, input.worldPos.z);
		c.a = a * falloff;
		return c;
	}
		ENDCG
	}
		Pass{
		Stencil{
		Ref 1
		Comp equal
	}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

		// Define the vertex and fragment shader functions
#pragma vertex vert
#pragma fragment frag 

		// Access Shaderlab properties
		uniform float _GridThickness;
	uniform float _ZoneThickness;
	uniform float _ZoneScalar;
	uniform float _GridSpacing;
	uniform float _Falloff;
	uniform float _GridOffsetX;
	uniform float _GridOffsetY;
	uniform float _ZonePosition;
	uniform float _ZoneWidth;
	uniform fixed4 _GridColour2;
	uniform fixed4 _ZoneColour;

	// Input into the vertex shader
	struct vertexInput {
		float4 vertex : POSITION;
	};


	// Output from vertex shader into fragment shader
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 worldPos : TEXCOORD0;
		float4 localPos : TEXCOORD1;
	};


	// VERTEX SHADER
	vertexOutput vert(vertexInput input) {
		vertexOutput output;
		output.pos = UnityObjectToClipPos(input.vertex);
		// Calculate the world position coordinates to pass to the fragment shader
		output.worldPos = mul(unity_ObjectToWorld, input.vertex);
		output.localPos = input.vertex;
		return output;
	}
	// FRAGMENT SHADER
	fixed4 frag(vertexOutput input) : SV_TARGET{
		float zoneMix = smoothstep(_ZonePosition - _ZoneWidth / 2, _ZonePosition, input.worldPos.z) - smoothstep(_ZonePosition, _ZonePosition + _ZoneWidth / 2, input.worldPos.z);
	float finalThickness = lerp(_GridThickness, _ZoneThickness, zoneMix);
	float4 f = abs(frac(input.worldPos * lerp(1, _ZoneScalar, zoneMix) * _GridSpacing) - 0.5);
	float4 df = fwidth(input.worldPos * lerp(1, _ZoneScalar, zoneMix) * _GridSpacing);
	float mi = max(0.0, finalThickness - 1.0);
	float ma = max(1.0, finalThickness);
	float4 g = clamp((f - df*mi) / (df*(ma - mi)), max(0.0, 1.0 - finalThickness), 1.0);
	float val = g.x*g.y*g.z;
	float a = lerp(0, 1,1 - val) *_GridColour2.a;
	fixed4 c = _GridColour2;
	float falloff = 1 - smoothstep(_ZonePosition + _ZoneWidth / 2, _ZonePosition + _ZoneWidth / 2 + _Falloff, input.worldPos.z);
	c.a = a * falloff;
	return c;
	}
		ENDCG
	}
			Pass{
		Stencil{
		Ref 2
		Comp equal
	}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

		// Define the vertex and fragment shader functions
#pragma vertex vert
#pragma fragment frag 

		// Access Shaderlab properties
		uniform float _GridThickness;
	uniform float _ZoneThickness;
	uniform float _ZoneScalar;
	uniform float _GridSpacing;
	uniform float _Falloff;
	uniform float _GridOffsetX;
	uniform float _GridOffsetY;
	uniform float _ZonePosition;
	uniform float _ZoneWidth;
	uniform fixed4 _GridColour3;
	uniform fixed4 _ZoneColour;

	// Input into the vertex shader
	struct vertexInput {
		float4 vertex : POSITION;
	};


	// Output from vertex shader into fragment shader
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 worldPos : TEXCOORD0;
		float4 localPos : TEXCOORD1;
	};


	// VERTEX SHADER
	vertexOutput vert(vertexInput input) {
		vertexOutput output;
		output.pos = UnityObjectToClipPos(input.vertex);
		// Calculate the world position coordinates to pass to the fragment shader
		output.worldPos = mul(unity_ObjectToWorld, input.vertex);
		output.localPos = input.vertex;
		return output;
	}
	// FRAGMENT SHADER
	fixed4 frag(vertexOutput input) : SV_TARGET{
		float zoneMix = smoothstep(_ZonePosition - _ZoneWidth / 2, _ZonePosition, input.worldPos.z) - smoothstep(_ZonePosition, _ZonePosition + _ZoneWidth / 2, input.worldPos.z);
	float finalThickness = lerp(_GridThickness, _ZoneThickness, zoneMix);
	float4 f = abs(frac(input.worldPos * lerp(1, _ZoneScalar, zoneMix) * _GridSpacing) - 0.5);
	float4 df = fwidth(input.worldPos * lerp(1, _ZoneScalar, zoneMix) * _GridSpacing);
	float mi = max(0.0, finalThickness - 1.0);
	float ma = max(1.0, finalThickness);
	float4 g = clamp((f - df*mi) / (df*(ma - mi)), max(0.0, 1.0 - finalThickness), 1.0);
	float val = g.x*g.y*g.z;
	float a = lerp(0, 1,1 - val) *_GridColour3.a;
	fixed4 c = _GridColour3;
	float falloff = 1 - smoothstep(_ZonePosition + _ZoneWidth / 2, _ZonePosition + _ZoneWidth / 2 + _Falloff, input.worldPos.z);
	c.a = a * falloff;
	return c;
	}
		ENDCG
	}
				Pass{
		Stencil{
		Ref 3
		Comp equal
	}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

		// Define the vertex and fragment shader functions
#pragma vertex vert
#pragma fragment frag 

		// Access Shaderlab properties
		uniform float _GridThickness;
	uniform float _ZoneThickness;
	uniform float _ZoneScalar;
	uniform float _GridSpacing;
	uniform float _Falloff;
	uniform float _GridOffsetX;
	uniform float _GridOffsetY;
	uniform float _ZonePosition;
	uniform float _ZoneWidth;
	uniform fixed4 _GridColour4;
	uniform fixed4 _ZoneColour;

	// Input into the vertex shader
	struct vertexInput {
		float4 vertex : POSITION;
	};


	// Output from vertex shader into fragment shader
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 worldPos : TEXCOORD0;
		float4 localPos : TEXCOORD1;
	};


	// VERTEX SHADER
	vertexOutput vert(vertexInput input) {
		vertexOutput output;
		output.pos = UnityObjectToClipPos(input.vertex);
		// Calculate the world position coordinates to pass to the fragment shader
		output.worldPos = mul(unity_ObjectToWorld, input.vertex);
		output.localPos = input.vertex;
		return output;
	}
	// FRAGMENT SHADER
	fixed4 frag(vertexOutput input) : SV_TARGET{
		float zoneMix = smoothstep(_ZonePosition - _ZoneWidth / 2, _ZonePosition, input.worldPos.z) - smoothstep(_ZonePosition, _ZonePosition + _ZoneWidth / 2, input.worldPos.z);
	float finalThickness = lerp(_GridThickness, _ZoneThickness, zoneMix);
	float4 f = abs(frac(input.worldPos * lerp(1, _ZoneScalar, zoneMix) * _GridSpacing) - 0.5);
	float4 df = fwidth(input.worldPos * lerp(1, _ZoneScalar, zoneMix) * _GridSpacing);
	float mi = max(0.0, finalThickness - 1.0);
	float ma = max(1.0, finalThickness);
	float4 g = clamp((f - df*mi) / (df*(ma - mi)), max(0.0, 1.0 - finalThickness), 1.0);
	float val = g.x*g.y*g.z;
	float a = lerp(0, 1,1 - val) *_GridColour4.a;
	fixed4 c = _GridColour4;
	float falloff = 1 - smoothstep(_ZonePosition + _ZoneWidth / 2, _ZonePosition + _ZoneWidth / 2 + _Falloff, input.worldPos.z);
	c.a = a * falloff;
	return c;
	}
		ENDCG
	}
					Pass{
		Stencil{
		Ref 4
		Comp equal
	}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

		// Define the vertex and fragment shader functions
#pragma vertex vert
#pragma fragment frag 

		// Access Shaderlab properties
		uniform float _GridThickness;
	uniform float _ZoneThickness;
	uniform float _ZoneScalar;
	uniform float _GridSpacing;
	uniform float _Falloff;
	uniform float _GridOffsetX;
	uniform float _GridOffsetY;
	uniform float _ZonePosition;
	uniform float _ZoneWidth;
	uniform fixed4 _GridColour5;
	uniform fixed4 _ZoneColour;

	// Input into the vertex shader
	struct vertexInput {
		float4 vertex : POSITION;
	};


	// Output from vertex shader into fragment shader
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 worldPos : TEXCOORD0;
		float4 localPos : TEXCOORD1;
	};


	// VERTEX SHADER
	vertexOutput vert(vertexInput input) {
		vertexOutput output;
		output.pos = UnityObjectToClipPos(input.vertex);
		// Calculate the world position coordinates to pass to the fragment shader
		output.worldPos = mul(unity_ObjectToWorld, input.vertex);
		output.localPos = input.vertex;
		return output;
	}
	// FRAGMENT SHADER
	fixed4 frag(vertexOutput input) : SV_TARGET{
		float zoneMix = smoothstep(_ZonePosition - _ZoneWidth / 2, _ZonePosition, input.worldPos.z) - smoothstep(_ZonePosition, _ZonePosition + _ZoneWidth / 2, input.worldPos.z);
	float finalThickness = lerp(_GridThickness, _ZoneThickness, zoneMix);
	float4 f = abs(frac(input.worldPos * lerp(1, _ZoneScalar, zoneMix) * _GridSpacing) - 0.5);
	float4 df = fwidth(input.worldPos * lerp(1, _ZoneScalar, zoneMix) * _GridSpacing);
	float mi = max(0.0, finalThickness - 1.0);
	float ma = max(1.0, finalThickness);
	float4 g = clamp((f - df*mi) / (df*(ma - mi)), max(0.0, 1.0 - finalThickness), 1.0);
	float val = g.x*g.y*g.z;
	float a = lerp(0, 1,1 - val) *_GridColour5.a;
	fixed4 c = _GridColour5;
	float falloff = 1 - smoothstep(_ZonePosition + _ZoneWidth / 2, _ZonePosition + _ZoneWidth / 2 + _Falloff, input.worldPos.z);
	c.a = a * falloff;
	return c;
	}
		ENDCG
	}
	}
}