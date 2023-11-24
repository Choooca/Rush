Shader "Custom/ToonLighting"
{
    Properties
    {
		[Header(Base Parameters)]
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Emmision ("Emission", float) = 0
		[HDR] _EmmisionColor ("Emmision Color", Color) = (1,1,1,1)

		[Header(Lighting Parameters)]
		_ShadowTint ("Shadow Color", Color) = (0,0,0,1)
		[IntRange] _StepAmount ("Shadow Steps", Range(1,16)) = 2
		_StepWidth ("Step Size", Range(0.05,1)) = 0.25

		[Header(Specular Parameters)]
		_SpecularSize ("Specular Size", Range(0,1)) = 0.1
		_SpecularFalloff ("Specular Falloff", Range(0, 2)) = 1
		_SpecularColor ("Specular Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags{ "RenderType"="Opaque" "Queue"="Geometry"}

        CGPROGRAM
        #pragma surface surf Stepped fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
		float  _Emmision;
		float4 _Color;
		float4 _EmmisionColor;
		float3 _ShadowTint;

		int _StepAmount;
		float _StepWidth;

		float _SpecularSize;
		float _SpecularFalloff;
		float3 _SpecularColor;

        struct Input
        {
            float2 uv_MainTex;
        };

		struct ToonSurfaceOutput
        {
            float2 uv_MainTex;
			fixed3 Albedo;
			half3 Emission;
			fixed3 Specular;
			fixed Alpha;
			fixed3 Normal;
        };

        void surf (Input IN, inout ToonSurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			c *= _Color;
			o.Albedo = c.rgb;

			o.Specular = _SpecularColor;

			float3 shadowColor = c.rgb * _ShadowTint;
			o.Emission = (_Emmision * _EmmisionColor) + shadowColor;
        }

		float4 LightingStepped(ToonSurfaceOutput s, float3 lightDir, half3 viewDir, float shadowAttenuation)
		{
			float towardsLight = dot(s.Normal, lightDir);
			towardsLight = towardsLight / _StepWidth;

			float lightIntensity = floor(towardsLight);

			
			float change = fwidth(towardsLight);
			float smoothing = smoothstep(0,change, frac(towardsLight));
			lightIntensity = lightIntensity + smoothing;

			lightIntensity = lightIntensity / _StepAmount;
			lightIntensity = saturate(lightIntensity);

			#ifdef USING_DIRECTIONAL_LIGHT
				float attenuationChange = fwidth(shadowAttenuation) * 0.5;
				float shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, shadowAttenuation);
			#else
				float attenuationChange = fwidth(shadowAttenuation);
				float shadow = smoothstep(0, attenuationChange, shadowAttenuation);
			#endif
				lightIntensity = lightIntensity * shadow;

			float3 shadowColor = s.Albedo * _ShadowTint;

			float reflectionDirection = reflect(lightDir, s.Normal);
			float towardsReflection = dot(viewDir, -reflectionDirection);

			float specularFalloff = dot(viewDir, s.Normal);
			specularFalloff = pow(specularFalloff, _SpecularFalloff);
			towardsReflection = towardsReflection * specularFalloff;

			float specularChange = fwidth(towardsReflection);
			float specularIntensity = smoothstep(1 - _SpecularSize, 1-_SpecularSize + specularChange, towardsReflection);
			specularIntensity = specularIntensity * shadow;

			float4 color;

			color.rgb = s.Albedo * lightIntensity* _LightColor0.rgb;
			color.rgb = lerp(color.rgb, s.Specular * _LightColor0.rgb, saturate(specularIntensity));

			color.a = s.Alpha;
			return color;
		}
        ENDCG
    }
    FallBack "Standard"
}
