---
layout: default
title: Soft Props 公式 Generator
---

# Soft Props 公式 Generator

Unity 2022.3.22f1、SabaProps Soft Props 0.2.0。公開 Generator API `GenerateAll` による5 Prefab を検査し、`CreateShowcase` と `CreateContactProbeTestInScene` を未保存シーンで実行しました。接触変形の Play Mode／VRChat クライアント結果ではありません。

| Prefab | 変形面 Controller | MeshFilter | Mesh 頂点合計 | Soft Surface Material |
| --- | ---: | ---: | ---: | ---: |
| Futon | 1 | 1 | 4793 | 1 |
| Bed | 1 | 7 | 7563 | 1 |
| Sofa | 6 | 10 | 9120 | 6 |
| Cushion | 1 | 1 | 2286 | 1 |
| ContactProbeTest | 1 | 6 | 6111 | 1 |

未保存シーンで Showcase の子オブジェクト4件と Contact Probe Test の変形面 Controller を確認しました。シーンは保存していません。
