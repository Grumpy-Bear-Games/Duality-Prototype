#ifndef VIRTUAL_CIRCLE_INCLUDED
#define VIRTUAL_CIRCLE_INCLUDED

void RaySphereIntersection_float(float3 rayOrigin, float3 rayDir, float3 sphereCenter, float sphereRadius, out float3 intersectionPoint, out bool hit, out float3 normal)
{
    // Compute coefficients of the quadratic equation
    const float3 oc = rayOrigin - sphereCenter;
    const float a = dot(rayDir, rayDir);
    const float b = 2.0 * dot(oc, rayDir);
    const float c = dot(oc, oc) - sphereRadius * sphereRadius;

    // Compute the discriminant
    const float discriminant = b * b - 4 * a * c;

    hit = discriminant > 0;
    if (!hit)
    {
        intersectionPoint = float3(0, 0, 0);
        normal = float3(0, 0, 0);
        return;
    }

    // Compute the nearest intersection point
    const float t = (-b + sqrt(discriminant)) / (2.0 * a);
    intersectionPoint = rayOrigin + t * rayDir;
    normal = (intersectionPoint - sphereCenter) / sphereRadius;
}

#endif
