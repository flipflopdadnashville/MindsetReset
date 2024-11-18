Shader "Explorer/Mandlebrot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area( "Area", vector) = (0, 0, 4, 4)
        _Angle ("Angle", range(-3.1415, 3.1415)) = 0
        _Color("Color", range(0,1)) = .9
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 _Area;
            float _Angle, _Color;
            sampler2D _MainTex;

            float2 rot(float2 p, float2 pivot, float a){
                float s = sin(a);
                float c = cos(a);
                
                p -= pivot;
                
                p = float2(p.x*c-p.y*s, p.x*s+p.y*c);
                p += pivot;

                return p;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                float2 c = _Area.xy + (i.uv-.5)*_Area.zw;
                c = rot(c, _Area.xy, _Angle);
                
                float2 z;
                float iter;
                for(iter = 0; iter < 255; iter++){
                    z = float2(z.x*z.x-z.y*z.y, 2*z.x*z.y) + c;
                    if(length(z) > 2) break;
                }

                float m = sqrt(iter / 255);
                float4 col = sin(float4(.3,.45,.65,1)*m*20)*.5+.5;
                col = tex2D(_MainTex, float2(m, _Color));
                return col;
            }
            ENDCG
        }
    }
}
