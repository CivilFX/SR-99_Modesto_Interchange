Shader "CivilFX/RoadStriping" {

	// This material's job is mainly to compensate for the way 3ds-max does 'Real-World-Scale'.
	// It also should do alpha blending, apply z-biasing and disable shadows
	//
	// Real-World-Scale makes texturing the road stripes really easy but it doesn't export nicely
	// and I have not found a good workflow that ends up with correct UV coords without messing things
	// up inside 3ds-max.  So, we use this shader (for now) to apply the inverse scale to uv-coords.  
	// if the Real World scale is 1.25' x 40', then set this shader's RealWorldScale parameter to 1/1.25 , 1/40, 0, 0

	Properties{
		_Diffuse("Diffuse",2D) = "white" {}
		_RealWorldScale("RealWorldScale",Vector) = (1,1,0,0) // sizex,sizey,offsetx,offsety
	}
	SubShader{
		Tags{ "RenderType" = "Fade" }
		Offset -1, -1
		LOD 200

		CGPROGRAM
		// Surface Shader Options - see: http://docs.unity3d.com/Manual/SL-SurfaceShaders.html
		// For road striping, we want no shadows, alpha blending, etc
#pragma surface surf Standard noshadow nometa noforwardadd halfasview interpolateview decal:blend

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _Diffuse;

		struct Input {
			float2 uv_Diffuse;
			float3 worldPos;
		};

		float4 _RealWorldScale;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half2 uv = IN.uv_Diffuse;
			uv.x = uv.x*_RealWorldScale.x + _RealWorldScale.z;
			uv.y = uv.y*_RealWorldScale.y + _RealWorldScale.w;

			fixed4 texel = tex2D(_Diffuse, uv);

			o.Albedo = texel.rgb;
			o.Alpha = texel.a;
			//o.Smoothness = 1;
			//o.Specular = 1;
		}
	ENDCG
	}
	FallBack "Diffuse"
}


