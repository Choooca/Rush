Shader "Custom/Dissolve"
{
    Properties
    {
		[Header(Noise)]
		_CellSize ("CellSize", float) = 1.0
		_Speed ("Speed", Range(0,1)) = 1.0
		_Step ("Step", float) = 1.0
		_Density("Density", float) = 1.0

		_FromColor ("From Color", Color) =(0.0 ,0.0 ,0.0 ,0.0) 
		_ToColor ("To Color", Color) = (1.0 ,1.0 ,1.0 ,1.0)

		[Header(Dissolve)]
        _MainTex ("Dissolve Texture", 2D) = "white" {}
		[HDR] _Emission ("Emission", Color) = (1.0,1.0,1.0,1.0)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_DissolveStep ("Dissolve Step", float) = 0.0
		_EmissionOffset ("Emission Step Offset", float) = 0.0
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
		
		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Random.cginc"
			#include "UnityCG.cginc"

			sampler2D _MainTex;
            float4 _MainTex_ST;

			float _CellSize;
			float _Step;
			float _Speed;
			float _Density;
			float4 _FromColor;
			float4 _ToColor;

			half _Glossiness;
			half _Metallic;
			float4 _Emission;
			float _DissolveStep;
			float _EmissionOffset; 

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float3 viewDir : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
				return o;
			}

			float easeIn(float interpolator)
			{
				return interpolator * interpolator;
			}

			float easeOut(float interpolator)
			{
				return 1- easeIn(1 - interpolator);
			}

			float easeInOut(float interpolator)
			{
				return lerp(easeIn(interpolator), easeOut(interpolator), interpolator);
			}

			float4 lerpColor(float4 from, float4 to, float interpolator)
			{
				return from + (to-from) * interpolator;
			}



			float4 frag (v2f IN) : SV_Target
			{
				fixed4 dissolveColor = tex2D(_MainTex, IN.uv);
				float _EmissionStep = _DissolveStep + _EmissionOffset; 
				float alpha = step(_DissolveStep, dissolveColor.r);
				float emission = step(dissolveColor.r, _EmissionStep);

				float emissionChange =  fwidth(emission);
				float finalEmission = smoothstep(0, emissionChange, emission);

				float alphaChange = fwidth(alpha);
				float finalAlpha = smoothstep(0, alphaChange, alpha );

				//Noise

				float3 value = IN.viewDir.xyz * _CellSize;
				value.y += _Time.y * _Speed;
				float3 fraction = frac(value);

				float interpolatorX = easeInOut(fraction.x);
				float interpolatorY = easeInOut(fraction.y);
				float interpolatorZ = easeInOut(fraction.z);

				float valueNoiseZ[2];
				[unroll]
				for(int z = 0; z < 2; z++)
				{
					float valueNoiseY[2];
					[unroll]
					for(int y = 0; y < 2; y++)
					{
						float valueNoiseX[2];
						[unroll]
						for(int x = 0; x < 2; x++)
						{
							float3 cell = floor(value) + float3(x,y,z);
							float3 cellDir = rand3dTo3d(cell) * 2 - 1;
							float3 compareDir = fraction - float3(x,y,z);
							valueNoiseX[x] = dot(cellDir , compareDir);
						}
						valueNoiseY[y] = lerp(valueNoiseX[0], valueNoiseX[1], interpolatorX);
					}
					valueNoiseZ[z] = lerp(valueNoiseY[0], valueNoiseY[1], interpolatorY);
				}
				float noise = lerp(valueNoiseZ[0], valueNoiseZ[1], interpolatorZ) + 0.5;

				_Step = floor(_Step);

				noise = frac(noise* _Density);

				noise = floor(noise*_Step) / (_Step - 1);

				float3 color = lerpColor(_FromColor, _ToColor, noise);

				return float4(color, finalAlpha);
			}
			ENDCG
		}
    }
    FallBack "Standard"
}
