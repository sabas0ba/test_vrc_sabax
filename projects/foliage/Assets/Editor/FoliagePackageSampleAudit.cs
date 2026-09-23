using System;
using System.IO;
using System.Linq;
using System.Text;
using SabaProps.Foliage;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SabaExample.Editor
{
    public static class FoliagePackageSampleAudit
    {
        private const string PackageName = "io.github.sabas0ba.sabaprops.foliage";
        private const string PackageVersion = "0.6.0";
        private static readonly string[] SceneNames =
        {
            "FoliageDemo.unity",
            "FoliageSpeciesDemo.unity",
            "FoliageLoadDemo.unity",
        };

        public static void ImportAndInspect()
        {
            Sample sample = Sample.FindByPackage(PackageName, PackageVersion)
                .FirstOrDefault(item => item.displayName == "Foliage Demo");
            if (string.IsNullOrEmpty(sample.displayName))
                throw new InvalidOperationException("Foliage Demo sample not found in " + PackageVersion);

            if (!sample.isImported && !sample.Import(Sample.ImportOptions.None))
                throw new InvalidOperationException("Package Manager sample import failed");
            AssetDatabase.Refresh();

            string assetsRoot = Application.dataPath.Replace('\\', '/');
            string sampleRoot = sample.importPath.Replace('\\', '/');
            if (!sampleRoot.StartsWith(assetsRoot + "/", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Sample path is outside this project's Assets: " + sampleRoot);
            string assetPathRoot = "Assets" + sampleRoot.Substring(assetsRoot.Length);

            var report = new StringBuilder();
            report.AppendLine("---\nlayout: default\ntitle: Foliage 公式 Package Manager サンプル\n---\n");
            report.AppendLine("# Foliage 公式 Package Manager サンプル\n");
            report.AppendLine("Unity 2022.3.22f1、SabaProps Foliage 0.6.0。`Sample.FindByPackage` と `Sample.Import` により Package Manager の「Foliage Demo」を導入し、各シーンを読み込んだ結果です。VRChat クライアントの実行結果ではありません。\n");
            report.AppendLine("| シーン | GameObject | MeshRenderer | FoliageField | SurfaceVine | RhizomePatch |");
            report.AppendLine("| --- | ---: | ---: | ---: | ---: | ---: |");

            foreach (string name in SceneNames)
            {
                string scenePath = assetPathRoot.TrimEnd('/') + "/" + name;
                if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) == null)
                    throw new InvalidOperationException("Imported scene not found: " + scenePath);

                Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                GameObject[] roots = scene.GetRootGameObjects();
                int objects = roots.Sum(root => root.GetComponentsInChildren<Transform>(true).Length);
                int renderers = roots.Sum(root => root.GetComponentsInChildren<MeshRenderer>(true).Length);
                int fields = roots.Sum(root => root.GetComponentsInChildren<FoliageField>(true).Length);
                int vines = roots.Sum(root => root.GetComponentsInChildren<SurfaceVine>(true).Length);
                int rhizomes = roots.Sum(root => root.GetComponentsInChildren<RhizomePatch>(true).Length);
                if (renderers == 0)
                    throw new InvalidOperationException("Imported scene has no MeshRenderer: " + scenePath);

                report.AppendLine($"| {name} | {objects} | {renderers} | {fields} | {vines} | {rhizomes} |");
                Debug.Log($"[SabaProps Foliage sample] {name}: {renderers} renderers");
            }

            report.AppendLine();
            report.AppendLine("MeshRenderer 数は描画コンポーネント数であり、植物の個体数ではありません。Merged Chunks では複数個体を一つの Renderer にまとめます。");

            string repository = Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
            string output = Path.Combine(repository, "docs", "reports", "foliage-package-samples.md");
            File.WriteAllText(output, report.ToString(), new UTF8Encoding(false));
            Debug.Log("[SabaProps Foliage sample] Report: " + output);
        }
    }
}
