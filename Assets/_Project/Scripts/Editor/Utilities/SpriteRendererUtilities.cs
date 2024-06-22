using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DualityGame.Editor.Utilities
{
    public static class SpriteRendererUtilities
    {
        [MenuItem("Duality Game/Fix Sprite Renderers")]
        public static void FixSpriteRenderer()
        {
            var spriteRenderers = Selection.gameObjects
                .Select(go => go.GetComponent<SpriteRenderer>())
                .Where(sr => sr != null)
                .ToList();
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                EditorUtility.SetDirty(spriteRenderer.gameObject);
            }
        }

        [MenuItem("Duality Game/Find and fix misconfigured Sprite Renderers")]
        public static void FindMisconfiguredSpriteRenderers()
        {
            var spriteRenderers = Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);
            foreach (var spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer.shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.On) continue;

                if (!PrefabUtility.IsPartOfAnyPrefab(spriteRenderer))
                {
                    Debug.Log($"[{spriteRenderer.gameObject.name}] Not a prefab");
                    continue;
                }

                var originalSpriteRenderer = PrefabUtility.GetCorrespondingObjectFromOriginalSource(spriteRenderer);
                var assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(originalSpriteRenderer);
                Debug.Log($"Fixing {assetPath}", spriteRenderer.gameObject);

                using var editingScope = new PrefabUtility.EditPrefabContentsScope(assetPath);
                originalSpriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
            AssetDatabase.SaveAssets();
        }
    }
}
