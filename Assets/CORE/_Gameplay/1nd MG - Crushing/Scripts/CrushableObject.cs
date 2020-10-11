using UnityEngine;

namespace GamePratic2020 {
	public class CrushableObject : MonoBehaviour {
        #region References
        [SerializeField] private Sprite crushedSprite = null;
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private ParticleSystem crushEffect = null;
        public CrushMiniGame miniGameOwner = null;
        #endregion

        #region Currents

        public bool Crushed { get; private set; } = false;
        #endregion

        #region Collisions
        public void OnTriggerEnter2D(Collider2D _collider) {
            if (!Crushed) {
                spriteRenderer.sprite = crushedSprite;
                Crushed = true;
                crushEffect.Play();
                miniGameOwner.CollectCoke();
            }
		}
        #endregion
	}
}