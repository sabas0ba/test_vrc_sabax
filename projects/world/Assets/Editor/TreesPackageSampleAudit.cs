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
    public static class TreesPackageSampleAudit
    {
        private const string PackageName = "io.github.sabas0ba.sabaprops.trees";
        private const string PackageVersion = "0.1.0";
        private static readonly string[] SceneNames =
        {
            "TreesDemo.unity",
            "SeasonalTreesDemo.unity",
            "ForestLoadDemo.unity",
        };

        public static void ImportAndInspect()
        {
            Sample sample = Sample.FindByPackage(PackageName, PackageVersion)
                .FirstOrDefault(item => item.displayName == "Trees Demo");
            if (string.IsNullOrEmpty(sample.displayName))
                throw new InvalidOperationException("Trees Demo sample not found in " + PackageVersion);
            if (!sample.isImported && !sample.Import(Sample.ImportOptions.None))
                throw new InvalidOperationException("Trees Demo Package Manager import failed");
            AssetDatabase.Refresh();

            string assetsRoot = Application.dataPath.Replace('\\', '/');
            string sampleRoot = sample.importPath.Replace('\\', '/');
            if (!sampleRoot.StartsWith(assetsRoot + "/", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Sample path is outside this project's Assets: " + sampleRoot);
            string pathRoot = "Assets" + sampleRoot.Substring(assetsRoot.Length).TrimEnd('/');

            var report = new StringBuilder();
            report.AppendLine("---\nlayout: default\ntitle: Trees 公式 Package Manager サンプル\n---\n");
            report.AppendLine("# Trees 公式 Package Manager サンプル\n");
            report.AppendLine("Unity 2022.3.22f1、SabaProps Trees 0.1.0。Package Manager の `Trees Demo` を `Sample.Import` で取り込み、同梱3シーンの LOD 構成を Editor で検査しました。Game View の LOD 遷移速度や実測描画負荷は未検証です。\n");
            report.AppendLine("| シーン | GameObject | LODGroup | MeshRenderer | LOD Renderer | 3段階 LODGroup |");
            report.AppendLine("| --- | ---: | ---: | ---: | ---: | ---: |");

            foreach (string name in SceneNames)
            {
                string scenePath = pathRoot + "/" + name;
                if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) == null)
                    throw new InvalidOperationException("Imported scene not found: " + scenePath);

                Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                GameObject[] roots = scene.GetRootGameObjects();
                int objects = roots.Sum(root => root.GetComponentsInChildren<Transform>(true).Length);
                LODGroup[] groups = roots.SelectMany(root => root.GetComponentsInChildren<LODGroup>(true))
                    .ToArray();
                int renderers = roots.Sum(root => root.GetComponentsInChildren<MeshRenderer>(true).Length);
                int lodRenderers = groups.Sum(group => group.GetLODs()
                    .Sum(lod => lod.renderers.Length));
                int threeLevelGroups = groups.Count(group => group.GetLODs().Length == 3 &&
                    group.GetLODs().All(lod => lod.renderers.Length > 0));
                if (groups.Length == 0 || threeLevelGroups != groups.Length)
                    throw new InvalidOperationException("Incomplete three-level LOD groups: " + scenePath);
                if (name == "ForestLoadDemo.unity" &&
                    (groups.Length != 192 || lodRenderers != 576))
                    throw new InvalidOperationException("Expected 192 load-demo trees and 576 LOD renderers");

                report.AppendLine($"| {name} | {objects} | {groups.Length} | {renderers} | {lodRenderers} | {threeLevelGroups} |");
                Debug.Log($"[SabaProps Trees sample] {name}: {groups.Length} LODGroup, {renderers} MeshRenderer");
            }

            report.AppendLine();
            report.AppendLine("LODGroup 数は各シーンに含まれる樹木オブジェクト数です。`ForestLoadDemo` は非アクティブの群も含めて192件を数えています。各 LOD の Renderer が存在することを検査しましたが、描画負荷や視覚品質を定量評価した結果ではありません。");

            string repository = Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
            string output = Path.Combine(repository, "docs", "reports", "trees-package-samples.md");
            File.WriteAllText(output, report.ToString(), new UTF8Encoding(false));
            Debug.Log("[SabaProps Trees sample] Report: " + output);
        }
    }
}
