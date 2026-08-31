#if UNITY_EDITOR
using UnityEditor;

namespace GatorDragonGames.JigglePhysics {

[InitializeOnLoad]
internal static class JigglePhysicsEditorCleanup {
    static JigglePhysicsEditorCleanup() {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state) {
        // Release native memory just before exiting Play mode,
        // regardless of whether there is a JiggleUpdateExample component in the scene.
        if (state == PlayModeStateChange.ExitingPlayMode) {
            JigglePhysics.Dispose();
        }
    }
}

}
#endif