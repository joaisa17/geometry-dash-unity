using UnityEngine;

namespace Assets.Scripts.Game
{
    public class CameraHandler : MonoBehaviour
    {
        public GameObject player;
        public GameObject ground;

        public Vector2 positionOffset;

        public float moveTime = 0.5f;

        private Rigidbody2D rb;
        private float currentSpeed;

        private float originX;

        private bool active;

        public void ResetState()
        {
            active = false;
        }

        public void Init()
        {
            active = true;
        }

        // Start is called before the first frame update
        private void Start()
        {
            rb = player.GetComponent<Rigidbody2D>();
            originX = transform.position.x;

            var cam = Camera.main;

            var camHeight = cam.orthographicSize * 2;
            var camWidth = camHeight * Screen.width / Screen.height;

            var currentScale = ground.transform.localScale;

            ground.transform.localScale = new Vector3(
                camWidth + 5,
                currentScale.y,
                currentScale.z
            );

            var groundRenderer = ground.GetComponent<SpriteRenderer>();

            Vector2 screenBounds = cam.ScreenToWorldPoint(new Vector3(
                0,
                0,
                cam.transform.position.z
            ));

            ground.transform.position = new Vector3(
                ground.transform.position.x,
                screenBounds.y + groundRenderer.bounds.extents.y,
                ground.transform.position.z
            );
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (!active) return;

            var targetPos = Mathf.Clamp(rb.position.x + positionOffset.x, originX, Mathf.Infinity);

            var newPos = Mathf.SmoothDamp(
                transform.position.x,
                targetPos,
                ref currentSpeed,
                moveTime
            );

            transform.position = new Vector3(
                newPos,
                transform.position.y + positionOffset.y,
                transform.position.z
            );

            ground.transform.position = new Vector3(
                transform.position.x,
                ground.transform.position.y,
                ground.transform.position.z
            );
        }
    }
}