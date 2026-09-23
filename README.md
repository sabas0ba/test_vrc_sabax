# test_vrc_sabax

`vrc_sabaprops`、`vrc_sabashader`、`vrc_sabatools`、`vrc_sabaaccessory` の公開 VPM パッケージを、ALCOM の CLI である `vrc-get` 経由で利用する検証リポジトリです。操作例と再現可能な観察結果を公開します。

対象の優先順位は SabaProps → SabaShader → SabaTools → SabaAccessory です。検証は [Worlds](projects/world/)、[Foliage 0.6.0](projects/foliage/)、[Avatars](projects/avatar/) の各 Unity プロジェクトで行います。

| 対象 | 最初に確認する例 | 状態 |
| --- | --- | --- |
| [SabaProps](docs/sabaprops.md) | Foliage Demo、Trees Demo、Put Items、Soft Props | Unity Editor で生成・読込済み |
| [SabaShader](docs/sabashader.md) | Illust2D、Debug | 比較シーンを生成・Editor 描画確認済み |
| [SabaTools](docs/sabatools.md) | Inspect Core と World／Avatar モジュール | 6件の検査レポートを生成済み |
| [SabaAccessory](docs/sabaaccessory.md) | Digital Halo Demo | Unity Editor で読込・描画確認済み |

導入順とプロジェクト構成は [セットアップ](docs/setup.md) を参照してください。配布状態と検証の進捗は [対象一覧](docs/status.md)、確認した不整合は [観察記録](docs/observations.md)、不具合・違和感の記録方法は [報告方法](docs/reporting.md) に記載しています。

このリポジトリの記録は配布パッケージを変更しません。VRChat クライアント内の動作は未確認です。再現手順と証拠をここに保存し、対象リポジトリへの報告をリンクします。
