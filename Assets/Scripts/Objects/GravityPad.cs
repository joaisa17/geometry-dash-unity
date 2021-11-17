using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class GravityPad : MonoBehaviour
    {
        private bool debounce;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb == null || debounce) return;

            debounce = true;

            rb.gravityScale *= -1;
            rb.velocity = new Vector2(
                rb.velocity.x,
                rb.velocity.y
            );
        }
    }
}