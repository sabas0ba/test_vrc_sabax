---
layout: default
---

# SabaTools Inspect Report

| | |
|---|---|
| Target | SoftPropsDemo |
| Mode | World |
| Findings | 0 error(s), 0 warning(s) |

## Geometry

| Item | Value |
|---|---|
| GameObjects | 115 |
| Triangles | 126,510 |
| Mesh Renderers | 85 |
| Skinned Mesh Renderers | 0 |
| Bones (unique) | 0 |
## Materials

| Item | Value |
|---|---|
| Material Slots | 85 |
| Unique Materials | 15 |
| Unique Shaders | 3 |
## Textures

| Item | Value |
|---|---|
| Textures | 2 |
| Estimated GPU Memory | 85.3 KB |
| UdonSharpProgramAsset icon (256x256 BC7, mips) | 85.3 KB |
| Font Texture (0x0 Alpha8) | 0 B |
## Dynamics

| Item | Value |
|---|---|
| PhysBones | 0 |
| PhysBone Colliders | 0 |
| Contacts | 16 |
| Constraints | 0 |
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
| Respawn Height Y | -10.0 |
| Lowest Geometry Y | -0.2 |
| Reference Camera | Main Camera |
| Udon Behaviours | 13 |
| Pickups | 3 |
| Stations | 0 |
| Mirrors (active / total) | 0 / 0 |

## Findings

- **Info** [World] 1 realtime (non-baked) light(s). Realtime lighting is a common world performance cost; consider baking.
- **Info** [World] 1 Camera component(s) under the target. Extra enabled cameras render every frame.
