Shader "Custom/IrregularPulse"
{
    Properties
    {
        _PulseAmplitude ("Pulse Amplitude", Range(0, 1)) = 0.1
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 1.0
        _NoiseScale ("Noise Scale", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _PulseAmplitude; 
            uniform float _PulseSpeed;    
            uniform float _NoiseScale;    

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float3 color : COLOR; 
            };

            float Noise(float3 pos) {
                return frac(sin(dot(pos, float3(12.9898, 78.233, 54.53))) * 43758.5453);
            }

            VertexOutput vert(VertexInput v) {
                VertexOutput o;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                float noise = Noise(worldPos * _NoiseScale + _Time.y * _PulseSpeed);

                float displacement = (noise - 0.5) * 2.0 * _PulseAmplitude;

                float3 displacedVertex = v.vertex.xyz + v.normal * displacement;

                o.pos = UnityObjectToClipPos(float4(displacedVertex, 1.0));

                o.color = float3(0.5 + displacement * 0.5, 0.5, 1.0);

                return o;
            }

            fixed4 frag(VertexOutput i) : SV_Target {
                return fixed4(i.color, 1.0);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
