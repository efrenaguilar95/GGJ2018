Shader "Custom/LightningShader" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ScrollX("UV Scroll X Speed", Range(0, 10)) = 1
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			sampler2D _MainTex;
			half _ScrollX;

			fixed4 frag (v2f i) : SV_TARGET {
				_ScrollX *= _Time.x;
				float2 uvOffset = i.uv + float2(_ScrollX, 0);

				fixed4 c = tex2D(_MainTex, uvOffset);

				return c;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
