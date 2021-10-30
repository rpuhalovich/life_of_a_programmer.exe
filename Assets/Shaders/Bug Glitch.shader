Shader "Custom/Bug Glitch"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1, 1, 1, 1)
        _GlitchLength("Glitch Length in Seconds", Float)  = 0.5
        _DisplacementProb("Displacement Probability", Float) = 0.1
        _DisplacementInt("Displacement Intensity", Float) = 0.1
        _ColorProb("Color Probability", Float) = 0.1
        _ColorInt("Color Intensity", Float) = 0.1
    }

    SubShader
    {
        Tags
		{ 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _TintColor;

            v2f vert (appdata input)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.texcoord = input.texcoord;

                output.color = input.color * _TintColor;
                output.vertex = UnityPixelSnap(output.vertex);
                return output;
            }

            // uses arbitrary numbers in the cos function to generates a number between [0,1)
			float rand(float x, float y)
            {
				return frac(cos(x*12.9898 + y*78.233)*43758.5453);
			}

            float _GlitchLength;
			float _DisplacementProb;
			float _DisplacementInt;
			float _ColorProb;
			float _ColorInt;

            fixed4 frag(v2f input) : SV_Target
            {
                // glitches can only occur every '_GlitchLength' seconds
                float interval1 = floor(_Time.y / _GlitchLength) * _GlitchLength;

                // second interval increased arbitrarily for more randomness
                float interval2 = interval1 + 3.163;

                // considers interval time and the sprite's x and y positions
                float timePos1 = interval1 +  UNITY_MATRIX_MV[0][3] + UNITY_MATRIX_MV[1][3];
                float timePos2 = interval2 +  UNITY_MATRIX_MV[0][3] + UNITY_MATRIX_MV[1][3];

                // random chance for either displacement or color glitch
                float displacementRand = rand(timePos1, -timePos1);
                float colorRand = rand(timePos1, timePos1);

                // precalculated color shifts
                float randR = (rand(-timePos1, timePos1) - 0.5) * _ColorInt;
                float randG = (rand(-timePos1, -timePos1) - 0.5) * _ColorInt;
                float randB = (rand(-timePos2, timePos2) - 0.5) * _ColorInt;

                // splits the sprite into strips, random offset for each strip's height for more randomness
                float lineOffset = float((rand(timePos2, timePos2) - 0.5) / 50);

                // apply the displacement glitch if the random chance for it is less than its probability
				if(displacementRand < _DisplacementProb){
					input.texcoord.x += (rand(floor(input.texcoord.y / (0.2 + lineOffset)) - timePos1, floor(input.texcoord.y / (0.2 + lineOffset)) + timePos1) - 0.5) * _DisplacementInt;
					// prevent the texture coordinate from going into other parts of the texture by looping the coordinate between 0 and 1
					input.texcoord.x = fmod(input.texcoord.x, 1);
				}

                // sample the texture at the normal position and at the shifted color channel positions
				fixed4 normalColor = tex2D(_MainTex, input.texcoord);
				fixed4 shiftedR = tex2D(_MainTex, float2(input.texcoord.x + randR, input.texcoord.y + randR));
				fixed4 shiftedG = tex2D(_MainTex, float2(input.texcoord.x + randG, input.texcoord.y + randG));
				fixed4 shiftedB = tex2D(_MainTex, float2(input.texcoord.x + randB, input.texcoord.y + randB));
                fixed4 outputColor = fixed4(0.0,0.0,0.0,0.0);

                // apply the color glitch if the random chance for it is less than its probability
				// sets the output color to the shifted r, g, b channels and takes the average of their alpha
				if(colorRand < _ColorProb){
					outputColor.r = shiftedR.r;
					outputColor.g = shiftedG.g;
					outputColor.b = shiftedB.b;
					outputColor.a = (shiftedR.a + shiftedG.a + shiftedB.a) / 3;
				}
				else{
					outputColor = normalColor;
				}

                // apply the tint and tint color alpha
				outputColor.rgb *= input.color;
				outputColor.a *= input.color.a;
				outputColor.rgb *= outputColor.a;
				return outputColor;
            }
            ENDCG
        }
    }
}