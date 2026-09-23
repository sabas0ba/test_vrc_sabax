using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SabaExample.Editor
{
    public static class SabaShaderShowcase
    {
        private const string OutputFolder = "Assets/Examples/SabaShader";
        private const string ScenePath = OutputFolder + "/CoreShaders.unity";

        public static void Create()
        {
            EnsureFolder("Assets/Examples");
            EnsureFolder(OutputFolder);

            Material illust = CreateMaterial("Illust2D", "SabaShader/Illust2D");
            Material debug = CreateMaterial("Debug", "SabaShader/Debug");
            Material standard = CreateMaterial("Standard", "Standard");

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) != null)
            {
                EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
                AssetDatabase.SaveAssets();
                Debug.Log("[SabaShader example] Updated materials for " + ScenePath);
                return;
            }

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateSphere("Standard", -2.5f, standard);
            CreateSphere("Illust2D", 0f, illust);
            CreateSphere("Debug", 2.5f, debug);

            var light = new GameObject("Directional Light").AddComponent<Light>();
            light.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(45f, -30f, 0f);

            var camera = new GameObject("Main Camera").AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.transform.position = new Vector3(0f, 2f, -8f);
            camera.transform.LookAt(new Vector3(0f, 1f, 0f));

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            Debug.Log("[SabaShader example] Created " + ScenePath);
        }

        private static Material CreateMaterial(string name, string shaderName)
        {
            Shader shader = Shader.Find(shaderName);
            if (shader == null)
                throw new InvalidOperationException("Shader not found: " + shaderName);

            string path = OutputFolder + "/" + name + ".mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }
            else
            {
                material.shader = shader;
                EditorUtility.SetDirty(material);
            }

            Color baseColor = new Color(0.45f, 0.78f, 0.85f, 1f);
            if (name == "Standard")
            {
                material.SetColor("_Color", baseColor);
            }
            else if (name == "Illust2D")
            {
                if (!material.HasProperty("_BaseColor"))
                    throw new InvalidOperationException("Illust2D has no _BaseColor");
                material.SetColor("_BaseColor", baseColor);
                material.SetColor("_Shade1Color", new Color(0.8f, 0.82f, 1f, 1f));
                material.SetColor("_Shade2Color", new Color(0.52f, 0.62f, 0.95f, 1f));
                material.SetFloat("_ShadeBlur1", 0.02f);
                material.SetFloat("_ShadeBlur2", 0.02f);
            }
            EditorUtility.SetDirty(material);

            Debug.Log("[SabaShader example] " + shaderName + ": supported=" + shader.isSupported);
            return material;
        }

        private static void CreateSphere(string name, float x, Material material)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = name;
            sphere.transform.position = new Vector3(x, 1f, 0f);
            sphere.transform.localScale = Vector3.one * 1.5f;
            sphere.GetComponent<Renderer>().sharedMaterial = material;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = path.Substring(0, path.LastIndexOf('/'));
            string name = path.Substring(path.LastIndexOf('/') + 1);
            AssetDatabase.CreateFolder(parent, name);
        }
    }
}
