---
layout: default
title: test_vrc_sabax
---

# test_vrc_sabax

公開 VPM パッケージの利用例と検証記録です。各機能は配布元の VPM リスティングから ALCOM (`vrc-get`) で導入します。実行結果が未確認の項目は、手順と期待結果のみを掲載しています。

1. [セットアップ](setup.html)
2. [対象一覧と公開版](status.html)
3. [SabaProps](sabaprops.html)
4. [SabaShader](sabashader.html)
5. [SabaTools](sabatools.html)
6. [SabaAccessory](sabaaccessory.html)
7. [観察結果と不具合の報告](reporting.html)
8. [観察記録](observations.html)

この `docs/` は [GitHub Actions workflow](https://github.com/sabas0ba/test_vrc_sabax/blob/main/.github/workflows/pages.yml) で Jekyll ビルドします。Pull Request ではビルドのみを実行し、`main` への反映後に Pages へデプロイします。公開先は [GitHub Pages](https://sabas0ba.github.io/test_vrc_sabax/) です。

2026-09-23 の `main` 反映後、トップ、各機能ページ、レポート、掲載画像の計21パスが HTTP 200 を返すことを確認しました。公開状態は [検査スクリプト](https://github.com/sabas0ba/test_vrc_sabax/blob/main/scripts/check-pages.sh)で再確認できます。
