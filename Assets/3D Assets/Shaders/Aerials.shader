Shader "CivilFX/Aerial" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.0
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" "Queue" = "Background+10" }
		LOD 200
		//ZWrite On
		//ZTest Always

		CGPROGRAM
#pragma surface surf Standard vertex:vert fullforwardshadows
#pragma target 3.0
		struct Input {
			float2 uv_MainTex;
			float3 vertexColor; // Vertex color stored here by vert() method
		};

		struct v2f {
			float4 pos : SV_POSITION;
			fixed4 color : COLOR;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.vertexColor = v.color; // Save the Vertex Color in the Input for the surf() method
		}

		sampler2D _MainTex;

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * IN.vertexColor; 
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			clip(c.a - 1.0);	//2.99 - c.r - c.g - c.b);	//Clip Aerials where they are perfectly white.
			o.Alpha = c.a;
		}
	ENDCG
	}

	FallBack "Diffuse"
}