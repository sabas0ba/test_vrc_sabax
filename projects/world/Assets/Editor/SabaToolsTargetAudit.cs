using System;
using System.IO;
using System.Linq;
using System.Text;
using SabaTools.Inspect;
using SabaTools.Inspect.Editors;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SabaExample.Editor
{
    public static class SabaToolsTargetAudit
    {
        private const string ScenePath =
            "Assets/SabaProps/PutItemsKitchenDemoV2/PutItemsKitchen.unity";

        public static void Inspect()
        {
            Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            GameObject target = scene.GetRootGameObjects()
                .SingleOrDefault(root => root.name == "Put Items / Kitchen Demo");
            if (target == null)
                throw new InvalidOperationException("Put Items demo root not found");

            InspectionReport targetReport = InspectApi.Inspect(target, InspectMode.World);
            InspectionReport sceneReport = InspectApi.InspectScene(scene, InspectMode.World);
            if (targetReport.Snapshot.GameObjectCount <= 0 ||
                targetReport.Snapshot.GameObjectCount > sceneReport.Snapshot.GameObjectCount ||
                targetReport.Snapshot.MeshRendererCount > sceneReport.Snapshot.MeshRendererCount)
                throw new InvalidOperationException("Target statistics exceed scene statistics");

            var report = new StringBuilder();
            report.AppendLine("---\nlayout: default\ntitle: SabaTools Target と Scene の比較\n---\n");
            report.AppendLine("# SabaTools Target と Scene の比較\n");
            report.AppendLine("Unity 2022.3.22f1、Inspect Core 0.1.0。Put Items の配布デモで公開 API `InspectApi.Inspect(target, World)` と `InspectApi.InspectScene(scene, World)` を実行しました。前者は `Put Items / Kitchen Demo` ルート以下、後者はシーン内の全ルートが対象です。Inspect Window のボタン操作を検証したものではありません。\n");
            report.AppendLine("| 指標 | Target | Scene |");
            report.AppendLine("| --- | ---: | ---: |");
            report.AppendLine($"| GameObjects | {targetReport.Snapshot.GameObjectCount} | {sceneReport.Snapshot.GameObjectCount} |");
            report.AppendLine($"| Mesh Renderers | {targetReport.Snapshot.MeshRendererCount} | {sceneReport.Snapshot.MeshRendererCount} |");
            report.AppendLine($"| Unique Materials | {targetReport.Snapshot.UniqueMaterialCount} | {sceneReport.Snapshot.UniqueMaterialCount} |");
            report.AppendLine($"| Errors | {targetReport.ErrorCount} | {sceneReport.ErrorCount} |");
            report.AppendLine($"| Warnings | {targetReport.WarningCount} | {sceneReport.WarningCount} |");
            report.AppendLine();
            report.AppendLine("## Target の警告");
            report.AppendLine();
            foreach (InspectionItem item in targetReport.Items
                .Where(item => item.Severity == InspectionSeverity.Warning))
            {
                report.AppendLine($"- {item.Category}: {item.Message}");
            }

            string repository = Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
            string output = Path.Combine(repository, "docs", "reports", "sabatools-target-vs-scene.md");
            File.WriteAllText(output, report.ToString(), new UTF8Encoding(false));
            Debug.Log("[SabaTools target audit] " + output);
        }
    }
}
