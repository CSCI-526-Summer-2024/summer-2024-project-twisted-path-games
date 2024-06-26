using UnityEngine;
namespace Controllers
{
    public class HuntedController : MonoBehaviour
    {
        public float moveSpeed;

        float horizontalInput;
        float verticalInput;
        Vector3 moveDirection;

        public Light flashlight;

        // Ground drag properties so the player doesn't skate across ice-like ground
        public float groundDrag;
        public float playerHeight;
        public LayerMask whatIsGround;
        bool grounded;

        // Hunted orientation object
        public Transform orientation;

        // Hunted rigid body component
        Rigidbody rb;

        void Start()
        {
            // Prevent rotation based on physics interactions
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            flashlight = GetComponentInChildren<Light>();
        }

        void Update()
        {
            // Check if the player is on the ground
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight);

            GetInputs();
            SpeedControl();

            // Apply drag only if the player is on the ground
            if (grounded)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = 0;
            }

            if (flashlight != null && Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("F key pressed");
                flashlight.enabled = !flashlight.enabled;
            }
        }

        // Use this for physics calculations since the MoveHunted method is applying
        // movement forces
        private void FixedUpdate()
        {
            MoveHunted();
        }

        // Get keyboard inputs
        private void GetInputs()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        // Calculate movement direction based on orientation and inputs
        // Apply force to the rigid body to move the player
        private void MoveHunted()
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // Ensure the player's velocity does not exceed moveSpeed
        private void SpeedControl()
        {
            // Get the velocity of the player's x and z axis
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                // Create a limited velocity based on the moveSpeed
                Vector3 limitedVel = flatVel.normalized * moveSpeed;

                // Apply the limit to the player
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }
}