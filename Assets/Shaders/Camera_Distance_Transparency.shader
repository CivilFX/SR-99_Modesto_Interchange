// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Camera_Distance_Transparency"
{
    Properties
    {
        _MainTex ("Color (RGB) Alpha (A)", 2D) = "white" {} // Regular object texture
        _Radius("Radius", Range(0.00,500)) = 10 
        _Cutoff("Cut off", Range(0.00,1)) = 0.5
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

            Blend SrcAlpha OneMinusSrcAlpha
          
            ZWrite Off
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag



                #include "UnityCG.cginc"

                

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;

            };

            sampler2D _MainTex;
            float _Cutoff;
            float4 _MainTex_ST;
          

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float _Radius;

             fixed4 frag(v2f i) : SV_Target {

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float dist = distance(i.worldPos, _WorldSpaceCameraPos);
                col.a*= saturate(dist / _Radius);
               
                /*if (col.a <=0.2) {
                    discard;
                }*/
                return col;
            }
            ENDCG
        }
    }
}
