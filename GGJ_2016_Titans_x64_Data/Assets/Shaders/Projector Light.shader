Shader "Projector/Projector Tileable"
{
    Properties
    {
		_Color ("Main Color", Color) = (1,1,1,0)
		_ShadowTex ("Cookie", 2D) = "gray" { TexGen ObjectLinear }
    }
     
    Subshader
    {
		Pass
		{
			Fog { Mode Off }
			
			Offset -1, -1
			ZWrite Off
			Blend SrcAlpha One
			
			
			CGPROGRAM
			
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				 
				 
				uniform sampler2D _ShadowTex;
				uniform float4 _ShadowTex_ST;
				
				uniform float4 _Color;
				
				uniform float4x4 _Projector;
				
				
				struct v_out
				{
					float4 pos : SV_POSITION;
					float2 texCoord : TEXCOORD0;
				};
				 
				v_out vert(appdata_tan v)
				{
					v_out o;
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
					o.texCoord = mul (_Projector, v.vertex).xy;
					o.texCoord = o.texCoord * _ShadowTex_ST.xy + _ShadowTex_ST.zw;
					
					return o;
				}
				 
				float4 frag(v_out i) : COLOR
				{
					float4 tex = tex2D(_ShadowTex, i.texCoord) * _Color;
					return tex;
				}
				
			ENDCG
		 
		}
    }
}