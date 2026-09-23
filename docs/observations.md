---
layout: default
title: 観察記録
---

# 観察記録

## SabaProps: Foliage サンプル生成メニューの案内が一致しない

- 確認日: 2026-09-23
- 対象: `vrc_sabaprops` の commit `2f9a39a4020f76b5b800c5280b1f8951784add4c`
- 種別: ドキュメントの不整合
- 上流報告: [sabas0ba/vrc_sabaprops#21](https://github.com/sabas0ba/vrc_sabaprops/issues/21)

[ルート README](https://github.com/sabas0ba/vrc_sabaprops/blob/2f9a39a4020f76b5b800c5280b1f8951784add4c/README.md) は `Tools > SabaProps > Foliage > Create Sample Scene` を案内しています。一方、[FoliageSampleScene.cs](https://github.com/sabas0ba/vrc_sabaprops/blob/2f9a39a4020f76b5b800c5280b1f8951784add4c/Packages/io.github.sabas0ba.sabaprops.foliage/Editor/FoliageSampleScene.cs) の `MenuItem` は `Tools/SabaProps/Debug/Foliage/Create Sample Scene` です。このリポジトリの[利用手順](sabaprops.html)にはコード上のメニュー名を記載しました。

Foliage 0.6.0 を Unity 2022.3.22f1 に導入し、対応する `FoliageSampleScene.CreateAndOpen` を実行してシーン生成を確認しました。メニューの GUI 表示自体は未確認です。

## 公開リスティングに含まれない機能

[対象一覧](status.html)に記載した Water、Stage Cam、Avatar Material Studio は、ソースリポジトリの説明に存在しますが、確認日の公開 VPM リスティングに含まれません。リリース状況として記録し、仕様上の公開範囲かどうかは未判定です。

## Trees 0.1.0 が Foliage 0.4.0 を要求する

- 確認日: 2026-09-23
- 対象: SabaProps Trees 0.1.0、Foliage 0.6.0／0.4.0
- 種別: 公開版の依存関係と `vrc-get` の解決結果
- 上流報告: [sabas0ba/vrc_sabaprops#20](https://github.com/sabas0ba/vrc_sabaprops/issues/20)

Trees 0.1.0 の公開パッケージは Foliage `0.4.0` を依存として指定しています。同じ Worlds プロジェクトに Foliage 0.6.0 を先に導入して Trees を追加すると、`vrc-get` は競合を表示し、lock と展開済みパッケージの Foliage を 0.4.0 に変更しました。Trees の [ソース側 README](https://github.com/sabas0ba/vrc_sabaprops/blob/main/Packages/io.github.sabas0ba.sabaprops.trees/README.md) は Foliage 0.6.0 に依存すると記載しています。

公開版を改変せず検証するため、Trees を含む `projects/world/` と Foliage 0.6.0 の `projects/foliage/` を分けました。両プロジェクトで Unity Editor によるシーン生成を確認しています。

## SabaTools: 同じサイズのテクスチャ行順が変わる

- 確認日: 2026-09-23
- 対象: SabaTools Inspect Core 0.1.0、VRChat 公式 Robot Avatar PC シーン
- 種別: レポート表示順の非決定性。集計値への影響なし
- 上流報告: [sabas0ba/vrc_sabatools#4](https://github.com/sabas0ba/vrc_sabatools/issues/4)

同じシーンを再検査すると、推定 GPU メモリがともに 2.7 MB の `BASE` と `BODY` のテクスチャ行が入れ替わりました。後続のバッチ再実行でも元の順へ戻りました。レポートの3件・推定合計5.4 MB・0エラー・0警告は変わりません。公開版の `TextureUsageCollector` は推定バイト数だけでソートしており、同値の二次キーを指定していません。検査結果の意味が変わったとは扱わず、Git 差分のノイズとして記録します。

## SabaShader 0.5.0 のライセンス表記の相違

- 確認日: 2026-09-23
- 対象: VPM 公開版 SabaShader 0.5.0
- 種別: 同一パッケージ内の文書とメタデータの表記差
- 上流報告: [sabas0ba/vrc_sabashader#17](https://github.com/sabas0ba/vrc_sabashader/issues/17)

展開済みパッケージの `README.md` には MIT と記載される一方、同梱の `LICENSE.md` は Apache License 2.0、`package.json` の `license` は `Apache-2.0` でした。利用条件の解釈はここでは行わず、公開版の記載不一致として報告しました。

## 隔離環境からの Unity LicensingClient IPC 接続失敗

2026-09-23、隔離された実行環境からホスト Unity をバッチ起動すると、LicensingClient の IPC チャネル接続が60秒でタイムアウトし、終了コード199を返しました。Unity Hub 上のアカウントとライセンスは有効です。同じ Unity 2022.3.22f1 とプロジェクトを隔離外で起動すると終了コード0で正常終了し、`scripts/run-unity-examples.ps1` も通しで成功しました。したがってパッケージの不具合ではなく、この作業環境のプロセス隔離と LicensingClient IPC の組合せによる制約です。Unity Editor のみ、IPC にアクセスできるホスト環境で実行します。

## 無人バッチ Play Mode での ClientSim 入力例外

2026-09-23、ホスト Unity 2022.3.22f1 で Put Items の配布デモを `-batchmode` の Play Mode に入り、ClientSim の起動ログと初期化ログを確認しました。その後、`ClientSimPlayerController.GetMovementInput` で `NullReferenceException` が繰り返され、バッチ処理は正常終了できませんでした。当時のプロジェクト設定は Active Input Handling が旧 Input Manager のみでした。ClientSim Settings は旧・新両 Input System の有効化と Unity 再起動を要求しており、その後プロジェクト設定が「Both」に変更されました。利用者は対話的な Put Items の Play Mode で Pickup、配置時の補正、お盆の動作が概ね期待どおりと報告しています。ただしバッチモードを同じ設定で再検証していないため、例外の原因を確定したとは扱いません。VRChat クライアント内の挙動も未確認です。

## World Descriptor 保存時の SceneTemplate 例外

2026-09-23、`projects/world/` のシーンへ World Descriptor を追加して保存した際、Unity Editor の `UnityEditor.SceneTemplate.SceneTemplateAsset.CreatePipeline` から `ArgumentNullException` が記録されました。シーン保存とバッチ実行自体は成功し、次回起動後に Descriptor を読み取れています。発生源は Unity の SceneTemplate 処理であり、SabaProps の不具合とは断定しません。既存 Descriptor があるシーンは再保存しないよう準備処理を変更しました。
