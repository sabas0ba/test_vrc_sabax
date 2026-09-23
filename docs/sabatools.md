---
layout: default
title: SabaTools の利用例
---

# SabaTools の利用例

[配布元の導入手順](https://github.com/sabas0ba/vrc_sabatools#vcc-への追加)でリスティングを追加します。Worlds には `SabaTools Inspect for Worlds`、Avatars には `SabaTools Inspect for Avatars` を選びます。`Inspect Core` は依存として導入されます。

1. `Tools > SabaTools > Inspect Window` を開きます。
2. 検証対象のルートを `Target` に指定して `Scan Target` を実行します。
3. GameObject、Renderer、Material、Texture、Missing 参照の結果を保存します。
4. 対象シーンの変更前後の差分を確認し、Inspect がシーンを変更していないかを確認します。

`Scan Active Scene` も別ケースとして確認します。GPU メモリは推定値であり、実測値との不一致だけを不具合と判断しません。操作の詳細は [Inspect Core](https://github.com/sabas0ba/vrc_sabatools/blob/main/Packages/io.github.sabas0ba.sabatools.core/README.md) を参照してください。

2026-09-23、Unity 2022.3.22f1 で公開 API `InspectApi.InspectScene` を実行し、[Trees](reports/trees.html)、[Put Items](reports/putitems.html)、[Soft Props](reports/softprops.html)、[SabaShader](reports/sabashader.html)、[Digital Halo](reports/digital-halo.html)、[VRChat 公式 Robot Avatar](reports/vrchat-robot-avatar.html) の6レポートを生成しました。Robot Avatar は Avatar モード、Trees／Put Items／Soft Props は World モードで検査しました。Trees 以外は0エラー・0警告です。

`scripts/run-unity-examples.ps1 -VerifyReadOnly` で検査前後の6シーンファイルの SHA-256 を比較し、全て不変でした。これは保存済みシーンファイルが書き換わらないことの確認であり、Editor の一時的なメモリ状態まで保証するものではありません。

再生成時、Robot Avatar レポートでは同じ推定サイズのテクスチャ2件の行順だけが変わりました。集計値は不変です。[観察記録](observations.html#sabatools-同じサイズのテクスチャ行順が変わる)と[上流 Issue](https://github.com/sabas0ba/vrc_sabatools/issues/4)を参照してください。

## Target と Active Scene の検査範囲

Put Items 配布デモの `Put Items / Kitchen Demo` ルートに公開 API `InspectApi.Inspect` を適用し、`InspectApi.InspectScene` の結果と[比較](reports/sabatools-target-vs-scene.html)しました。Target は222 GameObject、Scene は224 GameObject です。Target 側だけ World の警告が1件出ました。`VRCSceneDescriptor` は選択したルートの外にあるためで、シーン全体の検査では警告0件です。これは検査範囲の違いであり、デモシーンの Descriptor 欠落や SabaTools の不具合とは判定しません。Inspect Window の `Scan Target`／`Scan Active Scene` ボタン自体は未操作です。
