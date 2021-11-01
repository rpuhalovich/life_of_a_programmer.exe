Shader "Custom/Fog"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (1, 1, 1, .5)
        _FogInt("Fog Intersection", float) = 1
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _CameraDepthTexture;
            float4 _FogColor;
            float _FogInt;
            float4 _IntersectionColor;

            // takes each vertex's position and record it as fog coordinates
            v2f vert (appdata input)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = ComputeScreenPos(output.vertex);
                UNITY_TRANSFER_FOG(output, output.vertex);
                return output;
            }

            float4 frag (v2f input) : SV_Target
            {
                // getting the linear eye depth (depth buffer value in the world space)
                float depth = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(input.uv)));
                // saturating the fog intensity * (linear eye depth - homogenous vertex coordinate)
                float diff = saturate(_FogInt * (depth - input.uv.w));
                // interpolates the fog color and uses a "breathable, smoothstep" curve as the weight
                // weight is quite specific, altering the values lead to inverted colors
                float4 outputColor = lerp(fixed4(_FogColor.rgb, 0.0), _FogColor, pow(diff, 3) * (diff * (6 * diff - 15) + 10));
                UNITY_APPLY_FOG(input.fogCoord, outputColor);

                return outputColor;
            }
            ENDCG
        }
    }
}

