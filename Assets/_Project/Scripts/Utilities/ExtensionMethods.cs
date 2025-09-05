using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Utilities
{
    public static class ExtensionMethods
    {
        public static void PreventLoosingFocus(this VisualElement value)
        {
            value.RegisterCallback<PointerDownEvent>(evt =>
            {
                evt.StopImmediatePropagation();
                (evt.currentTarget as VisualElement)?.panel.focusController.IgnoreEvent(evt);
            });
        }

        public static Texture2D ScaleTextureGPU(this Texture src, int width, int height, FilterMode filter = FilterMode.Bilinear)
        {
            var desc = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32)
            {
                sRGB = (QualitySettings.activeColorSpace == ColorSpace.Linear)
            };
            var rt = RenderTexture.GetTemporary(desc);
            var prevActive = RenderTexture.active;

            try
            {
                var prevFilter = src.filterMode;
                src.filterMode = filter;
                Graphics.Blit(src, rt);
                src.filterMode = prevFilter;

                var linear = (QualitySettings.activeColorSpace == ColorSpace.Linear);
                var dst = new Texture2D(width, height, TextureFormat.RGBA32, false, linear);

                RenderTexture.active = rt;
                dst.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                dst.Apply(false, false);
                return dst;
            }
            finally
            {
                RenderTexture.active = prevActive;
                RenderTexture.ReleaseTemporary(rt);
            }
        }

        public static Texture2D RenderSpriteToTexture(this Sprite sprite, int width, int height, bool preserveAspect, Color background)
        {
            var tex = sprite.texture;
            if (tex == null) return null;

            // Compute source UVs (normalized) from the sprite's textureRect.
            var tr = sprite.textureRect; // pixel-space rect inside the texture
            var srcUV = new Rect(
                tr.x / tex.width,
                tr.y / tex.height,
                tr.width / tex.width,
                tr.height / tex.height
            );

            // Destination rect (pixel space in our canvas), aspect-fit & centered.
            var dstRect = new Rect(0, 0, width, height);
            if (preserveAspect)
            {
                var scale = Mathf.Min(width / tr.width, height / tr.height);
                var w = tr.width * scale;
                var h = tr.height * scale;
                dstRect = new Rect((width - w) * 0.5f, (height - h) * 0.5f, w, h);
            }

            var desc = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32)
            {
                sRGB = (QualitySettings.activeColorSpace == ColorSpace.Linear)
            };
            var rt = RenderTexture.GetTemporary(desc);

            var prevActive = RenderTexture.active;
            var prevWrap = tex.wrapMode;
            var prevFilter = tex.filterMode;

            try
            {
                RenderTexture.active = rt;
                GL.PushMatrix();
                GL.LoadPixelMatrix(0, width, height, 0);
                GL.Clear(true, true, background);

                // Avoid edge bleeding from neighboring atlas pixels.
                tex.wrapMode = TextureWrapMode.Clamp;
                tex.filterMode = FilterMode.Bilinear;

                // Draw the sprite's sub-rect scaled into dstRect
                Graphics.DrawTexture(dstRect, tex, srcUV, 0, 0, 0, 0);

                GL.PopMatrix();

                var linear = (QualitySettings.activeColorSpace == ColorSpace.Linear);
                var outTex = new Texture2D(width, height, TextureFormat.RGBA32, false, linear);
                outTex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                outTex.Apply(false, false);
                return outTex;
            }
            finally
            {
                tex.wrapMode = prevWrap;
                tex.filterMode = prevFilter;
                RenderTexture.active = prevActive;
                RenderTexture.ReleaseTemporary(rt);
            }
        }
    }
}
