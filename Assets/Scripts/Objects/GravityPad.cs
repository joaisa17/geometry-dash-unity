using UnityEngine;
using Assets.Scripts.Game;

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

            float newVelocity = -rb.velocity.y;

            Player player = rb.gameObject.GetComponent<Player>();
            if (player != null) newVelocity =  (rb.gravityScale < 0 ? player.jumpingPower : -player.jumpingPower) / 2;

            rb.velocity = new Vector2(
                rb.velocity.x,
                newVelocity
            );
        }
    }
}