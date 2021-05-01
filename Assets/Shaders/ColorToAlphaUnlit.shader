Shader "Unlit/ColorToAlphaUnlit"
{
    Properties {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _ColorToAlpha ("ColorToAlpha", Color) = (1, 1, 1, 1)
    }
    
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100
        Cull Off
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha 
        
        Pass {  
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog
                
                #include "UnityCG.cginc"
    
                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                    float4 color : COLOR;
                };
    
                struct v2f {
                    float4 vertex : SV_POSITION;
                    half2 texcoord : TEXCOORD0;
                    float4 color : COLOR;
                    UNITY_FOG_COORDS(1)
                };
    
                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                float4 _ColorToAlpha;

                v2f vert (appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.color = v.color;
                    return o;
                }

                fixed4 frag (v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;

                    if (all(length(col.rgb - _ColorToAlpha.rgb) < 0.04))
                        clip(-1);
                    return col * i.color;
                }
            ENDCG
        }
    }
 }
