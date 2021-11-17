using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class GravityPortal : MonoBehaviour
    {
        public bool inverse = false;

        private bool debounce;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb == null || debounce) return;

            debounce = true;

            rb.gravityScale = inverse ?
                rb.gravityScale > 0 ? -rb.gravityScale : rb.gravityScale :
                rb.gravityScale < 0 ? -rb.gravityScale : rb.gravityScale;
        }
    }
}