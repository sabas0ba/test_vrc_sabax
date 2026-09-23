#!/usr/bin/env bash
set -euo pipefail

repo_root=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
project_root=${1:-$repo_root}

for project_name in world avatar foliage; do
  project="$project_root/projects/$project_name"
  manifest="$project/Packages/vpm-manifest.json"
  jq -e '.locked | type == "object"' "$manifest" >/dev/null
  count=0

  while IFS=$'\t' read -r package_id expected_version; do
    package_json="$project/Packages/$package_id/package.json"
    if [[ ! -f "$package_json" ]]; then
      printf 'Missing package: %s (%s)\n' "$package_id" "$project_name" >&2
      exit 1
    fi
    actual_version=$(jq -r '.version // empty' "$package_json")
    if [[ "$actual_version" != "$expected_version" ]]; then
      printf 'Version mismatch: %s (%s): lock=%s installed=%s\n' \
        "$package_id" "$project_name" "$expected_version" "$actual_version" >&2
      exit 1
    fi
    count=$((count + 1))
  done < <(jq -r '.locked | to_entries[] | [.key, .value.version] | @tsv' "$manifest")

  printf '%s: verified %d locked packages\n' "$project_name" "$count"
done
