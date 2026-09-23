---
layout: default
title: セットアップ
---

# セットアップ

## プロジェクト

Unity 2022.3.22f1 のプロジェクトを `projects/world/`、`projects/foliage/`、`projects/avatar/` に配置しています。ALCOM の CLI である `vrc-get` を [導入スクリプト](https://github.com/sabas0ba/test_vrc_sabax/blob/main/scripts/install-vpm.sh) から実行し、次の公開リスティングから固定版を導入します。

| リポジトリ | VPM URL | 主な用途 |
| --- | --- | --- |
| [SabaProps](https://github.com/sabas0ba/vrc_sabaprops) | `https://sabas0ba.github.io/vrc_sabaprops/index.json` | Worlds |
| [SabaShader](https://github.com/sabas0ba/vrc_sabashader) | `https://sabas0ba.github.io/vrc_sabashader/index.json` | Worlds／Avatars |
| [SabaTools](https://github.com/sabas0ba/vrc_sabatools) | `https://sabas0ba.github.io/vrc_sabatools/index.json` | Worlds／Avatars |
| [SabaAccessory](https://github.com/sabas0ba/vrc_sabaaccessory) | `https://sabas0ba.github.io/vrc_sabaaccessory/index.json` | PC Avatars |
| [Shader Core](https://github.com/lilxyzw/Shader-Core) | `https://lilxyzw.github.io/vpm-repos/vpm.json` | SabaShader の依存 |

SabaShader を導入する前に Shader Core のリスティングも追加します。VRChat 公式パッケージは `vrc-get` の既定リポジトリから取得します。SabaTools は Worlds に `world`、Avatars に `avatar` を追加します。両モジュールを一つのプロジェクトへ入れません。`core` は依存として解決されます。

Trees 0.1.0 は Foliage 0.4.0 を要求し、`vrc-get` は Foliage 0.6.0 との競合を報告します。このため、`projects/world/` では Trees と Foliage 0.4.0、`projects/foliage/` では Foliage 0.6.0 を検証します。`projects/avatar/` には Avatar SDK、SabaShader、SabaTools Avatar、SabaAccessory を導入します。

`Packages/manifest.json`、`Packages/vpm-manifest.json`、`Packages/packages-lock.json`、`ProjectSettings/`、自作の検証用 `Assets/` を管理対象とし、VPM が展開したパッケージ本体、配布デモからインポートしたアセット、SDK／UdonSharp が生成したアセット、`Library/` 等は除外します。各パッケージの固定版は [対象一覧](status.md) に記録します。

## 開発用ツール

開発環境は [sabas0ba/dotfiles](https://github.com/sabas0ba/dotfiles/tree/fc4cdecc02a6a95c81a259549d3fb9e7df18bb8f) の commit `fc4cdecc02a6a95c81a259549d3fb9e7df18bb8f` に基づく Nix コンテナです。同リポジトリの `CLAUDE.md`、`AGENTS.md`、`docs/development.md` を参照し、コンテナ内で `scripts/check-env.sh` を通してから作業します。同 flake の nixpkgs commit `597283ad8aa0b331c788e97c4c262d58877074ef` に固定した `vrc-get 1.9.1` を使用します。Unity Editor だけはホストの 2022.3.22f1 を使用します。

```sh
nix shell github:NixOS/nixpkgs/597283ad8aa0b331c788e97c4c262d58877074ef#vrc-get --command bash scripts/install-vpm.sh
```

このコマンドは dotfiles の開発シェルまたは同じ flake のコンテナ内で、作業ツリーを `/project` に配置して実行します。パッケージ本体は `vrc-get` が VPM リスティングから解決します。

クリーン clone に対して同じコマンドを実行し、Worlds 10件、Avatars 7件、Foliage 3件のロック済みパッケージが復元され、Git 管理対象の manifest に変更が出ないことを確認しました。再現時は [lock 照合スクリプト](https://github.com/sabas0ba/test_vrc_sabax/blob/main/scripts/check-vpm-locks.sh)を Nix 環境で実行します。

```sh
bash scripts/check-vpm-locks.sh
```

SabaProps の使用版は、公開リスティングが示す `zipSHA256` も [固定値](https://github.com/sabas0ba/test_vrc_sabax/blob/main/scripts/props-vpm-sha256.tsv)として記録しています。次の検査は現行リスティングのハッシュと各プロジェクトの VPM lock を照合します。同じ版のアーカイブが差し替わった場合は失敗します。これはアーカイブを再ダウンロードして照合する検査ではありません。

```sh
bash scripts/check-props-vpm.sh
```

パッケージ導入後、ホストの Unity Editor で次を実行すると、サンプルの生成・読込と SabaTools レポートの更新を行います。

```powershell
powershell -NoProfile -File scripts/run-unity-examples.ps1
```

このスクリプトは 2026-09-23 に通しで正常終了しました。隔離環境からホスト Unity を起動すると、ライセンスが有効でも LicensingClient の IPC 接続が拒否され、終了コード199になる場合があります。この環境では Unity バッチ処理を隔離外のホスト側で実行します。[観察記録](observations.md#隔離環境からの-unity-licensingclient-ipc-接続失敗)を参照してください。

Foliage と Trees の掲載画像も再生成する場合は、グラフィック出力が使えるホスト Unity で `-RenderImages` を指定します。

```powershell
powershell -NoProfile -File scripts/run-unity-examples.ps1 -RenderImages
```

配布サンプル由来の Foliage、Trees、Put Items、Soft Props、Digital Halo のシーン／アセットは Git 管理から除外しています。上記スクリプトが無い場合だけ生成またはインポートします。自作の Shader 比較シーンと検証コードは管理対象です。検査レポートは `docs/reports/` にあります。

## 記録

各ケースについて、Unity の正確な版、`vrc-get` が解決したパッケージ版、SDK 版、対象プラットフォーム、実行日時、期待結果、実際の結果を記録します。画像を掲載する場合は自分で作成したシーンのキャプチャを使用し、第三者素材のライセンスを確認します。未実施のケースを成功として記録しません。
