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

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
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
                        Debug.LogWarning($"{gameObject.name} has the wrong layer", gameObject);
                    }
                }
            }
        }

        private static void OnSceneSaving(Scene scene, string path)
        {
            Undo.SetCurrentGroupName("Fix layers");
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
                        Debug.LogWarning($"Autofix: Setting {gameObject.name} layer to {realm.LevelLayer}", gameObject);
                        Undo.RecordObject(gameObject, "");
                        gameObject.layer = realm.LevelLayer;
                        EditorUtility.SetDirty(gameObject);
                    }
                }
            }
            Undo.CollapseUndoOperations(undoGroup);
        }

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
