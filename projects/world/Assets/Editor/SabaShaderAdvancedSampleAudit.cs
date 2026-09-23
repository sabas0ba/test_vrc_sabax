using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SabaExample.Editor
{
    public static class SabaShaderAdvancedSampleAudit
    {
        private const string PackageName = "io.github.sabas0ba.sabashader";
        private const string PackageVersion = "0.5.0";
        private const string SampleName = "Advanced Shader Suite Demo";
        private static readonly string[] FeatureNames =
        {
            "Decal UV", "Decal Projection", "Skin Detail", "Fabric Detail",
            "Spatial Universe Rift", "Spatial Starfield", "Spatial Cyber Back", "Spatial Mud",
            "Upward Dissolve", "Glitch Spawn", "Liquid Solid"
        };
        private static readonly string[] RequiredProperties =
        {
            "_io_github_sabas0ba_decal_Amount",
            "_io_github_sabas0ba_surfacedetail_Amount",
            "_io_github_sabas0ba_spatialinterior_Amount",
            "_io_github_sabas0ba_transition_Progress"
        };

        public static void Import()
        {
            Sample sample = FindSample();
            if (!sample.isImported && !sample.Import(Sample.ImportOptions.None))
                throw new InvalidOperationException(SampleName + " import failed");
            Shader shader = Shader.Find("SabaShader/Illust2D");
            if (shader == null)
                throw new InvalidOperationException("SabaShader/Illust2D not found");
            var probe = new Material(shader);
            try
            {
                if (RequiredProperties.Any(required => !probe.HasProperty(required)))
                {
                    AssetDatabase.ImportAsset(
                        "Packages/io.github.sabas0ba.sabashader/Shaders/Illust2D/Illust2D.scshader",
                        ImportAssetOptions.ForceUpdate);
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(probe);
            }
            AssetDatabase.Refresh();
            Debug.Log("[SabaShader Advanced sample] Imported: " + sample.importPath);
        }

        public static void Inspect()
        {
            Sample sample = FindSample();
            if (!sample.isImported)
                throw new InvalidOperationException(SampleName + " is not imported");

            string assetsRoot = Application.dataPath.Replace('\\', '/');
            string sampleRoot = sample.importPath.Replace('\\', '/');
            if (!sampleRoot.StartsWith(assetsRoot + "/", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Sample path is outside this project's Assets: " + sampleRoot);
            string scenePath = "Assets" + sampleRoot.Substring(assetsRoot.Length).TrimEnd('/')
                + "/AdvancedShaderSuiteDemo.unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) == null)
                throw new InvalidOperationException("Imported scene not found: " + scenePath);

            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            Component[] items = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Component>(true))
                .Where(item => item != null && item.GetType().Name == "AdvancedShaderDemoObject")
                .ToArray();
            if (items.Length != FeatureNames.Length)
                throw new InvalidOperationException("Expected 11 Advanced features, found " + items.Length);

            var report = new StringBuilder();
            report.AppendLine("---\nlayout: default\ntitle: SabaShader Advanced 公式サンプル\n---\n");
            report.AppendLine("# SabaShader Advanced 公式サンプル\n");
            report.AppendLine("Unity 2022.3.22f1、SabaShader 0.5.0。Package Manager の `Advanced Shader Suite Demo` を Import し、Shader Core で Decal、Surface Detail、Spatial Interior、Transition を有効にしました。11項目の生成 Mesh と Material を Editor で検査しました。Play Mode アニメーションと VRChat クライアント描画は未検証です。\n");
            report.AppendLine("| Feature | オブジェクト | Mesh 頂点 | 必須プロパティ | Shader 対応 |");
            report.AppendLine("| --- | --- | ---: | --- | --- |");

            var seen = new bool[FeatureNames.Length];
            foreach (Component item in items.OrderBy(item => item.gameObject.name, StringComparer.Ordinal))
            {
                var serialized = new SerializedObject(item);
                SerializedProperty property = serialized.FindProperty("feature");
                if (property == null || property.intValue < 0 || property.intValue >= FeatureNames.Length)
                    throw new InvalidOperationException("Invalid Advanced feature: " + item.name);
                int feature = property.intValue;
                if (seen[feature])
                    throw new InvalidOperationException("Duplicate Advanced feature: " + FeatureNames[feature]);
                seen[feature] = true;

                MeshFilter filter = item.GetComponent<MeshFilter>();
                MeshRenderer renderer = item.GetComponent<MeshRenderer>();
                Mesh mesh = filter != null ? filter.sharedMesh : null;
                Material material = renderer != null ? renderer.sharedMaterial : null;
                int propertyGroup = feature <= 1 ? 0 : feature <= 3 ? 1 : feature <= 7 ? 2 : 3;
                string required = RequiredProperties[propertyGroup];
                if (mesh == null || mesh.vertexCount == 0 || material == null ||
                    material.shader == null || material.shader.name != "SabaShader/Illust2D" ||
                    !material.shader.isSupported || !material.HasProperty(required))
                    throw new InvalidOperationException("Advanced preview incomplete: " + item.name + ", property " + required);
                report.AppendLine($"| {FeatureNames[feature]} | {item.gameObject.name} | {mesh.vertexCount} | `{required}` | {material.shader.isSupported} |");
            }

            if (seen.Any(value => !value))
                throw new InvalidOperationException("Advanced features are not complete");

            string repository = Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
            string output = Path.Combine(repository, "docs", "reports", "sabashader-advanced-sample.md");
            File.WriteAllText(output, report.ToString(), new UTF8Encoding(false));
            Debug.Log("[SabaShader Advanced sample] Verified 11 features: " + output);
        }

        private static Sample FindSample()
        {
            Sample sample = Sample.FindByPackage(PackageName, PackageVersion)
                .FirstOrDefault(item => item.displayName == SampleName);
            if (string.IsNullOrEmpty(sample.displayName))
                throw new InvalidOperationException(SampleName + " not found in " + PackageVersion);
            return sample;
        }
    }
}
