using UnityEngine;

namespace DualityGame.VFX
{
    public static class ShaderGlobals
    {
        private static readonly int WarpEnabledProperty = Shader.PropertyToID("_WarpEnabled");
        private static readonly int WarpCenterProperty = Shader.PropertyToID("_WarpCenter");
        private static readonly int RadiusProperty = Shader.PropertyToID("_WarpRadius");
        private static readonly int WarpToRealmProperty = Shader.PropertyToID("_warpToRealm");
        private static readonly int CurrentRealmProperty = Shader.PropertyToID("_currentRealm");
        private static readonly int WarpTransitionProperty = Shader.PropertyToID("_WarpTransition");
        private static readonly int PlayerPositionProperty = Shader.PropertyToID("_Player_Position");

        public static bool WarpEffectEnabled
        {
            get => Shader.GetGlobalInteger(WarpEnabledProperty) == 1;
            set => Shader.SetGlobalInteger(WarpEnabledProperty, value ? 1 : 0);
        }

        public static Vector3 WarpCenter
        {
            get => Shader.GetGlobalVector(WarpCenterProperty);
            set => Shader.SetGlobalVector(WarpCenterProperty, value);
        }

        public static float WarpRadius
        {
            get => Shader.GetGlobalFloat(RadiusProperty);
            set => Shader.SetGlobalFloat(RadiusProperty, value);
        }

        public static int CurrentRealm
        {
            get => Shader.GetGlobalInteger(CurrentRealmProperty);
            set => Shader.SetGlobalInteger(CurrentRealmProperty, value);
        }

        public static int WarpToRealm
        {
            get => Shader.GetGlobalInteger(WarpToRealmProperty);
            set => Shader.SetGlobalInteger(WarpToRealmProperty, value);
        }

        public static float WarpTransition
        {
            get => Shader.GetGlobalFloat(WarpTransitionProperty);
            set => Shader.SetGlobalFloat(WarpTransitionProperty, value);
        }

        public static Vector3 PlayerPosition
        {
            get => Shader.GetGlobalVector(PlayerPositionProperty);
            set => Shader.SetGlobalVector(PlayerPositionProperty, value);
        }

        public static void Reset()
        {
            WarpEffectEnabled = false;
            WarpCenter = Vector3.zero;
            WarpRadius = 0f;
            CurrentRealm = 0;
            WarpToRealm = 0;
            WarpTransition = 0f;
            PlayerPosition = Vector3.zero;
        }
    }
}
