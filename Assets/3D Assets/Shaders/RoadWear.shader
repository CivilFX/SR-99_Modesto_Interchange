
/*
Shader "CivilFX/RoadWear" 
{
	Properties{
		_Diffuse("Diffuse",2D) = "white" {}
	}
	SubShader{
		Tags{ "Queue" = "Geometry + 500" }
		Pass{
		Blend OneMinusSrcAlpha SrcAlpha
		SetTexture[_Diffuse]{ combine texture }
	}
	}
}
*/

Shader "CivilFX/RoadWear" {

	// This material's job is mainly to compensate for the way 3ds-max does 'Real-World-Scale'.
	// It also should do alpha blending, apply z-biasing and disable shadows
	//
	// Real-World-Scale makes texturing the road stripes really easy but it doesn't export nicely
	// and I have not found a good workflow that ends up with correct UV coords without messing things
	// up inside 3ds-max.  So, we use this shader (for now) to apply the inverse scale to uv-coords.  
	// if the Real World scale is 1.25' x 40', then set this shader's RealWorldScale parameter to 1/1.25 , 1/40, 0, 0

	Properties{
		_Diffuse("Diffuse",2D) = "white" {}
	}
	SubShader{
		Tags{ "RenderType" = "Fade"  "Queue" = "Geometry+500" } 
		Offset -1, -1
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		// Surface Shader Options - see: http://docs.unity3d.com/Manual/SL-SurfaceShaders.html
		// For road striping, we want no shadows, alpha blending, etc
//#pragma surface surf Lambert noshadow nometa noforwardadd nodirlightmap nodynlightmap noambient nofog halfasview interpolateview alpha:fade exclude_path:prepass exclude_path:deferred finalcolor:final_color
#pragma surface surf Lambert noshadow alpha:blend

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _Diffuse;

		struct Input {
			float2 uv_Diffuse;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			half2 uv = IN.uv_Diffuse;
			fixed4 texel = tex2D(_Diffuse, uv);

			o.Albedo = texel.rgb;
			o.Alpha =  texel.a;
			
			//o.Normal = float3(1, 0, 0);
			//o.Emission = float3(1, 1, 1);
			//o.Metallic = 0;
			//o.Smoothness = 0;
			//o.Occlusion = 0;

			//o.Specular = 0.0f;
			//o.Gloss = 0;
		}

	ENDCG
	}
	FallBack "Diffuse"
}



