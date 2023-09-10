Shader "Custom/DitherSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _DitherTex ("Dither Pattern (R)", 2D) = "white" {}
        _Alpha ("Alpha", Range(0.0, 1.0)) = 1.0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #include "UnityCG.cginc"
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 clipPos;
        };

        sampler2D _DitherTex;
        float _Alpha;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert(inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            // クリップ座標を求める
             o.clipPos = UnityObjectToClipPos(v.vertex);
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            // クリップ座標からビューポート座標を求める
            float2 viewPortPos = IN.clipPos.xy / IN.clipPos.w * 0.5 + 0.5;

            // スクリーン座標を求める
            float2 screenPos = viewPortPos * _ScreenParams.xy;

            // ディザリングテクスチャ用のUVを作成
            float2 ditherUv = screenPos / 4;

            float dither = tex2D(_DitherTex, ditherUv).r;
            clip(_Alpha - dither);
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}
