param(
    [string]$UnityEditor = 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Unity.exe',
    [switch]$RenderImages
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path -Parent $PSScriptRoot
$logs = Join-Path $repository '.work'
New-Item -ItemType Directory -Path $logs -Force | Out-Null

if (-not (Test-Path -LiteralPath $UnityEditor -PathType Leaf)) {
    throw "Unity Editor not found: $UnityEditor"
}

function Invoke-UnityExample {
    param(
        [string]$Project,
        [string]$Name,
        [string]$Method,
        [switch]$Graphics
    )

    $projectPath = Join-Path $repository "projects/$Project"
    $logPath = Join-Path $logs "unity-$Name.log"
    $mode = if ($Graphics) { '-batchmode -quit' } else { '-batchmode -nographics -quit' }
    $arguments = '{0} -projectPath "{1}" -logFile "{2}" -executeMethod {3}' -f $mode, $projectPath, $logPath, $Method
    $process = Start-Process -FilePath $UnityEditor -ArgumentList $arguments -Wait -PassThru -WindowStyle Hidden
    if ($process.ExitCode -ne 0) {
        throw "Unity failed for ${Name} ($($process.ExitCode)). Log: $logPath"
    }
    if (-not (Select-String -Path $logPath -Pattern 'Exiting batchmode successfully now!' -Quiet)) {
        throw "Unity success marker not found for ${Name}. Log: $logPath"
    }
    if (Select-String -Path $logPath -Pattern 'error CS[0-9]+|Compilation failed|Script compilation failed' -Quiet) {
        throw "Unity compilation error for ${Name}. Log: $logPath"
    }
}

$foliageScene = Join-Path $repository 'projects/foliage/Assets/SabaProps/Foliage/Samples/FoliageDemo.unity'
if (-not (Test-Path -LiteralPath $foliageScene)) {
    Invoke-UnityExample foliage foliage-scene SabaProps.Foliage.Editors.FoliageSampleScene.CreateAndOpen
}

$treesScene = Join-Path $repository 'projects/world/Assets/SabaProps/TreesBundledDemo/TreesDemo.unity'
if (-not (Test-Path -LiteralPath $treesScene)) {
    Invoke-UnityExample world trees-demo SabaProps.Trees.Editors.TreeBundledDemo.CreateAndOpen
}

$putItemsScene = Join-Path $repository 'projects/world/Assets/SabaProps/PutItemsKitchenDemoV2/PutItemsKitchen.unity'
if (-not (Test-Path -LiteralPath $putItemsScene)) {
    Invoke-UnityExample world putitems-demo SabaProps.PutItems.Editors.KitchenDemoSample.OpenDemo
}

$softPropsScene = Join-Path $repository 'projects/world/Assets/SabaProps/SoftPropsDemoMotion/SoftPropsDemo.unity'
if (-not (Test-Path -LiteralPath $softPropsScene)) {
    Invoke-UnityExample world softprops-demo SabaProps.SoftProps.Editors.SoftPropsDemo.OpenDemo
}

$shaderScene = Join-Path $repository 'projects/world/Assets/Examples/SabaShader/CoreShaders.unity'
if (-not (Test-Path -LiteralPath $shaderScene)) {
    Invoke-UnityExample world sabashader-example SabaExample.Editor.SabaShaderShowcase.Create
}
Invoke-UnityExample world sabatools-reports SabaExample.Editor.SabaToolsReportExporter.Export

$haloScene = Join-Path $repository 'projects/avatar/Assets/Samples/SabaAccessory Digital Halo/0.2.1/Digital Halo Demo/DigitalHaloDemo.unity'
if (-not (Test-Path -LiteralPath $haloScene)) {
    Invoke-UnityExample avatar digitalhalo-demo SabaAccessory.DigitalHalo.Editor.DigitalHaloDemoInstaller.ImportAndOpenForValidation
}
Invoke-UnityExample avatar avatar-audit SabaExample.Editor.AvatarExamplesAudit.Run

if ($RenderImages) {
    Invoke-UnityExample foliage foliage-screenshot SabaExample.Editor.FoliageScreenshotExporter.Export -Graphics
    Invoke-UnityExample world world-props-screenshots SabaExample.Editor.WorldPropsScreenshotExporter.Export -Graphics
}
