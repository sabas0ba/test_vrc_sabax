using System;
using System.IO;
using System.Linq;
using System.Text;
using SabaProps.SoftProps.Editors;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SabaExample.Editor
{
    public static class SoftPropsGeneratorAudit
    {
        private static readonly (string name, int surfaces)[] Cases =
        {
            ("Futon", 1),
            ("Bed", 1),
            ("Sofa", 6),
            ("Cushion", 1),
            ("ContactProbeTest", 1),
        };

        public static void GenerateAndInspect()
        {
            string[] paths = Cases.Select(item =>
                SoftPropGenerator.PrefabFolder + "/" + item.name + ".prefab").ToArray();
            bool[] exists = paths.Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
                .ToArray();
            if (exists.All(value => !value))
            {
                SoftPropGenerator.GenerateAll();
                AssetDatabase.Refresh();
            }
            else if (exists.Any(value => !value))
            {
                throw new InvalidOperationException(
                    "Soft Props generation is incomplete; inspect existing assets before regeneration");
            }

            var report = new StringBuilder();
            report.AppendLine("---\nlayout: default\ntitle: Soft Props 公式 Generator\n---\n");
            report.AppendLine("# Soft Props 公式 Generator\n");
            report.AppendLine("Unity 2022.3.22f1、SabaProps Soft Props 0.2.0。公開 Generator API `GenerateAll` による5 Prefab を検査し、`CreateShowcase` と `CreateContactProbeTestInScene` を未保存シーンで実行しました。接触変形の Play Mode／VRChat クライアント結果ではありません。\n");
            report.AppendLine("| Prefab | 変形面 Controller | MeshFilter | Mesh 頂点合計 | Soft Surface Material |");
            report.AppendLine("| --- | ---: | ---: | ---: | ---: |");

            foreach (var item in Cases)
            {
                string path = SoftPropGenerator.PrefabFolder + "/" + item.name + ".prefab";
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null || PrefabUtility.GetPrefabAssetType(prefab) == PrefabAssetType.NotAPrefab)
                    throw new InvalidOperationException("Generated prefab not found: " + path);

                int controllers = prefab.GetComponentsInChildren<Component>(true)
                    .Count(component => component != null &&
                        component.GetType().Name == "SoftSurfaceContactController");
                MeshFilter[] filters = prefab.GetComponentsInChildren<MeshFilter>(true);
                int vertices = filters.Where(filter => filter.sharedMesh != null)
                    .Sum(filter => filter.sharedMesh.vertexCount);
                int softMaterials = prefab.GetComponentsInChildren<MeshRenderer>(true)
                    .SelectMany(renderer => renderer.sharedMaterials)
                    .Count(material => material != null && material.shader != null &&
                        material.shader.name == "SabaProps/Soft Surface");
                if (controllers != item.surfaces || filters.Length == 0 || vertices == 0 ||
                    softMaterials < item.surfaces)
                    throw new InvalidOperationException("Generated prefab is incomplete: " + item.name);
                report.AppendLine($"| {item.name} | {controllers} | {filters.Length} | {vertices} | {softMaterials} |");
            }

            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject showcase = SoftPropGenerator.CreateShowcase();
            if (showcase == null || showcase.transform.childCount != 4)
                throw new InvalidOperationException("Soft Props Showcase should contain four furniture objects");
            GameObject contactProbe = SoftPropGenerator.CreateContactProbeTestInScene();
            if (contactProbe == null || contactProbe.GetComponentsInChildren<Component>(true)
                    .All(component => component == null ||
                        component.GetType().Name != "SoftSurfaceContactController"))
                throw new InvalidOperationException("Contact Probe Test lacks a soft surface controller");
            report.AppendLine();
            report.AppendLine("未保存シーンで Showcase の子オブジェクト4件と Contact Probe Test の変形面 Controller を確認しました。シーンは保存していません。");

            string repository = Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
            string output = Path.Combine(repository, "docs", "reports", "softprops-generator.md");
            File.WriteAllText(output, report.ToString(), new UTF8Encoding(false));
            Debug.Log("[Soft Props generator audit] " + output);
        }
    }
}
