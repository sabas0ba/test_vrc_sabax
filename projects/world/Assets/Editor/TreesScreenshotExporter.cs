using System;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SabaExample.Editor
{
    public static class TreesScreenshotExporter
    {
        public static void Export()
        {
            EditorSceneManager.OpenScene(
                "Assets/SabaProps/TreesBundledDemo/TreesDemo.unity");
            Camera camera = Camera.main;
            if (camera == null)
                throw new InvalidOperationException("Trees demo has no MainCamera");

            const int width = 1280;
            const int height = 720;
            RenderTexture target = RenderTexture.GetTemporary(width, height, 24);
            RenderTexture previousActive = RenderTexture.active;
            RenderTexture previousTarget = camera.targetTexture;
            Texture2D screenshot = null;
            try
            {
                camera.targetTexture = target;
                camera.Render();
                RenderTexture.active = target;
                screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
                screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                screenshot.Apply(false);

                string repository = Path.GetFullPath(
                    Path.Combine(Application.dataPath, "../../.."));
                string output = Path.Combine(repository, "docs", "images",
                    "trees-demo.png");
                Directory.CreateDirectory(Path.GetDirectoryName(output));
                File.WriteAllBytes(output, screenshot.EncodeToPNG());
                Debug.Log("[SabaProps example] Screenshot: " + output);
            }
            finally
            {
                camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                if (screenshot != null)
                    UnityEngine.Object.DestroyImmediate(screenshot);
                RenderTexture.ReleaseTemporary(target);
            }
        }
    }
}
