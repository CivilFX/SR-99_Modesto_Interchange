// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CivilFX/Object Mask" {
	Properties{
	}
	SubShader{
		Tags{ "Queue" = "Geometry+1" "RenderType" = "Opaque" }
		ZWrite On
		Pass{
			Blend Zero One
		}
	}
	FallBack "Diffuse"
}