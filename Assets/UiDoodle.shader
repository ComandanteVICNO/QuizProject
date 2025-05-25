// UI Doodle Shader Effect - Fragment-based for Unity UI Images
// Based on Alan Zucconi's Sprite Doodle Shader
// Compatible with Unity 6.1
Shader "UI/Doodle"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        
        _ColorMask ("Color Mask", Float) = 15
        
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        [Space]
        _NoiseSnap  ("Noise Snap", Range(0.001,1)) = 0.1
        _NoiseScale ("Noise Scale", Range(0,0.02)) = 0.005
        _NoiseSpeed ("Noise Speed", Range(0,5)) = 1
        _RegionSize ("Region Size", Range(2,32)) = 8
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            // Random function that matches the original shader behavior
            float3 random3(float3 p)
            {
                return frac(sin(float3(
                    dot(p, float3(127.1, 311.7, 543.2)),
                    dot(p, float3(269.5, 183.3, 461.7)),
                    dot(p, float3(732.1, 845.3, 231.7))
                )) * 43758.5453);
            }

            inline float snap(float x, float snapValue)
            {
                return snapValue * round(x / snapValue);
            }

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float2 screenPos : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float _NoiseSnap;
            float _NoiseScale;
            float _NoiseSpeed;
            float _RegionSize;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color;
                
                // Pass screen position for noise calculation
                OUT.screenPos = ComputeScreenPos(OUT.vertex).xy;

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Snap time to create discrete random jumps (like the original)
                float time = snap(_Time.y * _NoiseSpeed, _NoiseSnap);
                
                // Sample multiple neighboring regions and blend them for smooth transitions
                float2 uv = IN.texcoord * _RegionSize;
                float2 gridUV = floor(uv) / _RegionSize;
                float2 fractUV = frac(uv);
                
                // Get displacement from 4 neighboring regions
                float3 seed00 = float3(gridUV, time);
                float3 seed10 = float3(gridUV + float2(1.0/_RegionSize, 0), time);
                float3 seed01 = float3(gridUV + float2(0, 1.0/_RegionSize), time);
                float3 seed11 = float3(gridUV + float2(1.0/_RegionSize, 1.0/_RegionSize), time);
                
                float2 disp00 = (random3(seed00).xy - 0.5) * 2.0 * _NoiseScale;
                float2 disp10 = (random3(seed10).xy - 0.5) * 2.0 * _NoiseScale;
                float2 disp01 = (random3(seed01).xy - 0.5) * 2.0 * _NoiseScale;
                float2 disp11 = (random3(seed11).xy - 0.5) * 2.0 * _NoiseScale;
                
                // Smooth interpolation using smoothstep for organic feel
                float2 smoothFract = smoothstep(0.0, 1.0, fractUV);
                
                // Bilinear interpolation between the 4 displacement values
                float2 displacement = lerp(
                    lerp(disp00, disp10, smoothFract.x),
                    lerp(disp01, disp11, smoothFract.x),
                    smoothFract.y
                );
                
                // Apply displacement to UV coordinates
                float2 distortedUV = IN.texcoord + displacement;
                
                // Sample texture with distorted UVs
                half4 color = (tex2D(_MainTex, distortedUV) + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

                return color;
            }
        ENDCG
        }
    }
}