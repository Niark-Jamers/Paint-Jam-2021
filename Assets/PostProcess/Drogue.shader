Shader "Hidden/Custom/Drogue"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float _Blend;
        float _RealTime;
        float _Disto;
        float _LSD;

        float3 pal( in float t, in float3 a, in float3 b, in float3 c, in float3 d )
        {
            return a + b*cos( 6.28318*(c*t+d) );
        }

        #define palette(t) pal(t, float3(0.5,0.5,0.5),float3(0.5,0.5,0.5),float3(1.0,1.0,1.0),float3(0.0,0.33,0.67) )

        // Precision-adjusted variations of https://www.shadertoy.com/view/4djSRW
        float hash(float p) { p = frac(p * 0.011); p *= p + 7.5; p *= p + p; return frac(p); }
        float hash(float2 p) {float3 p3 = frac(float3(p.xyx) * 0.13); p3 += dot(p3, p3.yzx + 3.333); return frac((p3.x + p3.y) * p3.z); }

        float noise1D(float x) {
            float i = floor(x);
            float f = frac(x);
            float u = f * f * (3.0 - 2.0 * f);
            return lerp(hash(i), hash(i + 1.0), u);
        }

        float noise2D(float2 x) {
            float2 i = floor(x);
            float2 f = frac(x);

            // Four corners in 2D of a tile
            float a = hash(i);
            float b = hash(i + float2(1.0, 0.0));
            float c = hash(i + float2(0.0, 1.0));
            float d = hash(i + float2(1.0, 1.0));

            // Simple 2D lerp using smoothstep envelope between the values.
            // return float3(lerp(lerp(a, b, smoothstep(0.0, 1.0, f.x)),
            //			lerp(c, d, smoothstep(0.0, 1.0, f.x)),
            //			smoothstep(0.0, 1.0, f.y)));

            // Same code, with the clamps in smoothstep and common subexpressions
            // optimized away.
            float2 u = f * f * (3.0 - 2.0 * f);
            return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
        }

        float fbm(float2 x) {
            // Add time
            x += _RealTime;
            float v = 0.0;
            float a = 0.5;
            float2 shift = 100;
            // Rotate to reduce axial bias
            float2x2 rot = float2x2(cos(0.5), sin(0.5), -sin(0.5), cos(0.50));
            for (int i = 0; i < 4; ++i) {
                v += a * noise2D(x);
                x = mul(rot, x) * 2.0 + shift;
                x += _RealTime * a;
                a *= 0.5;
            }
            return v;
        }

        float2x2 rot(in float theta)
        {
            return float2x2(cos(theta),-sin(theta), sin(theta), cos(theta));
        }

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float4 cameraColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            float2 uv = i.texcoord;
            float4 color;

            float2 noiseUV = uv * 10;
            float noiseX = fbm(noiseUV) * 2 - 1;
            float noiseY = fbm(noiseUV - 1000) * 2 - 1;

            float2 distortedUVs = uv + float2(noiseX, noiseY) * 0.04 * _Blend;

            float4 distortedColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, distortedUVs);

            if (_Disto)
                color = distortedColor;
            else
                color = cameraColor;

            float2 fuv = mul(uv * 4, rot(_RealTime));
            float2 fbm1 = float2(fbm(fuv), fbm(fuv + _RealTime*0.1 + 0.2));
            float2 fbm2 = float2(fbm(fuv + fbm1.x + 0.120*_RealTime), fbm(uv + fbm1.y - 0.220*_RealTime) );
            float thmod = (1+fbm2.x*fbm2.y*1.0/(4))/PI + 1.;
            float3 lsdColor = palette(thmod +_RealTime*0.2 + fbm2.y) * (fbm2.x*fbm2.x*fbm2.x*fbm2.x +0.7);

            if (_LSD > 0)
                color.rgb += lsdColor * _Blend - _Blend / 2;

            return color;
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}
