using System.IO;
using System.Text;
using SabaTools.Inspect;
using SabaTools.Inspect.Editors;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SabaExample.Editor
{
    public static class SabaToolsReportExporter
    {
        private static readonly (string scene, string report, InspectMode mode)[] Cases =
        {
            ("Assets/SabaProps/TreesBundledDemo/TreesDemo.unity", "trees.md", InspectMode.World),
            ("Assets/SabaProps/PutItemsKitchenDemoV2/PutItemsKitchen.unity", "putitems.md", InspectMode.World),
            ("Assets/SabaProps/SoftPropsDemoMotion/SoftPropsDemo.unity", "softprops.md", InspectMode.World),
            ("Assets/Examples/SabaShader/CoreShaders.unity", "sabashader.md", InspectMode.Generic),
        };

        public static void Export()
        {
            string repository = Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
            string reports = Path.Combine(repository, "docs", "reports");
            Directory.CreateDirectory(reports);

            foreach (var item in Cases)
            {
                Scene scene = EditorSceneManager.OpenScene(item.scene, OpenSceneMode.Single);
                InspectionReport report = InspectApi.InspectScene(scene, item.mode);
                File.WriteAllText(Path.Combine(reports, item.report),
                    "---\nlayout: default\n---\n\n" + report.ToMarkdown(),
                    new UTF8Encoding(false));
                Debug.Log("[SabaTools example] " + item.scene + ": "
                    + report.ErrorCount + " errors, " + report.WarningCount + " warnings");
            }
        }
    }
}
