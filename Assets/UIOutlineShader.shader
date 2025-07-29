Shader "UI/URPOutlineImage"
{
    Properties
    {
        _MainTex ("Image", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness", Range(0,50)) = 2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off

        Pass
        {
            Name "OutlinePass"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // ✅ Correct URP-compatible texture declaration
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineThickness;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                float alpha = tex.a;

                // Screen-space pixel offset for thickness
                float2 pixelSize = _OutlineThickness / _ScreenParams.xy;
                float neighborAlpha = 0.0;

                // Sample neighbors to create outline
                neighborAlpha += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(pixelSize.x, 0)).a;
                neighborAlpha += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv - float2(pixelSize.x, 0)).a;
                neighborAlpha += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(0, pixelSize.y)).a;
                neighborAlpha += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv - float2(0, pixelSize.y)).a;

                if (alpha > 0.01)
                    return tex;                     // draw image
                else if (neighborAlpha > 0.01)
                    return _OutlineColor;           // draw outline
                else
                    return half4(0,0,0,0);          // transparent
            }
            ENDHLSL
        }
    }
}
