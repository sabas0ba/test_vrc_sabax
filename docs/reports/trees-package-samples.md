---
layout: default
title: Trees 公式 Package Manager サンプル
---

# Trees 公式 Package Manager サンプル

Unity 2022.3.22f1、SabaProps Trees 0.1.0。Package Manager の `Trees Demo` を `Sample.Import` で取り込み、同梱3シーンの LOD 構成を Editor で検査しました。Game View の LOD 遷移速度や実測描画負荷は未検証です。

| シーン | GameObject | LODGroup | MeshRenderer | LOD Renderer | 3段階 LODGroup |
| --- | ---: | ---: | ---: | ---: | ---: |
| TreesDemo.unity | 191 | 45 | 141 | 135 | 45 |
| SeasonalTreesDemo.unity | 160 | 37 | 118 | 111 | 37 |
| ForestLoadDemo.unity | 776 | 192 | 579 | 576 | 192 |

LODGroup 数は各シーンに含まれる樹木オブジェクト数です。`ForestLoadDemo` は非アクティブの群も含めて192件を数えています。各 LOD の Renderer が存在することを検査しましたが、描画負荷や視覚品質を定量評価した結果ではありません。
