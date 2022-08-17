Shader "CivilFX/Double Landscape" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_DetailTex("Detail (RGB)", 2D) = "white" {}
		_DetailNormal("Detail (Normal)", 2D) = "white" {}
		_Scale("Scale", Range(0,1000)) = 1
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		Cull Front
		CGPROGRAM

			// Physically based Standard lighting model, and enable shadows on all light types
	#pragma surface surf Standard Lambert vertex:vert fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
	#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DetailTex;
		sampler2D _DetailNormal;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DetailTex;
			float2 uv_DetailNormal;
			fixed facing : VFACE;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _Scale;

		void vert(inout appdata_full v) {
			//v.normal *= -1;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 d = tex2D(_DetailTex, IN.uv_DetailTex * _Scale);
			o.Albedo = c.rgb * d.rgb * 4;
			o.Normal = UnpackNormal(tex2D(_DetailNormal, IN.uv_DetailNormal * _Scale));
			o.Normal.z *= IN.facing; // flip Z based on facing
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG

		Cull Back
		CGPROGRAM

	// Physically based Standard lighting model, and enable shadows on all light types
	#pragma surface surf Standard fullforwardshadows

	// Use shader model 3.0 target, to get nicer looking lighting
	#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DetailTex;
		sampler2D _DetailNormal;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DetailTex;
			float2 uv_DetailNormal;
			fixed facing : VFACE;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _Scale;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 d = tex2D(_DetailTex, IN.uv_DetailTex * _Scale);
			o.Albedo = c.rgb * d.rgb * 4;
			o.Normal = UnpackNormal(tex2D(_DetailNormal, IN.uv_DetailNormal * _Scale));
			o.Normal.z *= IN.facing; // flip Z based on facing
			//float3 n = UnpackNormal(text2D(_DetailNormal, IN.uv_DetailNormal * _Scale));
			//o.Normal = dot(IN.viewDir, float3(0, 0, 1)) > 0 ? n : -n;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
		}
			FallBack "Diffuse"
}