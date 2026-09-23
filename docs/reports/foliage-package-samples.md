---
layout: default
title: Foliage 公式 Package Manager サンプル
---

# Foliage 公式 Package Manager サンプル

Unity 2022.3.22f1、SabaProps Foliage 0.6.0。`Sample.FindByPackage` と `Sample.Import` により Package Manager の「Foliage Demo」を導入し、各シーンを読み込んだ結果です。VRChat クライアントの実行結果ではありません。

| シーン | GameObject | MeshRenderer | FoliageField | SurfaceVine | RhizomePatch |
| --- | ---: | ---: | ---: | ---: | ---: |
| FoliageDemo.unity | 30 | 28 | 0 | 3 | 1 |
| FoliageSpeciesDemo.unity | 102 | 90 | 0 | 0 | 0 |
| FoliageLoadDemo.unity | 74 | 69 | 0 | 0 | 0 |

MeshRenderer 数は描画コンポーネント数であり、植物の個体数ではありません。Merged Chunks では複数個体を一つの Renderer にまとめます。
