Shader "Custom/ColorToAlpha"
{
    Properties
    {
        [HDR]_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _MinLuminance ("Min Luminance", Float) = 0.0
        _MaxLuminance ("Max Luminance", Float) = 1.0

        _ColorToAlpha("Color To Alpha", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags {"Queue" = "Geometry" "RenderType"="Opaque" }
        Blend Off 
        ZTest Off
        ZWrite Off
        Cull Off
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float4 _ColorToAlpha;
        float4 _AdditiveColor;
        float _MaxLuminance;
        float _MinLuminance;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float3 invLerp(float3 from, float3 to, float3 value) {
            return (value - from) / (to - from);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            float4 cTex = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 c = cTex * _Color;
            o.Albedo = c.rgb;

            if (all(cTex.rgb == _ColorToAlpha.rgb))
                clip(-1);
            else
                o.Alpha = c.a;

            o.Albedo *= IN.color;

            o.Albedo.rgb = invLerp(_MinLuminance, _MaxLuminance, o.Albedo.rgb);

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
