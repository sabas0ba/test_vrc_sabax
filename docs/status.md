---
layout: default
title: 対象一覧
---

# 対象一覧

2026-09-23 に各 GitHub Pages の `index.json` を取得して確認した公開版です。ALCOM の `vrc-get 1.9.1` で導入し、VPM lock と展開済みの `package.json` の版が一致することを確認しました。Unity 上の動作確認は別途記録します。

| 優先 | パッケージ | 導入版 | 使用先 | Unity 結果 |
| --- | --- | --- | --- | --- |
| 1 | SabaProps Foliage | 0.6.0／Trees 依存では 0.4.0 | foliage／world | サンプル生成・公式 Package Manager サンプル3シーン読込済み |
| 1 | SabaProps Trees | 0.1.0 | world | 公式サンプル3シーン読込、12種×3 LOD Mesh 生成・LODGroup 検査済み |
| 1 | SabaProps Put Items | 0.1.1 | world | デモ読込済み。対話的 Play Mode 操作は概ね期待どおりとの利用者報告あり |
| 1 | SabaProps Soft Props | 0.2.0 | world／PC | デモ読込・公式 Generator の5 Prefab 構成検査済み |
| 1 | SabaProps Water | 公開リスティングに無し | Worlds | 未実施 |
| 1 | SabaProps Stage Cam | 公開リスティングに無し | Worlds | 未実施 |
| 2 | SabaShader | 0.5.0（Shader Core 0.1.12） | world／avatar | 比較シーン、公式 Debug 18モード、Advanced 11項目を Editor で検査済み |
| 3 | SabaTools Inspect Core | 0.1.0 | world／avatar | 6件のシーン検査と Put Items の Target／Scene 比較済み |
| 3 | SabaTools Inspect for Avatars | 0.2.0 | avatar | 公式サンプルを検査済み |
| 3 | SabaTools Inspect for Worlds | 0.1.0 | world | 3シーンを検査済み |
| 3 | SabaTools Avatar Material Studio | 公開リスティングに無し | Avatars | 未実施 |
| 4 | SabaAccessory Digital Halo | 0.2.1 | avatar／PC | デモ読込・Editor 描画確認済み |

「公開リスティングに無し」はソースリポジトリに機能が無いことを意味しません。公開版の導入例は ALCOM／VPM から選択できるパッケージに限定します。未公開機能を検証する場合は、その目的とソース版の固定方法を別途記録します。

「生成済み」「読込済み」は Unity Editor 2022.3.22f1 での確認です。VRChat クライアント内の同期、接触、描画は未確認です。

参照した配布元: [SabaProps](https://sabas0ba.github.io/vrc_sabaprops/index.json)、[SabaShader](https://sabas0ba.github.io/vrc_sabashader/index.json)、[SabaTools](https://sabas0ba.github.io/vrc_sabatools/index.json)、[SabaAccessory](https://sabas0ba.github.io/vrc_sabaaccessory/index.json)。
