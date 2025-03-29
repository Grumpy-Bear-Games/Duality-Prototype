using System.Collections.Generic;
using System.Linq;
using DualityGame.Realm;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DualityGame.Editor.Realm
{
    [InitializeOnLoad]
    public class SceneValidation
    {
        static SceneValidation()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
            EditorSceneManager.sceneSaving += OnSceneSaving;
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode _) => ValidateRealmGameObjects(scene);

        private static void OnSceneSaving(Scene scene, string path) => ValidateRealmGameObjects(scene);

        private static void ValidateRealmGameObjects(Scene scene)
        {
            Undo.SetCurrentGroupName("Scene validation");
            var undoGroup = Undo.GetCurrentGroup();

            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                foreach (var realmVisible in rootGameObject.GetComponentsInChildren<RealmVisible>())
                {
                    var realm = GetRealm(realmVisible);
                    if (realm is null)
                    {
                        Debug.LogError($"Error in scene {scene.name}: Realm not set in RealmVisible component", realmVisible);
                        continue;
                    }

                    foreach (var gameObject in GetMisconfiguredRealmGameObjects(realmVisible))
                    {
                        Debug.LogWarning($"Autofix: Setting {gameObject.name} layer to {LayerMask.LayerToName(realm.LevelLayer)}", gameObject);
                        Undo.RecordObject(gameObject, "");
                        gameObject.layer = realm.LevelLayer;
                        EditorUtility.SetDirty(gameObject);
                    }

                    foreach (var light in realmVisible.GetComponentsInChildren<Light>(includeInactive: true))
                    {
                        if (light.cullingMask == realm.RealmMask) continue;
                        Debug.LogWarning($"Autofix: Setting {light.name} culling mask to {RealmMaskString(realm)}", light);
                        Undo.RecordObject(light, "");
                        light.cullingMask = realm.RealmMask;
                        EditorUtility.SetDirty(light.gameObject);
                    }
                }
            }
            Undo.CollapseUndoOperations(undoGroup);
        }

        private static string RealmMaskString(DualityGame.Realm.Realm realm) => $"[{LayerMask.LayerToName(realm.LevelLayer)}, {LayerMask.LayerToName(realm.PlayerLayer)}]";


        private static DualityGame.Realm.Realm GetRealm(RealmVisible realmVisible)
        {
            var serializedObject = new SerializedObject(realmVisible);
            var realmProperty = serializedObject.FindProperty(RealmVisible.Fields.Realm);
            return realmProperty.objectReferenceValue as DualityGame.Realm.Realm;
        }

        private static IEnumerable<GameObject> GetMisconfiguredRealmGameObjects(RealmVisible realmVisible)
        {
            var realm = GetRealm(realmVisible);
            if (realm is null)
            {
                return null;
            }

            return realmVisible.GetComponentsInChildren<Transform>(includeInactive: true)
                .Select(t => t.gameObject)
                .Where(gameObject => gameObject.gameObject.layer !=realm.LevelLayer);
        }
    }
}
