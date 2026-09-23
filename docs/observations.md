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

[ルート README](https://github.com/sabas0ba/vrc_sabaprops/blob/2f9a39a4020f76b5b800c5280b1f8951784add4c/README.md) は `Tools > SabaProps > Foliage > Create Sample Scene` を案内しています。一方、[FoliageSampleScene.cs](https://github.com/sabas0ba/vrc_sabaprops/blob/2f9a39a4020f76b5b800c5280b1f8951784add4c/Packages/io.github.sabas0ba.sabaprops.foliage/Editor/FoliageSampleScene.cs) の `MenuItem` は `Tools/SabaProps/Debug/Foliage/Create Sample Scene` です。このリポジトリの[利用手順](sabaprops.md)にはコード上のメニュー名を記載しました。

Foliage 0.6.0 を Unity 2022.3.22f1 に導入し、対応する `FoliageSampleScene.CreateAndOpen` を実行してシーン生成を確認しました。メニューの GUI 表示自体は未確認です。

## 公開リスティングに含まれない機能

[対象一覧](status.md)に記載した Water、Stage Cam、Avatar Material Studio は、ソースリポジトリの説明に存在しますが、確認日の公開 VPM リスティングに含まれません。リリース状況として記録し、仕様上の公開範囲かどうかは未判定です。

## Trees 0.1.0 が Foliage 0.4.0 を要求する

- 確認日: 2026-09-23
- 対象: SabaProps Trees 0.1.0、Foliage 0.6.0／0.4.0
- 種別: 公開版の依存関係と `vrc-get` の解決結果
- 上流報告: [sabas0ba/vrc_sabaprops#20](https://github.com/sabas0ba/vrc_sabaprops/issues/20)

Trees 0.1.0 の公開パッケージは Foliage `0.4.0` を依存として指定しています。同じ Worlds プロジェクトに Foliage 0.6.0 を先に導入して Trees を追加すると、`vrc-get` は競合を表示し、lock と展開済みパッケージの Foliage を 0.4.0 に変更しました。Trees の [ソース側 README](https://github.com/sabas0ba/vrc_sabaprops/blob/main/Packages/io.github.sabas0ba.sabaprops.trees/README.md) は Foliage 0.6.0 に依存すると記載しています。

公開版を改変せず検証するため、Trees を含む `projects/world/` と Foliage 0.6.0 の `projects/foliage/` を分けました。両プロジェクトで Unity Editor によるシーン生成を確認しています。

## Unity ライセンス接続による再検証の停止

2026-09-23 の初回バッチ実行では、3プロジェクトでシーン生成・検査レポートの出力に成功しました。同日、再実行スクリプトの動作確認時にホストの Unity LicensingClient 接続が2回連続で60秒タイムアウトし、Unity は終了コード199を返しました。これはスクリプトの最初の Unity 起動時点で発生し、パッケージ処理に到達していません。ライセンス接続が回復した環境で `scripts/run-unity-examples.ps1` の通し再実行が必要です。
