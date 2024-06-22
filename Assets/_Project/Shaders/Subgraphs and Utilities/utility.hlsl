//UNITY_SHADER_NO_UPGRADE
#ifndef UTILITY_INCLUDED
#define UTILITY_INCLUDED
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "./distanceFieldUtils.hlsl"

void Dilation_float(UnityTexture2D inputTexture, float2 uv, float kernelSize, out float4 outputValue) {
    const int kernelSizeCeil = ceil(kernelSize);

    outputValue = float4(0, 0, 0, 0);
    // Loop over the pixels in the dilation kernel
    for (int i = -kernelSizeCeil; i <= kernelSizeCeil; i++) {
        for (int j = -kernelSizeCeil; j <= kernelSizeCeil; j++) {
            const float dist = vectorLength(i,j);
            const float factor = 1 + (kernelSize - dist); 
            if (factor < 0) { continue; }
            
            // Calculate the texture coordinates of the current kernel pixel
            float2 kernelTexCoord = uv + (float2(float(i), float(j)) * inputTexture.texelSize.xy);

            if (all(kernelTexCoord >= 0) && all(kernelTexCoord.x <= 1))
            {
                // Read the binary value of the current kernel pixel from the input texture
                float4 pixelValue = inputTexture.Sample(inputTexture.samplerstate, kernelTexCoord) * min(factor, 1);
                outputValue = max(outputValue, pixelValue);
            }
        }
    }
}



void sdEllipse_float( float2 p, float2 ab, out float distance )
{
    // symmetry
    p = abs( p );
    
    // initial value
    float2 q = ab*(p-ab);
    float2 cs = normalize( (q.x<q.y) ? float2(0.01,1) : float2(1,0.01) );
    
    // find root with Newton solver
    for( int i=0; i<5; i++ )
    {
        float2 u = ab*float2( cs.x,cs.y);
        float2 v = ab*float2(-cs.y,cs.x);
        float a = dot(p-u,v);
        float c = dot(p-u,u) + dot(v,v);
        float b = sqrt(c*c-a*a);
        cs = float2( cs.x*b-cs.y*a, cs.y*b+cs.x*a )/c;
    }
    
    // compute final point and distance
    float d = length(p-ab*cs);
    
    // return signed distance
    distance = (dot(p/ab,p/ab)>1.0) ? d : -d;
}

void sdEllipse_half( half2 p, half2 ab, out half distance )
{
    // symmetry
    p = abs( p );
    
    // initial value
    half2 q = ab*(p-ab);
    half2 cs = normalize( (q.x<q.y) ? half2(0.01,1) : half2(1,0.01) );
    
    // find root with Newton solver
    for( int i=0; i<5; i++ )
    {
        half2 u = ab*half2( cs.x,cs.y);
        half2 v = ab*half2(-cs.y,cs.x);
        float a = dot(p-u,v);
        float c = dot(p-u,u) + dot(v,v);
        float b = sqrt(c*c-a*a);
        cs = half2( cs.x*b-cs.y*a, cs.y*b+cs.x*a )/c;
    }
    
    // compute final point and distance
    float d = length(p-ab*cs);
    
    // return signed distance
    distance = (dot(p/ab,p/ab)>1.0) ? d : -d;
}

void TextureSize_float(UnityTexture2D tex, out float2 size)
{
    size = float2(1,1);  //tex.texelSize.zw;
}

void TextureSize_half(UnityTexture2D tex, out half2 size)
{
    size = tex.texelSize.zw;
}

void SafeSample_float(UnityTexture2D inputTexture, float2 uv, out float4 value)
{
    if (any(uv < 0) || any(uv > 1))
    {
        value = 0;
    }
    else
    {
        value = inputTexture.Sample(inputTexture.samplerstate, uv);
    }
}

#endif //UTILITY_INCLUDED
