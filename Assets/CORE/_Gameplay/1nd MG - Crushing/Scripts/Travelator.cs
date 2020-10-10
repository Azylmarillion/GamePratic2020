// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GamePratic2020 {
    public class Travelator : MonoBehaviour {
        #region Fields / Properties
        [Section("Travelator", order = 1)]
        [SerializeField] private float speed = 1f;
        [SerializeField] private Vector2 range = new Vector2(-5f, 5f);
        [SerializeField, Min(0.001f)] private Vector2 randomRate = new Vector2(1f, 2f);
        public bool spawn = true;

        [Header("References")]
        [SerializeField] private CrushableObject crushablePrefab = null;
        [SerializeField] private CrushMiniGame ownerCrushMiniGame = null;
        #endregion

        #region Events
        [Section("Events")]
        [SerializeField] UnityEvent onSpawnElement = new UnityEvent();
        [SerializeField] UnityEvent onCollectFailedElement = new UnityEvent();
        [SerializeField] UnityEvent onCollectValidElement = new UnityEvent();
        #endregion

        #region Currents
        private List<CrushableObject> crushableObjects = new List<CrushableObject>(10);
        private float nextElementTimer = 0f;
        #endregion

        #region Methods
        private void Update() {
            MoveObjects();

            if (spawn) {
                nextElementTimer -= Time.deltaTime;
                if (nextElementTimer < 0f) {
                    Spawn();
                    float rate = Random.Range(randomRate.x, randomRate.y);
                    nextElementTimer += 1 / rate;
                }
            }

            CheckObjects();
        }
        #endregion

        #region Manage Objects
        private void MoveObjects() {
            for (int i = 0; i < crushableObjects.Count; i++) {
                crushableObjects[i].transform.position += Vector3.right * Time.deltaTime * speed;
            }
        }

        private void Spawn() {
            Vector3 spawnPos = transform.position + Vector3.right * range.x;
            CrushableObject instance = Instantiate(crushablePrefab, spawnPos, Quaternion.identity);
            crushableObjects.Add(instance);
            onSpawnElement?.Invoke();
        }

        private void CheckObjects() {
            for (int i = 0; i < crushableObjects.Count; i++) {
                CrushableObject obj = crushableObjects[i];
                if (obj.transform.position.x > range.y) {
                    crushableObjects.RemoveAt(i);
                    CollectElement(obj);
                }
            }
        }

        private void CollectElement(CrushableObject obj) {
            if (obj.Crushed) {
                ownerCrushMiniGame.CollectCoke();
                onCollectValidElement?.Invoke();
            } else {
                onCollectFailedElement?.Invoke();
            }

            obj.Collect();
        }
        #endregion


        #region Debug
#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Vector3 infiniteUp = Vector3.up * 220f;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(infiniteUp + Vector3.right * range.x, -infiniteUp + Vector3.right * range.x);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(infiniteUp + Vector3.right * range.y, -infiniteUp + Vector3.right * range.y);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.right * range.x, transform.position + Vector3.right * range.y);
        }
#endif
        #endregion
    }
}
