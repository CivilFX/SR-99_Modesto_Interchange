Shader "CivilFX/Masked Aerial" 
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader{
		Tags{ 
			"Queue" = "Geometry+2"
			"RenderType" = "Opaque"
		}
		ZWrite On ZTest On ColorMask RGBA
		LOD 200

		CGPROGRAM
			#pragma surface surf Lambert noforwardadd
			#pragma target 3.0
			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
		ENDCG
	}
	Fallback "Mobile/VertexLit"
}