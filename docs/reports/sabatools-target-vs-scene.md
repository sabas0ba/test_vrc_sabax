---
layout: default
title: SabaTools Target と Scene の比較
---

# SabaTools Target と Scene の比較

Unity 2022.3.22f1、Inspect Core 0.1.0。Put Items の配布デモで公開 API `InspectApi.Inspect(target, World)` と `InspectApi.InspectScene(scene, World)` を実行しました。前者は `Put Items / Kitchen Demo` ルート以下、後者はシーン内の全ルートが対象です。Inspect Window のボタン操作を検証したものではありません。

| 指標 | Target | Scene |
| --- | ---: | ---: |
| GameObjects | 222 | 224 |
| Mesh Renderers | 179 | 179 |
| Unique Materials | 11 | 11 |
| Errors | 0 | 0 |
| Warnings | 1 | 0 |

## Target の警告

- World: No VRCSceneDescriptor found on the target. A world scene needs one (or the VRChat SDK is not installed in this project).
