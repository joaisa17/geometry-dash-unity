using Assets.Scripts.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game
{
    public class Player : MonoBehaviour
    {
        public Collider2D finishTrigger;
        public ParticleSystem deathEffect;

        public KeyCode[] jumpKeys = {KeyCode.Space};
        public bool enableJumpWithMouse = true;

        // Physics
        public float jumpingPower = 5f;
        public float movementSpeed = 5f;

        // Rotation
        public float rotationSpeed = 5f;

        // Events
        public UnityEvent winEvent;
        public UnityEvent dieEvent;

        private Rigidbody2D rigidBody2D;
        private Vector2 defaultPosition;

        private bool active;
        private bool canJump = true;
        private bool isTouchingObject = true;

        private bool dead;

        private int gravityMultiplier = 1;

        private void Start()
        {
            if (TryGetComponent<Rigidbody2D>(out var rigidbody))
                rigidBody2D = rigidbody;
        }

        // Runs when level has started
        public void Init()
        {
            active = true;

            rigidBody2D.WakeUp();
            rigidBody2D.velocity.Set(movementSpeed, rigidBody2D.velocity.y);
        }

        // Runs when level is resetting
        public void ResetState()
        {
            active = false;
            dead = false;

            rigidBody2D.position.Set(defaultPosition.x, defaultPosition.y);
            rigidBody2D.velocity.Set(0, 0);
            rigidBody2D.Sleep();
        }

        private void Jump()
        {
            if (!active || !canJump || dead) return;

            canJump = false;

            rigidBody2D.angularVelocity = 0;
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpingPower * gravityMultiplier);
        }

        private void OnCollisionEnter2D()
        {
            canJump = true;
            isTouchingObject = true;
        }

        private void OnCollisionExit2D()
        {
            canJump = false;
            isTouchingObject = false;
        }

        private static bool TriggerKillsPlayer(Collider2D collision)
        {
            var go = collision.gameObject;
            return !go.GetComponent<JumpPad>() && !go.GetComponent<GravityPad>() && !go.GetComponent<GravityPortal>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == finishTrigger) winEvent.Invoke();
            else if (TriggerKillsPlayer(collision)) Die();
        }

        // Runs once per frame
        private void Update()
        {
            if (!active || dead) return;

            gravityMultiplier = rigidBody2D.gravityScale < 0 ? -1 : 1;
            rigidBody2D.velocity = new Vector2(movementSpeed, rigidBody2D.velocity.y);
            if (!isTouchingObject)
            {
                rigidBody2D.SetRotation(rigidBody2D.rotation - rotationSpeed * Time.deltaTime * gravityMultiplier);
            }
            else
            {
                rigidBody2D.angularVelocity /= 1.01f;
            }
            // Loop through jump keybinds

            foreach (var keyCode in jumpKeys)
            {
                if (Input.GetKey(keyCode)) Jump();
            }

            // Jump with mouse
            if (enableJumpWithMouse && Input.GetMouseButton(0))
                Jump();
        }

        private void Die()
        {
            dead = true;

            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            deathEffect.Play();
            GetComponent<SpriteRenderer>().enabled = false;

            dieEvent.Invoke();
        }
    }
}