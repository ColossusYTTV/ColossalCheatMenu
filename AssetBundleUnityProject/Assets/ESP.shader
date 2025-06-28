Shader "Custom/ESP"
{
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
        }
    }
}