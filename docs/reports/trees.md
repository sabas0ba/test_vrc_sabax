---
layout: default
---

# SabaTools Inspect Report

| | |
|---|---|
| Target | TreesDemo |
| Mode | World |
| Findings | 0 error(s), 1 warning(s) |

## Geometry

| Item | Value |
|---|---|
| GameObjects | 192 |
| Triangles | 3,533,534 |
| Mesh Renderers | 141 |
| Skinned Mesh Renderers | 0 |
| Bones (unique) | 0 |
## Materials

| Item | Value |
|---|---|
| Material Slots | 141 |
| Unique Materials | 4 |
| Unique Shaders | 3 |
## Textures

| Item | Value |
|---|---|
| Textures | 1 |
| Estimated GPU Memory | 0 B |
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
| Respawn Height Y | -100.0 |
| Lowest Geometry Y | -0.3 |
| Reference Camera | Main Camera |
| Udon Behaviours | 0 |
| Pickups | 0 |
| Stations | 0 |
| Mirrors (active / total) | 0 / 0 |

## Findings

- **Info** [World] 1 realtime (non-baked) light(s). Realtime lighting is a common world performance cost; consider baking.
- **Info** [World] 1 Camera component(s) under the target. Extra enabled cameras render every frame.
- **Warning** [World] The Reference Camera's near clip plane is 0.300. Anything above about 0.05 clips geometry a player can put their head into. — `Main Camera`
