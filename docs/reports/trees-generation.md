---
layout: default
title: Trees デフォルトアセット生成
---

# Trees デフォルトアセット生成

Unity 2022.3.22f1、SabaProps Trees 0.1.0。公開版の `Tools > SabaProps > Trees > Create Default Assets` を実行し、生成された Species と3段階 LOD Mesh を確認しました。

| Species | LOD0 頂点 | LOD1 頂点 | LOD2 頂点 |
| --- | ---: | ---: | ---: |
| Deadwood | 3019 | 959 | 266 |
| DesertScrub | 5408 | 1552 | 416 |
| GinkgoAutumn | 71355 | 26168 | 9686 |
| GinkgoSummer | 71355 | 26168 | 9686 |
| HinokiCypress | 103639 | 43078 | 18292 |
| JapaneseCedar | 118759 | 48408 | 20208 |
| JapaneseMaple | 96785 | 33542 | 10952 |
| JapaneseRedPine | 64201 | 25676 | 10696 |
| JapaneseWhiteBirch | 17319 | 7156 | 3024 |
| JapaneseZelkova | 37767 | 15688 | 6736 |
| SomeiYoshinoSpring | 92318 | 30427 | 10536 |
| SomeiYoshinoSummer | 49184 | 16253 | 5884 |

`Deadwood` の `Rebuild LOD Meshes` 相当処理後も3 Mesh の参照と GUID は不変でした。`Create LOD Group in Scene` 相当処理で3段階の `LODGroup` を生成しました。検証用シーンは保存していません。
