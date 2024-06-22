#ifndef GLOBAL_WARP_PROPERTIES_INCLUDED
#define GLOBAL_WARP_PROPERTIES_INCLUDED

bool _WarpEnabled = true;
float3 _WarpCenter;
float _WarpRadius;
float _WarpTransition;
int _currentRealm;
int _warpToRealm;

void WarpProperties_float(
    out bool warpEnabled,
    out float3 warpCenter, out float warpRadius, out float warpTransition,
    out int currentRealm, out int warpToRealm
) {
    warpEnabled = _WarpEnabled;
    warpCenter = _WarpCenter;
    warpRadius = _WarpRadius;
    warpTransition = _WarpTransition;
    currentRealm = _currentRealm;
    warpToRealm = _warpToRealm;
}

#endif
