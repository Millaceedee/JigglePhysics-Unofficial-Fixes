# JigglePhysics Unofficial Fixes
Unofficial memory leak fixes and warning cleanups for [JigglePhysics](https://github.com/naelstrof/JigglePhysics). 

---

## 🛠️ What's Fixed?

* **Domain Reload Memory Leak:** Fixed an issue where native containers (`NativeArray`, `NativeHashMap`, etc.) leaked memory when stopping Play mode with Domain Reload disabled. Added robust cleanup via `EditorApplication.playModeStateChanged`.
* **Compiler Warnings Cleaned Up (in `Packages/com.gator-dragon-games.jigglephysics`):**
  * Fixed `JiggleTreeJobData.cs` (`CS0660`/`CS0661`) by adding missing `IEquatable`, `Equals`, and `GetHashCode` implementations.
  * Removed unused `hasWrittenData` field in `JiggleMemoryBus.cs` (`CS0414`).

---

## 🔍 Root Cause

All of JigglePhysics's persistent native memory (`NativeArray`s, `NativeHashMap broadPhaseMap`, `NativeReference globalCell`, `TransformAccessArray`s) lives inside `JiggleMemoryBus`, and it's only ever released through one call chain: `JigglePhysics.Dispose()` → `JiggleJobs.Dispose()` → `_memoryBus.Dispose()`.

The problem is that **`JigglePhysics.Dispose()` is only ever called from a single place in the entire codebase** — `JiggleUpdateExample.OnApplicationQuit()`. If your scene doesn't happen to contain that specific example component (for instance, if you've written your own update driver instead of using the provided example), nothing ever disposes the native memory when you stop Play mode.

With Domain Reload disabled, the static `jobs` field — and everything it holds — survives the Stop untouched. `JigglePhysics.Initialize()` (marked `[RuntimeInitializeOnLoadMethod(SubsystemRegistration)]`) does call `jobs?.Dispose()`, but only at the *start* of the *next* Play session, to clean up after the previous one. The Editor's leak detector, however, takes its snapshot right when you exit Play — before that next `Initialize()` has a chance to run — so it flags the previous session's still-undisposed allocations as a leak.

This also explains why some users reported the leak appearing "sometimes, not always": it depends entirely on whether a component with that exact `OnApplicationQuit` call happens to be present and active in the scene when Play mode stops. Any custom setup without it will leak every time.

The fix in this repo (`JigglePhysicsEditorCleanup.cs`) stops relying on any particular MonoBehaviour being present, and instead hooks the cleanup directly into the Editor's play-mode lifecycle via `EditorApplication.playModeStateChanged`, so disposal always happens on exiting Play mode regardless of what's in the scene.

---

## 📥 Installation

1. Place `JigglePhysicsEditorCleanup.cs` anywhere in your Unity project (e.g., inside your `Assets/` folder). It uses `#if UNITY_EDITOR`, so it automatically stays out of final builds.
2. Apply the package updates directly to `Packages/com.gator-dragon-games.jigglephysics`.

---

## ⚠️ Disclaimer

*These workarounds are relatively untested and meant as a temporary solution for the editor. Use them at your own risk, and always back up your project first!*