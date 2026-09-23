#!/usr/bin/env bash
set -euo pipefail

repo_root=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
index_url=https://sabas0ba.github.io/vrc_sabaprops/index.json
hashes_file="$repo_root/scripts/props-vpm-sha256.tsv"
mkdir -p "$repo_root/.work"
index_file=$(mktemp "$repo_root/.work/props-vpm-index.XXXXXX")
trap 'rm -f "$index_file"' EXIT

curl --fail --silent --show-error --location "$index_url" --output "$index_file"

checked=0
while IFS=$'\t' read -r package_id version expected_hash; do
  if [[ -z "$package_id" || "$package_id" == \#* ]]; then
    continue
  fi

  actual_hash=$(jq -r --arg id "$package_id" --arg version "$version" \
    '.packages[$id].versions[$version].zipSHA256 // empty' "$index_file")
  if [[ ! "$expected_hash" =~ ^[0-9a-f]{64}$ || "$actual_hash" != "$expected_hash" ]]; then
    printf 'VPM hash mismatch: %s %s (expected %s, actual %s)\n' \
      "$package_id" "$version" "$expected_hash" "$actual_hash" >&2
    exit 1
  fi
  if ! jq -e --arg id "$package_id" --arg version "$version" \
    '.locked[$id].version == $version' \
    "$repo_root/projects/world/Packages/vpm-manifest.json" >/dev/null \
    && ! jq -e --arg id "$package_id" --arg version "$version" \
      '.locked[$id].version == $version' \
      "$repo_root/projects/foliage/Packages/vpm-manifest.json" >/dev/null; then
    printf 'VPM package is not locked in a project: %s %s\n' \
      "$package_id" "$version" >&2
    exit 1
  fi
  checked=$((checked + 1))
done < "$hashes_file"

if ((checked != 5)); then
  printf 'Expected 5 SabaProps package hashes, found %d\n' "$checked" >&2
  exit 1
fi
printf 'Verified %d SabaProps package hashes against %s\n' "$checked" "$index_url"
