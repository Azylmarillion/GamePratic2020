using UnityEngine;

public class FreezeZ : MonoBehaviour {
    private float baseZ = 0f;
    private void Awake() {
        baseZ = transform.position.z;
    }

    void LateUpdate() {
        transform.position = new Vector3(transform.position.x, transform.position.y, baseZ);
    }
}
