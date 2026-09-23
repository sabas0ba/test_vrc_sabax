#!/usr/bin/env bash
set -euo pipefail

pages_base_url="${1:-https://sabas0ba.github.io/test_vrc_sabax}"
pages_base_url="${pages_base_url%/}"

paths=(
  /
  /setup.html
  /status.html
  /sabaprops.html
  /sabashader.html
  /sabatools.html
  /sabaaccessory.html
  /observations.html
  /reporting.html
  /reports/trees.html
  /reports/trees-generation.html
  /reports/foliage-package-samples.html
  /reports/putitems.html
  /reports/softprops.html
  /reports/sabashader.html
  /reports/sabashader-debug-sample.html
  /reports/sabashader-advanced-sample.html
  /reports/sabatools-target-vs-scene.html
  /reports/digital-halo.html
  /reports/vrchat-robot-avatar.html
  /images/foliage-demo.png
  /images/foliage-species-demo.png
  /images/trees-demo.png
  /images/putitems-demo.png
  /images/softprops-demo.png
  /images/sabashader-core.png
  /images/sabashader-debug-modes.png
  /images/sabashader-advanced-suite.png
  /images/digital-halo-demo.png
)

for path in "${paths[@]}"; do
  url="${pages_base_url}${path}"
  status="$(curl --location --silent --show-error --fail \
    --output /dev/null --write-out '%{http_code}' "$url")"
  if [[ "$status" != 200 ]]; then
    printf 'Unexpected HTTP status %s: %s\n' "$status" "$url" >&2
    exit 1
  fi
  printf '%s %s\n' "$status" "$path"
done

printf 'Verified %d published Pages paths\n' "${#paths[@]}"
