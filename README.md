# JigglePhysics Unofficial Fixes

Unofficial memory leak fixes and warning cleanups for [JigglePhysics](https://github.com/naelstrof/JigglePhysics). 

---

## 🛠️ What's Fixed?

* **Domain Reload Memory Leak:** Fixed an issue where native containers (`NativeArray`, `NativeHashMap`, etc.) leaked memory when stopping Play mode with Domain Reload disabled. Added robust cleanup via `EditorApplication.playModeStateChanged`. 🧹
* **Compiler Warnings Cleaned Up (in `Packages/com.gator-dragon-games.jigglephysics`):**
  * Fixed `JiggleTreeJobData.cs` (`CS0660`/`CS0661`) by adding missing `IEquatable`, `Equals`, and `GetHashCode` implementations. 
  * Removed unused `hasWrittenData` field in `JiggleMemoryBus.cs` (`CS0414`). 

---

## 📥 Installation

1. Place `JigglePhysicsEditorCleanup.cs` anywhere in your Unity project (e.g., inside your `Assets/` folder). It uses `#if UNITY_EDITOR`, so it automatically stays out of final builds. 📂
2. Apply the package updates directly to `Packages/com.gator-dragon-games.jigglephysics`. 