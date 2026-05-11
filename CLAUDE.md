# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Unity 6000.3.11f1 roguelike survival game (Vampire Survivors-style). Players fight waves of enemies, collect experience, and choose weapon/passive upgrades on level-up. Up to 6 weapons can be equipped simultaneously.

## Build & Development

This is a Unity project — there is no CLI build command for day-to-day scripting work. Use the Unity Editor directly:

- **Open project:** Unity Hub → Add → select repo root
- **Play in Editor:** Press Play in the Unity Editor (main scene: `Assets/Scenes/GameStart.unity`)
- **Test scenes:** Each developer has their own `TestScene-[name].unity` under `Assets/Scenes/`
- **Build:** Unity Editor → File → Build Settings

There are no automated test runners or linting tools configured. `StatTest.cs` is a manual in-editor test script.

## Architecture

### Entity Hierarchy

```
BaseEntity (abstract) — stat dictionary, damage calc with defense mitigation
├── PlayerStatus — HP, stat upgrades, invincibility frames, loot radius
└── BaseEnemy (abstract) — NavMeshAgent pathfinding, attack interval, knockback, death/loot
    └── NormalEnemy, HeavyEnemy, RangedEnemy, EliteEnemy, AssasinEnemy,
        SuicideEnemy, FinalEnemy, … (11 variants total)
```

### Stat System

All entities store stats as `Dictionary<StatType, StatContainer>` where `StatType` is a 14-value enum (MaxHp, Defense, Speed, Attack, Cool, KnockBack, Resistance, etc.).

`StatContainer` uses a dirty-flag lazy recalculation pattern. `StatModifier` structs are applied in order: **Flat → Percent(Default) → Percent(Multiply)**. This system is also used by `WeaponBase` for weapon stats.

### Weapon System

```
WeaponBase (abstract) — stat dict, cooldown loop, IUpgrade
├── ProjectileWeaponBase — spawns ProjectileBase via PoolManager
│   └── (most ranged weapons)
└── SlasherWeapon, ThunderWeapon, ShurikenWeapon, ElectronicFieldWeapon
```

`ProjectileBase` is the abstract poolable projectile. Concrete types: `SlashProjectile`, `ExplosiveProjectile`, `ThunderProjectile`, `ShurikenOrbit`, `ShurikenTrap`.

`ProjectileInitData` (struct) carries owner, direction, damage, speed, layer mask, size, and knockback from weapon to projectile at spawn time.

`Assets/Scripts/Weapons/` contains **legacy** weapon classes (ShotgunWeapon, MolotovWeapon, etc.) that are superseded by `Assets/Scripts/Weapons-newScript/`. Prefer the new system for any changes.

### Upgrade System

`IUpgrade` is implemented by both `WeaponData` and `UpgradeItemData` (ScriptableObjects). `UpgradeManager` (singleton) picks 3 random `IUpgrade` candidates on level-up and calls `Apply()` on the player's choice. `LevelStats` stores per-level `List<StatModifier>` plus a display description string.

### Object Pooling

`PoolManager` (singleton) holds a `Dictionary<GameObject, ObjectPool<T>>` and auto-creates missing pools on first `Spawn()` call. All projectiles and damage popups use this pool. Always return objects via `PoolManager.Despawn()` rather than `Destroy()`.

### Singletons

`WeaponManager`, `PoolManager`, `UpgradeManager`, `SFXManager` — all use the standard Unity singleton pattern.

### Key ScriptableObjects

| Asset type | Location | Defines |
|---|---|---|
| `WeaponData` | `Assets/Scripts/Weapons-newScript/` | damage, cooldown, range, speed, size, knockBack, existTime, projectileCount, maxLevel, levelStats |
| `EnemyData` | `Assets/Scripts/Enemy/` | maxHp, defense, speed, attack, attackDistance, attackInterval, resistance |
| `UpgradeItemData` | `Assets/Scripts/Upgrade/` | icon, itemName, maxLevel, per-level StatModifier lists |
| `ItemData` | `Assets/Scripts/Item/` | itemName, effectValue, prefab |

### Wave / Spawn System

`EnemySpawner` drives waves; each wave config specifies enemy types, count, spawn interval, and chest count. Chests spawn proportionally as the wave progresses. `BreakableObject` handles chest drops.

### Leveling Formula

Experience threshold per level: `100 × 1.2^(level − 1)`. Max weapon level: 8. Max passive item level: 5.

### Audio

`SFXManager` maintains a pool of 16 `AudioSource` components with voice-stealing. `BgmManager` loops a playlist. Use `SFXManager.Instance.Play(clip, position)` for spatial SFX.

## Code Conventions

- Comments and some variable names are in Korean — this is intentional for the team.
- `EnemySpawner2.cs` is unused; do not extend it.
- Avoid `Destroy()` for pooled objects (projectiles, items, popups) — always `Despawn()`.
