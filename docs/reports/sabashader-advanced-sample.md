---
layout: default
title: SabaShader Advanced 公式サンプル
---

# SabaShader Advanced 公式サンプル

Unity 2022.3.22f1、SabaShader 0.5.0。Package Manager の `Advanced Shader Suite Demo` を Import し、Shader Core で Decal、Surface Detail、Spatial Interior、Transition を有効にしました。11項目の生成 Mesh と Material を Editor で検査しました。Play Mode アニメーションと VRChat クライアント描画は未検証です。

| Feature | オブジェクト | Mesh 頂点 | 必須プロパティ | Shader 対応 |
| --- | --- | ---: | --- | --- |
| Decal UV | 00 Decal / UV Cylinder | 88 | `_io_github_sabas0ba_decal_Amount` | True |
| Decal Projection | 01 Decal / Projected Logo | 88 | `_io_github_sabas0ba_decal_Amount` | True |
| Skin Detail | 02 Surface / Skin | 515 | `_io_github_sabas0ba_surfacedetail_Amount` | True |
| Fabric Detail | 03 Surface / Fabric | 24 | `_io_github_sabas0ba_surfacedetail_Amount` | True |
| Spatial Universe Rift | 04 Spatial / Universe Rift | 515 | `_io_github_sabas0ba_spatialinterior_Amount` | True |
| Spatial Starfield | 05 Spatial / Starfield | 515 | `_io_github_sabas0ba_spatialinterior_Amount` | True |
| Spatial Cyber Back | 06 Spatial / Cyber Back | 515 | `_io_github_sabas0ba_spatialinterior_Amount` | True |
| Spatial Mud | 07 Spatial / Mud | 515 | `_io_github_sabas0ba_spatialinterior_Amount` | True |
| Upward Dissolve | 08 Transition / Upward | 550 | `_io_github_sabas0ba_transition_Progress` | True |
| Glitch Spawn | 09 Transition / Glitch | 550 | `_io_github_sabas0ba_transition_Progress` | True |
| Liquid Solid | 10 Transition / Liquid | 515 | `_io_github_sabas0ba_transition_Progress` | True |
