using System;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SabaExample.Editor
{
    public static class WorldExampleScreenshotExporter
    {
        public static void ExportProps()
        {
            Render("Assets/SabaProps/TreesBundledDemo/TreesDemo.unity",
                "trees-demo.png");
            Render("Assets/SabaProps/PutItemsKitchenDemoV2/PutItemsKitchen.unity",
                "putitems-demo.png");
            Render("Assets/SabaProps/SoftPropsDemoMotion/SoftPropsDemo.unity",
                "softprops-demo.png");
        }

        public static void ExportShader()
        {
            Render("Assets/Examples/SabaShader/CoreShaders.unity",
                "sabashader-core.png");
        }

        private static void Render(string scenePath, string fileName)
        {
            EditorSceneManager.OpenScene(scenePath);
            Camera camera = Camera.main;
            if (camera == null)
                throw new InvalidOperationException(scenePath + " has no MainCamera");

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
                    fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(output));
                File.WriteAllBytes(output, screenshot.EncodeToPNG());
                Debug.Log("[SabaExample] Screenshot: " + output);
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
