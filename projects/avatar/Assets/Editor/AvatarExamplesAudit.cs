using System;
using System.IO;
using System.Text;
using SabaTools.Inspect;
using SabaTools.Inspect.Editors;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SabaExample.Editor
{
    public static class AvatarExamplesAudit
    {
        private const string HaloScene =
            "Assets/Samples/SabaAccessory Digital Halo/0.2.1/Digital Halo Demo/DigitalHaloDemo.unity";
        private const string RobotAvatarScene =
            "Packages/com.vrchat.avatars/Samples/Dynamics/Robot Avatar/Avatar Dynamics Robot Avatar PC.unity";

        public static void Run()
        {
            VerifyShader("SabaAccessory/Digital Halo");
            VerifyShader("SabaAccessory/Digital Halo Particle");

            string repository = Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
            string reports = Path.Combine(repository, "docs", "reports");
            Directory.CreateDirectory(reports);
            Export(HaloScene, InspectMode.Generic, Path.Combine(reports, "digital-halo.md"));
            Export(RobotAvatarScene, InspectMode.Avatar,
                Path.Combine(reports, "vrchat-robot-avatar.md"));
        }

        private static void VerifyShader(string name)
        {
            Shader shader = Shader.Find(name);
            if (shader == null)
                throw new InvalidOperationException("Shader not found: " + name);
            Debug.Log("[SabaAccessory example] " + name + ": supported=" + shader.isSupported);
        }

        private static void Export(string scenePath, InspectMode mode, string output)
        {
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            InspectionReport report = InspectApi.InspectScene(scene, mode);
            File.WriteAllText(output, "---\nlayout: default\n---\n\n" + report.ToMarkdown(),
                new UTF8Encoding(false));
            Debug.Log("[SabaTools example] " + scenePath + ": "
                + report.ErrorCount + " errors, " + report.WarningCount + " warnings");
        }
    }
}
