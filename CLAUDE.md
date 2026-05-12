# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Unity 6 (6000.3.11f1, URP) 3D top-down "vampire-survivors" style game. Player auto-attacks with multiple weapons, kills waves of enemies for EXP, picks upgrades on level-up. Comments and most identifiers in source are Korean; preserve that style when editing existing files.

There is no command-line test/build runner configured. Build and play through the Unity Editor (open `Assets/Scenes/GameStart.unity` or `SampleScene.unity`). A pre-built Windows binary lives in `Build/ToyProject.exe`.

`.editorconfig` enforces LF endings + final newline on `*.cs` — on Windows, be careful that editors don't rewrite to CRLF.

Branch naming convention: `features/<issue#>-<slug>`. Commit messages are written in Korean.

## Code Architecture

### Stat system (`Assets/Scripts/Stat/`)
Central to every entity. `BaseEntity` holds `Dictionary<StatType, StatContainer> stats`. Each `StatContainer` has a baseValue + a list of `StatModifier`s and lazily recomputes `FinalValue` when `isDirty`.

Modifier math order in `StatContainer.ReCalculate`:
1. Sum all `ModType.Flat` modifiers, add to base.
2. Sum all `ModType.Percent` + `ModCategory.Default` (additive percent), multiply once.
3. Each `ModType.Percent` + `ModCategory.Multiply` is applied as a *separate* multiplier (compounding).

Modifiers carry a `source` reference so they can be removed in bulk via `RemoveBySource(source)`. Use this rather than mutating values directly when stacking/unstacking buffs.

### Entity hierarchy
- `BaseEntity` (abstract MonoBehaviour) → defines stats dictionary, `currentHp`, `TakeDamage`, `Die` → `DieRoutine` → `OnDie` template.
- `PlayerStatus : BaseEntity` — has invincibility frames, hit FX, weapon-modifier cache (`weaponModifiers`) that re-applies passive stats to newly equipped weapons in `PlayerWeapon.Equip`.
- `BaseEnemy : BaseEntity` — drives NavMeshAgent toward `_player`, distance-gated attack via `attackDistance`/`attackInterval`. Subclasses (Assasin/Heavy/Ranged/Elite/Final/Suicide/Triple…) override `Move`/`DoAttak`/`Update`. Enemies return themselves to the pool in `OnDie` via the prefab handle set by `EnemySpawner.SetPrefab`.

`PlayerStatus.AddModifier` has a special path: if the stat key doesn't exist on the player, it's assumed to be a *weapon* stat — it's cached in `weaponModifiers` and forwarded to every currently equipped `WeaponBase`. New weapons replay the cache on `Equip`. Don't bypass this by directly mutating weapon stats from upgrade code.

### Weapons — two trees, only one is live
- `Assets/Scripts/Weapons/` — older base classes (`AreaWeaponBase`, `DirectinalWeaponBase`, `ElectronicStaffWeapon`, `ShotgunWeapon`, plus `MolotovWeapon/`, `SwordAura/` subfolders).
- `Assets/Scripts/Weapons-newScript/` — **the active system.** Contains `WeaponBase` (abstract), `WeaponManager` (singleton WeaponData→prefab map), `WeaponData` ScriptableObject, and the in-use concrete weapons (Shuriken, Slasher, Thunder, ExplosiveProjectile, ElectronicField…).

`WeaponBase` runs a single `_timer` that fires `Attack()` every `Cooldown` seconds while `IsActive` and `CanAttack`. New weapons subclass it and implement `Attack()`. Stats come from `WeaponData` via `InitStats()`; per-level upgrades push `StatModifier`s through `weapon.AddModifier(...)` (see `WeaponData.Apply` reading `levelStats[weapon.Level - 1]`).

`PlayerWeapon` caps active weapons at 6 (`IsFull`). `Equip` instantiates the prefab from `WeaponManager.Weapons[data]`, then replays `PlayerStatus.WeaponModifiers` onto the new instance before `Activate()`.

### Upgrade flow
`PlayerLevel.OnLevelUp` → `UpgradeManager.ShowUpgradeSelection` → `GetRandomChoices(3)` filtering out maxed weapons/passives → `UpgradeUI.Show(choices, ApplyUpgrade)`. Both `WeaponData` and `UpgradeItemData` implement `IUpgrade` and return their new level from `Apply` so the manager can fire `OnFirstGet` (icon unlocks) only on level 1. Starting weapon is chosen the same way via `ShowStartingWeaponSelection` on `Start`.

### Pooling
`PoolManager.Instance` (singleton) wraps `ObjectPool<Transform>` keyed by prefab `GameObject`. Always go through `PoolManager.Instance.Spawn(prefab, pos, rot)` / `Despawn(prefab, obj)` — many systems (enemy spawning, projectiles, damage popups, EXP drops, items) rely on `OnEnable`-based reset, so any pooled component must restore its state in `OnEnable` rather than `Awake`/`Start`. `BaseEntity.OnEnable` already resets `isDead` and `currentHp`; subclasses calling `base.OnEnable()` is mandatory.

`PoolManager` lazily creates pools for prefabs that weren't pre-configured (`preloadCount = 0`), so missing inspector entries don't crash but lose the warmup.

### Spawning
`EnemySpawner` runs wave-based spawning: each `Wave` has prefabs, count, interval, and chest count. `GetSpawnPos` rejects samples closer than `minSpawnDist` from the player and uses `NavMesh.SamplePosition` to keep enemies on the mesh. Returns `Vector3.positiveInfinity` as a "no valid position" sentinel — callers must check before using. `EnemySpawner2.cs` is a separate variant; check which is wired into the active scene before editing.

### Audio
`SFXManager.Instance` — hand-rolled 3D voice pool (`pool3D[]`) with min-interval deduping and per-clip concurrency cap (voice stealing). Call `Play3D(clip, pos, vol)` for world sounds, `Play2D(clip, vol)` for UI/global. `BgmManager` plays a sequential playlist coroutine; not a singleton.

### Data assets
ScriptableObjects under `Assets/EnemyData/`, `Assets/WeaponData/`, `Assets/UpgradeData/`, `Assets/ItemData/`, `Assets/UIData/`. Authored via the `Game/...` `CreateAssetMenu` entries on the respective classes.

## Gotchas

- `Assets/Scripts/Player/PlayerWeapon.cs` contains corrupted/mojibake Korean strings in `Debug.LogError` messages and comments (visible as `�` characters). Don't "fix" them blindly — they're encoding artifacts in the file as committed. Only touch if explicitly cleaning encoding.
- Multiple `TestScene-*.unity` scenes exist (one per developer initials: `kkh`, `mj`, `ss`, `map`). `GameStart.unity` and `SampleScene.unity` are the canonical entry scenes.
- `PoolManager.Spawn` returns the spawned `GameObject`, not a typed component — callers commonly `GetComponent<BaseEnemy>()` immediately after. If the prefab is missing the expected component this will null-ref silently.
- `BaseEnemy.Awake` calls `GameObject.FindWithTag("Player")` — the Player must exist before enemies spawn, and the Player GameObject must carry the `Player` tag.
