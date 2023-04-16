#ifndef OUTLINES_INCLUDED
#define OUTLINES_INCLUDED
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"

static float2 sobelSamplePoints[9] = {
    float2(-1, 1), float2(0, 1), float2(1, 1), 
    float2(-1, 0), float2(0, 0), float2(1, 0),
    float2(-1, -1), float2(0, -1), float2(1, -1),
};

static float2 sobelKernel[9] = {
    float2(1, 1), float2(0, 2), float2(-1, 1),
    float2(2, 0), float2(0, 0), float2(-2, 0),
    float2(1, -1), float2(0, -2), float2(-1, -1),
};

void DepthAndNormalSobel_float(
    UnityTexture2D viewSpaceNormals, UnityTexture2D objectIds,
    float2 uv, float2 thickness,
    float depthThreshold, float normalThreshold, float objectIdThreshold,
    out float outEdge
) {
    // This function calculates the normal and depth sobels at the same time
    float2 sobelNormalX = 0;
    float2 sobelNormalY = 0;
    float2 sobelNormalZ = 0;
    float2 sobelDepth = 0;
    float2 sobelObjectIdX = 0;
    float2 sobelObjectIdY = 0;

    // We can unroll this loop to make it more efficient
    // The compiler is also smart enough to remove the i=4 iteration, which is always zero
    [unroll] for (int i = 0; i < 9; i++) {
        float2 sampleUV = uv + sobelSamplePoints[i] * thickness;
        float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(sampleUV);
        float4 normal = viewSpaceNormals.Sample(viewSpaceNormals.samplerstate, sampleUV);
        float4 objectId = objectIds.Sample(objectIds.samplerstate, sampleUV);
        // Create the kernel for this iteration
        float2 kernel = sobelKernel[i] * normal.w;
        // Accumulate samples for each channel
        sobelNormalX += normal.x * kernel;
        sobelNormalY += normal.y * kernel;
        sobelNormalZ += normal.z * kernel;
        sobelDepth += depth * kernel;
        sobelObjectIdX += objectId.x * kernel;
        sobelObjectIdY += objectId.y * kernel;
    }
    // Get the final sobel values by taking the maximum

    float depthEdge = length(sobelDepth) < 0.0000001f ? 0 : step(depthThreshold, length(sobelDepth));
    float normalEdge = step(normalThreshold, max(length(sobelNormalX), max(length(sobelNormalY), length(sobelNormalZ))));
    float objectIdEdge = step(objectIdThreshold, max(length(sobelObjectIdX), length(sobelObjectIdY)));

    outEdge = max(objectIdEdge, max(depthEdge, normalEdge));
}

#endif
