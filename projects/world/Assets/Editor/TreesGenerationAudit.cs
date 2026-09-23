using System;
using System.IO;
using System.Linq;
using System.Text;
using SabaProps.Trees;
using SabaProps.Trees.Editors;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SabaExample.Editor
{
    public static class TreesGenerationAudit
    {
        public static void GenerateAndInspect()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            TreeMenu.CreateDefaultAssets();

            TreeSpecies[] species = Selection.objects.OfType<TreeSpecies>()
                .OrderBy(item => item.name, StringComparer.Ordinal).ToArray();
            if (species.Length != TreeAssetLibrary.AllBotanicalPresets.Length + 2)
                throw new InvalidOperationException("Unexpected TreeSpecies count: " + species.Length);

            var report = new StringBuilder();
            report.AppendLine("---\nlayout: default\ntitle: Trees デフォルトアセット生成\n---\n");
            report.AppendLine("# Trees デフォルトアセット生成\n");
            report.AppendLine("Unity 2022.3.22f1、SabaProps Trees 0.1.0。公開版の `Tools > SabaProps > Trees > Create Default Assets` を実行し、生成された Species と3段階 LOD Mesh を確認しました。\n");
            report.AppendLine("| Species | LOD0 頂点 | LOD1 頂点 | LOD2 頂点 |");
            report.AppendLine("| --- | ---: | ---: | ---: |");

            foreach (TreeSpecies item in species)
            {
                Mesh[] meshes = { item.lod0Mesh, item.lod1Mesh, item.lod2Mesh };
                if (meshes.Any(mesh => mesh == null || mesh.vertexCount == 0))
                    throw new InvalidOperationException("Missing LOD Mesh: " + item.name);
                report.AppendLine($"| {item.name} | {meshes[0].vertexCount} | {meshes[1].vertexCount} | {meshes[2].vertexCount} |");
            }

            TreeSpecies selected = species[0];
            Mesh[] before = { selected.lod0Mesh, selected.lod1Mesh, selected.lod2Mesh };
            string[] guids = before.Select(mesh =>
                AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(mesh))).ToArray();
            Mesh[] rebuilt = TreeAssetLibrary.WriteLodMeshes(selected);
            AssetDatabase.SaveAssets();
            for (int index = 0; index < rebuilt.Length; index++)
            {
                string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(rebuilt[index]));
                if (rebuilt[index] != before[index] || guid != guids[index])
                    throw new InvalidOperationException("LOD Mesh reference changed during rebuild: " + index);
            }

            GameObject tree = TreeAssetLibrary.CreateLodGroup(selected);
            LODGroup group = tree != null ? tree.GetComponent<LODGroup>() : null;
            if (group == null || group.GetLODs().Length != 3)
                throw new InvalidOperationException("Three-level LODGroup was not created");

            report.AppendLine();
            report.AppendLine($"`{selected.name}` の `Rebuild LOD Meshes` 相当処理後も3 Mesh の参照と GUID は不変でした。`Create LOD Group in Scene` 相当処理で3段階の `LODGroup` を生成しました。検証用シーンは保存していません。");

            string repository = Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
            string output = Path.Combine(repository, "docs", "reports", "trees-generation.md");
            File.WriteAllText(output, report.ToString(), new UTF8Encoding(false));
            Debug.Log("[SabaProps Trees generation] " + species.Length + " species, LODGroup=3, report=" + output);
        }
    }
}
