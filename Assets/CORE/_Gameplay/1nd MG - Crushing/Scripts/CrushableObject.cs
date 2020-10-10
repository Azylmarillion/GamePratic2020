using UnityEngine;

namespace GamePratic2020 {
	public class CrushableObject : MonoBehaviour {
        #region References
        [SerializeField] private Sprite crushedSprite = null;
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        #endregion

        #region Currents
        private bool crushed = false;
        #endregion

        #region Behaviour
        public void Collect() {
            Destroy(gameObject);
        }
        #endregion

        #region Collisions
        public void OnTriggerEnter2D(Collider2D _collider) {
            if (!crushed) {
                spriteRenderer.sprite = crushedSprite;
                crushed = true;
            }
		}
        #endregion
	}
}