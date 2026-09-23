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
    public static class SabaShaderDebugSampleAudit
    {
        private const string PackageName = "io.github.sabas0ba.sabashader";
        private const string PackageVersion = "0.5.0";
        private const string SampleName = "Debug Shader Demo";

        public static void Import()
        {
            Sample sample = FindSample();
            if (!sample.isImported && !sample.Import(Sample.ImportOptions.None))
                throw new InvalidOperationException("Debug Shader Demo import failed");
            AssetDatabase.Refresh();
            Debug.Log("[SabaShader Debug sample] Imported: " + sample.importPath);
        }

        public static void Inspect()
        {
            Sample sample = FindSample();
            if (!sample.isImported)
                throw new InvalidOperationException("Debug Shader Demo is not imported");

            string assetsRoot = Application.dataPath.Replace('\\', '/');
            string sampleRoot = sample.importPath.Replace('\\', '/');
            if (!sampleRoot.StartsWith(assetsRoot + "/", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Sample path is outside this project's Assets: " + sampleRoot);
            string scenePath = "Assets" + sampleRoot.Substring(assetsRoot.Length).TrimEnd('/')
                + "/DebugShaderDemo.unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) == null)
                throw new InvalidOperationException("Imported scene not found: " + scenePath);

            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            Component[] items = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Component>(true))
                .Where(item => item != null && item.GetType().Name == "DebugShaderDemoObject")
                .ToArray();
            if (items.Length != 18)
                throw new InvalidOperationException("Expected 18 Debug modes, found " + items.Length);

            var report = new StringBuilder();
            report.AppendLine("---\nlayout: default\ntitle: SabaShader Debug 公式サンプル\n---\n");
            report.AppendLine("# SabaShader Debug 公式サンプル\n");
            report.AppendLine("Unity 2022.3.22f1、SabaShader 0.5.0。Package Manager の `Debug Shader Demo` を Import し、18表示モードの一時 Material と Mesh を Editor で確認しました。VRChat クライアントの描画結果ではありません。\n");
            report.AppendLine("| Mode | オブジェクト | Mesh 頂点 | Shader 対応 |");
            report.AppendLine("| ---: | --- | ---: | --- |");

            foreach (Component item in items.OrderBy(item => item.gameObject.name, StringComparer.Ordinal))
            {
                MeshFilter filter = item.GetComponent<MeshFilter>();
                MeshRenderer renderer = item.GetComponent<MeshRenderer>();
                Mesh mesh = filter != null ? filter.sharedMesh : null;
                Material material = renderer != null ? renderer.sharedMaterial : null;
                if (mesh == null || material == null || material.shader == null ||
                    material.shader.name != "SabaShader/Debug" || !material.shader.isSupported)
                    throw new InvalidOperationException("Debug preview incomplete: " + item.name);
                int mode = material.GetInteger("_Mode");
                if (mode < 0 || mode > 17 || mesh.vertexCount == 0)
                    throw new InvalidOperationException("Invalid Debug mode: " + item.name);
                report.AppendLine($"| {mode} | {item.gameObject.name} | {mesh.vertexCount} | {material.shader.isSupported} |");
            }

            int[] modes = items.Select(item => item.GetComponent<MeshRenderer>().sharedMaterial.GetInteger("_Mode"))
                .OrderBy(mode => mode).ToArray();
            if (!modes.SequenceEqual(Enumerable.Range(0, 18)))
                throw new InvalidOperationException("Debug modes 0–17 are not unique and complete");

            string repository = Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
            string output = Path.Combine(repository, "docs", "reports", "sabashader-debug-sample.md");
            File.WriteAllText(output, report.ToString(), new UTF8Encoding(false));
            Debug.Log("[SabaShader Debug sample] Verified 18 modes: " + output);
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
