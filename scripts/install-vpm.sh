#!/usr/bin/env bash
set -euo pipefail

repo_root=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
world_project="$repo_root/projects/world"
avatar_project="$repo_root/projects/avatar"
foliage_project="$repo_root/projects/foliage"

install_package() {
  local project=$1
  local package_id=$2
  local version=$3

  if jq -e --arg id "$package_id" --arg version "$version" \
    '.dependencies[$id].version == $version' \
    "$project/Packages/vpm-manifest.json" >/dev/null; then
    return
  fi

  vrc-get install --yes --project "$project" "$package_id" "$version"
}

resolve_missing_packages() {
  local project=$1
  local package_id
  local locked_version
  local installed_version

  while IFS=$'\t' read -r package_id locked_version; do
    installed_version=$(jq -r '.version // empty' \
      "$project/Packages/$package_id/package.json" 2>/dev/null || true)
    if [[ "$installed_version" != "$locked_version" ]]; then
      vrc-get resolve --project "$project"
      return
    fi
  done < <(jq -r '.locked | to_entries[] | [.key, .value.version] | @tsv' \
    "$project/Packages/vpm-manifest.json")
}

vrc-get repo add https://sabas0ba.github.io/vrc_sabaprops/index.json SabaProps
vrc-get repo add https://sabas0ba.github.io/vrc_sabashader/index.json SabaShader
vrc-get repo add https://sabas0ba.github.io/vrc_sabatools/index.json SabaTools
vrc-get repo add https://sabas0ba.github.io/vrc_sabaaccessory/index.json SabaAccessory
vrc-get repo add https://lilxyzw.github.io/vpm-repos/vpm.json ShaderCore

install_package "$world_project" com.vrchat.worlds 3.10.4
install_package "$world_project" io.github.sabas0ba.sabaprops.trees 0.1.0
install_package "$world_project" io.github.sabas0ba.sabaprops.putitems 0.1.1
install_package "$world_project" io.github.sabas0ba.sabaprops.softprops 0.2.0
install_package "$world_project" io.github.sabas0ba.sabashader 0.5.0
install_package "$world_project" io.github.sabas0ba.sabatools.world 0.1.0

if [[ ! -f "$world_project/Packages/io.github.sabas0ba.sabaprops.foliage/package.json" ]]; then
  install_package "$world_project" io.github.sabas0ba.sabaprops.foliage 0.4.0
fi

install_package "$avatar_project" com.vrchat.avatars 3.10.4
install_package "$avatar_project" io.github.sabas0ba.sabashader 0.5.0
install_package "$avatar_project" io.github.sabas0ba.sabatools.avatar 0.2.0
install_package "$avatar_project" io.github.sabas0ba.sabaaccessory.digital-halo 0.2.1

install_package "$foliage_project" com.vrchat.worlds 3.10.4
install_package "$foliage_project" io.github.sabas0ba.sabaprops.foliage 0.6.0

resolve_missing_packages "$world_project"
resolve_missing_packages "$avatar_project"
resolve_missing_packages "$foliage_project"
