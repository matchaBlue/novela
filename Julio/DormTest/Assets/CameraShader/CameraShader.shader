Shader "Custom/CameraShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _NoiseTex;

			float _StartTime;

			fixed4 frag (v2f IN) : SV_Target
			{
				//fixed4 col = tex2D(_MainTex, IN.uv + float2(0, 0.03*sin(10*IN.uv.x + 5*_Time[1])));
				// just invert the colors
				// col.rgb = 1 - col.rgb;
				fixed4 col = tex2D(_MainTex, IN.uv);

				float offset_x = 0.5 - IN.uv.x;
				float offset_y = 0.5 - IN.uv.y;

				float dst_from_ctr = pow(offset_x, 2) + pow(offset_y, 2);

				float color_offset = 3 * dst_from_ctr * (1-tex2D(_NoiseText, IN.uv).r)/(_Time[1]+0.1));
				float time = Time[1] - _StartTime + 0.1;

				col.r = col.r + color_offset;
				col.g = col.g - color_offset;
				col.b = col.b - color_offset;

				return col;
			}
			ENDCG
		}
	}
}
