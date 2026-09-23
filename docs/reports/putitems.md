---
layout: default
---

# SabaTools Inspect Report

| | |
|---|---|
| Target | PutItemsKitchen |
| Mode | World |
| Findings | 0 error(s), 0 warning(s) |

## Geometry

| Item | Value |
|---|---|
| GameObjects | 224 |
| Triangles | 24,968 |
| Mesh Renderers | 179 |
| Skinned Mesh Renderers | 0 |
| Bones (unique) | 0 |
## Materials

| Item | Value |
|---|---|
| Material Slots | 179 |
| Unique Materials | 11 |
| Unique Shaders | 2 |
## Textures

| Item | Value |
|---|---|
| Textures | 2 |
| Estimated GPU Memory | 85.3 KB |
| UdonSharpProgramAsset icon (256x256 BC7, mips) | 85.3 KB |
| Font Texture (0x0 Alpha8) | 0 B |
## Components

| Item | Value |
|---|---|
| Particle Systems | 0 |
| Trail / Line Renderers | 0 / 0 |
| Cloth | 0 |
| Lights (realtime / total) | 1 / 1 |
| Reflection Probes | 0 |
| Audio Sources | 0 |
| Animators | 0 |
| Cameras | 1 |
## World

| Item | Value |
|---|---|
| Spawn Points | 1 |
| Respawn Height Y | -5.0 |
| Lowest Geometry Y | -0.2 |
| Reference Camera | Overview camera |
| Udon Behaviours | 46 |
| Pickups | 19 |
| Stations | 0 |
| Mirrors (active / total) | 0 / 0 |

## Findings

- **Info** [World] 1 realtime (non-baked) light(s). Realtime lighting is a common world performance cost; consider baking.
- **Info** [World] 1 Camera component(s) under the target. Extra enabled cameras render every frame.
