using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDK3.Components;

namespace SabaExample.Editor
{
    public static class WorldPlayModeSetup
    {
        private static readonly (string path, string spawn)[] Scenes =
        {
            ("Assets/SabaProps/TreesBundledDemo/TreesDemo.unity", null),
            ("Assets/SabaProps/PutItemsKitchenDemoV2/PutItemsKitchen.unity", "Spawn - dining area"),
            ("Assets/SabaProps/SoftPropsDemoMotion/SoftPropsDemo.unity", "Spawn"),
            ("Assets/Examples/SabaShader/CoreShaders.unity", null),
        };

        public static void Prepare()
        {
            foreach (var item in Scenes)
            {
                Scene scene = EditorSceneManager.OpenScene(item.path, OpenSceneMode.Single);
                Camera camera = Camera.main;
                if (camera == null)
                    throw new InvalidOperationException(item.path + " has no MainCamera");

                VRCSceneDescriptor descriptor = FindComponent<VRCSceneDescriptor>(scene);
                if (descriptor != null)
                {
                    Debug.Log("[SabaExample] Existing World Descriptor: " + item.path);
                    continue;
                }

                var holder = new GameObject("World Descriptor");
                descriptor = holder.AddComponent<VRCSceneDescriptor>();

                Transform spawn = FindTransform(scene, item.spawn);
                if (spawn == null)
                {
                    descriptor.transform.position = camera.transform.position;
                    spawn = descriptor.transform;
                }
                if (spawn == descriptor.transform)
                    descriptor.transform.rotation = Quaternion.Euler(
                        0f, camera.transform.eulerAngles.y, 0f);

                descriptor.spawns = new[] { spawn };
                descriptor.ReferenceCamera = camera.gameObject;
                EditorSceneManager.SaveScene(scene);
                Debug.Log("[SabaExample] Play Mode ready: " + item.path
                    + ", spawn=" + spawn.name);
            }
        }

        private static T FindComponent<T>(Scene scene) where T : Component
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                T component = root.GetComponentInChildren<T>(true);
                if (component != null)
                    return component;
            }
            return null;
        }

        private static Transform FindTransform(Scene scene, string name)
        {
            if (name == null)
                return null;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
                {
                    if (child.name == name)
                        return child;
                }
            }
            throw new InvalidOperationException(scene.path + " has no spawn: " + name);
        }
    }
}
