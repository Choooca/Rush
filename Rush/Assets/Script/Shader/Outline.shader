Shader "Hidden/Outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Thickness ("Thickness", float) = 1.0
		_Step ("Step", float) = 1.0
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)

	}

	SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM


			#pragma vertex vert	
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;

			float _Step;
			float _Thickness;
			float4 _Color;
			
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

			float2 pixelOffset[9] = 
			{
				float2(-1,1), float2(0,1), float2(1,1),
				float2(-1,0), float2(0,0), float2(1,0),
				float2(-1,-1), float2(0,-1), float2(1,-1)
			};

			float scharrOperatorX[9] =
			{
				3.0, 0.0, -3.0,
				10.0, 0.0, -10.0,
				3.0, 0.0, -3.0
			};

			float scharrOperatorY[9] =
			{
				3.0, 10.0, 3.0,
				0.0, 0.0, 0.0,
				-3.0, -10.0, -3.0
			};

			float gaussianBlurFilter[9] =
			{
				1.0, 2.0, 1.0,
				2.0, 4.0, 2.0,
				1.0, 2.0, 1.0
			};


			// Gaussian blur : 
			// 1 2 1
			// 2 4 2 * 1/16
			// 1 2 1

			float gaussianBlur (float stepX, float stepY, float2 baseCoord, sampler2D tex)
			{
				float topLeft = tex2D(tex, baseCoord + float2(-stepX, stepY));
				float midLeft = tex2D(tex, baseCoord + float2(-stepX, 0));
				float botLeft = tex2D(tex, baseCoord + float2(-stepX, -stepY));
				float topMid = tex2D(tex, baseCoord + float2(0, stepY));
				float midMid  = tex2D(tex, baseCoord);
				float botMid = tex2D(tex, baseCoord + float2(0, -stepY));
				float topRight = tex2D(tex, baseCoord + float2(stepX, stepY));
				float midRight = tex2D(tex, baseCoord + float2(stepX, 0));
				float botRight = tex2D(tex, baseCoord + float2(stepX, -stepY));

				float value = 1.0 * topLeft + 2.0 * topMid + 1.0 * topRight + 2.0 * midLeft + 4.0 * midMid + 2.0 * midRight + 1.0 * botLeft + 2.0 * botMid + 1.0 * botRight;
				return value;
			}

			// Scharr operator:
			//        3 0 -3        3 10   3
			//    X = 10 0 -10  Y = 0  0   0
			//        3 0 -3        -3 -10 -3

			float sobel (float stepX, float stepY, float2 baseCoord, sampler2D tex)
			{
				float3 greyScale = float3(0.3333,0.3333,0.3333);

				float topLeft = gaussianBlur(stepX, stepY, baseCoord + float2(-stepX, stepY), tex);
				float midLeft = gaussianBlur(stepX, stepY, baseCoord + float2(-stepX, 0), tex);
				float botLeft = gaussianBlur(stepX, stepY, baseCoord + float2(-stepX, -stepY), tex);
				float topMid = gaussianBlur(stepX, stepY, baseCoord + float2(0, stepY),tex);
				float botMid = gaussianBlur(stepX, stepY, baseCoord + float2(0, -stepY),tex);
				float topRight = gaussianBlur(stepX, stepY, baseCoord + float2(stepX, stepY),tex);
				float midRight = gaussianBlur(stepX, stepY, baseCoord + float2(stepX, 0),tex);
				float botRight = gaussianBlur(stepX, stepY, baseCoord + float2(stepX, -stepY),tex);
				
				float x = (3.0 * topLeft + 10.0 * midLeft + 3.0 * botLeft - 3.0 * topRight - 10.0 * midRight - 3.0 * botRight) * greyScale;
				float y = (3.0 * topLeft + 10.0 * topMid + 3.0 * topRight - 3.0 * botLeft - 10.0 * botMid - 3.0 * botRight) * greyScale;

				return sqrt((x * x) + (y * y));
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			float4 frag (v2f i) : SV_Target
			{
				float4 baseColor = tex2D(_MainTex, i.uv);
				float4 depth = tex2D(_CameraDepthTexture, i.uv);;
				depth = Linear01Depth(depth);

				float depthSobel = sobel(_Step / _ScreenParams.x, _Step / _ScreenParams.y , i.uv , _MainTex);
				depthSobel = step( _Thickness, depthSobel);

				float4 _OutlineColored = depthSobel * _Color;

				float4 finalColor = baseColor * (1.0 - depthSobel) + _OutlineColored * depthSobel;
				finalColor = saturate(finalColor);
				return finalColor	;

			}

            ENDCG
        }
    }
}
