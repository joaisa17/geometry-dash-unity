using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class JumpPad : MonoBehaviour
    {
        public float boostHeight = 50f;

        private bool debounce;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb == null || debounce) return;

            debounce = true;

            rb.velocity = new Vector2(
                rb.velocity.x,
                rb.gravityScale < 0 ? -boostHeight : boostHeight
            );
        }
    }
}