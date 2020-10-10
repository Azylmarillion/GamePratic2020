using UnityEngine;

namespace GamePratic2020.Tools.Performances {
    public class FramerateDebugger : MonoBehaviour {
        private void OnGUI() {
            GUILayout.Label($"{ 1.0f / Time.deltaTime}", new GUIStyle {fontSize = 50});
        }
    } 
}
