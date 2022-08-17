Shader "CivilFX/RoadSurface" {
	// This shader uses a wear mask to blend between 'new' and 'worn' asphalt.  It can also blend
	// other material properties that we want to customize between the 'new' and 'worn' areas such as the
	// specular properties.
	Properties {
		_WearMask("WearMask",2D) = "white" {}
		_Gloss("Gloss",Range(0,1)) = 0.5
		_GlossWorn("GlossWorn",Range(0,1)) = 0.7
		_Specular("Specular", Float) = 128.0
		_SpecularWorn("SpecularWorn", Float) = 16.0
		_Surface("Surface",2D) = "white" {}
		_SurfaceWorn("SurfaceWorn",2D) = "white" {}
		_SurfaceUVScale("SurfaceUVScale",Float) = 1.0
	}
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _WearMask;
		sampler2D _Surface;
		sampler2D _SurfaceWorn;

		struct Input {
			float2 uv_WearMask;
			float3 worldPos;
		};

		float _Gloss;
		float _GlossWorn;
		float _Specular;
		float _SpecularWorn;
		float _SurfaceUVScale;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			half2 surface_uv = IN.worldPos.xz * _SurfaceUVScale;

			fixed4 wear_texel = tex2D(_WearMask, IN.uv_WearMask);
			fixed4 surface_texel = tex2D(_Surface, surface_uv);
			fixed4 surface_worn_texel = tex2D(_SurfaceWorn, surface_uv);

			o.Albedo = lerp(surface_texel.rgb, surface_worn_texel.rgb, wear_texel.r);
			o.Smoothness = lerp(_Gloss, _GlossWorn, wear_texel.r);
			//o.Specular = lerp(_Specular, _SpecularWorn, wear_texel.r);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
