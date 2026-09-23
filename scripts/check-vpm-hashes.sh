#!/usr/bin/env bash
set -euo pipefail

repo_root=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
hashes_file="$repo_root/scripts/vpm-sha256.tsv"
mkdir -p "$repo_root/.work"
index_file=$(mktemp "$repo_root/.work/vpm-index.XXXXXX")
trap 'rm -f "$index_file"' EXIT

checked=0
current_url=
while IFS=$'\t' read -r index_url package_id version expected_hash; do
  if [[ -z "$index_url" || "$index_url" == \#* ]]; then
    continue
  fi

  if [[ "$index_url" != "$current_url" ]]; then
    curl --fail --silent --show-error --location "$index_url" --output "$index_file"
    current_url=$index_url
  fi
  actual_hash=$(jq -r --arg id "$package_id" --arg version "$version" \
    '.packages[$id].versions[$version].zipSHA256 // empty' "$index_file")
  if [[ ! "$expected_hash" =~ ^[0-9a-f]{64}$ || "$actual_hash" != "$expected_hash" ]]; then
    printf 'VPM hash mismatch: %s %s (expected %s, actual %s)\n' \
      "$package_id" "$version" "$expected_hash" "$actual_hash" >&2
    exit 1
  fi
  found=0
  for project_name in world foliage avatar; do
    if jq -e --arg id "$package_id" --arg version "$version" \
      '.locked[$id].version == $version' \
      "$repo_root/projects/$project_name/Packages/vpm-manifest.json" >/dev/null; then
      found=1
      break
    fi
  done
  if ((found == 0)); then
    printf 'VPM package is not locked in a project: %s %s\n' \
      "$package_id" "$version" >&2
    exit 1
  fi
  checked=$((checked + 1))
done < "$hashes_file"

if ((checked != 11)); then
  printf 'Expected 11 package hashes, found %d\n' "$checked" >&2
  exit 1
fi
printf 'Verified %d VPM package hashes against public registries and locks\n' "$checked"
