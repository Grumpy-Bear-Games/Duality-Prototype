using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DualityGame.Editor
{
    public class BlenderAssetPostprocessor : AssetPostprocessor
    {
        private const string IgnoreSuffix = "__IGNORE";
        
        private const string CustomColliderSuffix = "__CUSTOM_COLLIDER";
        private const string BoxColliderSuffix = "__BOX_COLLIDER";
        private const string NoColliderSuffix = "__NO_COLLIDER";

        public void OnPostprocessMeshHierarchy(GameObject g)
        {
            if (!assetPath.EndsWith(".fbx")) return;
            if (!g.name.EndsWith(IgnoreSuffix)) return;
            Debug.Log($"Deleting {g.name}");
            Object.DestroyImmediate(g);
        }

        public void OnPostprocessModel(GameObject gameObject)
        {
            Debug.Log(assetPath);
            if (!assetPath.EndsWith(".fbx")) return;
            
            HandleCustomColliders(gameObject);
            HandleSimpleColliders(gameObject);
        }

        private void HandleSimpleColliders(GameObject gameObject)
        {
            foreach (var meshFilter in gameObject.GetComponentsInChildren<MeshFilter>())
            {
                var name = meshFilter.name;
                if (name.EndsWith(NoColliderSuffix))
                {
                    meshFilter.name = name.Replace(NoColliderSuffix, "");
                    continue;
                }

                if (name.EndsWith(BoxColliderSuffix))
                {
                    meshFilter.gameObject.AddComponent<BoxCollider>();
                    meshFilter.name = name.Replace(BoxColliderSuffix, "");
                    continue;
                }
                
                if (meshFilter.GetComponent<MeshCollider>() != null) continue;
                
                var meshCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshFilter.sharedMesh;
            }
        }


        private void HandleCustomColliders(GameObject gameObject)
        {
            var meshFilters = gameObject.GetComponentsInChildren<MeshFilter>()
                .ToDictionary(filter => filter.gameObject.name);
            foreach (var meshFilter in meshFilters.Values.ToList())
            {
                var name = meshFilter.name; 
                if (!name.EndsWith(CustomColliderSuffix)) continue;
                
                var prefix = name.Replace(CustomColliderSuffix, "");
                if (!meshFilters.TryGetValue(prefix, out var parentMeshFilter))
                {
                    Debug.LogWarning($"Can't match collider {name} to GameObject in {assetPath}");
                    continue;
                }

                Debug.Log($"Making {meshFilter.name} the collider for {parentMeshFilter.name}");
                var meshCollider = parentMeshFilter.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshFilter.sharedMesh;

                meshFilters.Remove(name);
                Object.DestroyImmediate(meshFilter.gameObject);
            }
        }
    }
}
