Shader "Custom/FastBlur"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "" {}
    }
        Subshader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            Fog { Mode off }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                half4 pos : POSITION;
                half2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos  : SV_POSITION;
                half2  uv  : TEXCOORD0;
            };

            sampler2D _MainTex;
            uniform half4 _MainTex_TexelSize;
            uniform half _BlurSize;

            /*
                    static const int BLUR_SAMPLE_COUNT = 4;
                    static const float2 BLUR_KERNEL[BLUR_SAMPLE_COUNT] = {
                        float2(-1.0, -1.0),
                        float2(-1.0, 1.0),
                        float2(1.0, -1.0),
                        float2(1.0, 1.0),
                    };
                    */

                    // ぼかしを強めに掛けたときのアーティファクトが気になる場合はこちらを使う
                    static const int BLUR_SAMPLE_COUNT = 8;
                    static const float2 BLUR_KERNEL[BLUR_SAMPLE_COUNT] = {
                        float2(-1.0, -1.0),
                        float2(-1.0, 1.0),
                        float2(1.0, -1.0),
                        float2(1.0, 1.0),
                        float2(-0.70711, 0),
                        float2(0, 0.70711),
                        float2(0.70711, 0),
                        float2(0, -0.70711),
                    };

                    v2f vert(appdata v)
                    {
                        v2f o;
                        o.pos = UnityObjectToClipPos(v.pos);
                        o.uv = v.uv;
                        return o;
                    }

                    fixed4 frag(v2f i) : COLOR
                    {
                        // 解像度が違っても同じ見え方にする
                        float2 scale = _BlurSize / 1000;
                        scale.y *= _MainTex_TexelSize.y / _MainTex_TexelSize.x;

                        half4 color = 0;
                        for (int j = 0; j < BLUR_SAMPLE_COUNT; j++) {
                            color += tex2D(_MainTex, i.uv + BLUR_KERNEL[j] * scale);
                        }
                        // 最後にサンプリング数で割る
                        color.rgb /= BLUR_SAMPLE_COUNT;
                        color.a = 1;
                        return color;
                    }

                    ENDCG
                }
    }

        Fallback Off
}
