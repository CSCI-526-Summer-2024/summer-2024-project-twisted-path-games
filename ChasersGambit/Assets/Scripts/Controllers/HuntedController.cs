using UnityEngine;
using System.Collections;
namespace Controllers

{
    public class HuntedController : MonoBehaviour
    {
        private float moveSpeed = 3;

        float horizontalInput;
        float verticalInput;
        Vector3 moveDirection;
        private float fadeDuration = 0.3f;

        // Ground drag properties so the player doesn't skate across ice-like ground
        private float groundDrag = 5;
        private float playerHeight = 2;
        public LayerMask whatIsGround;
        bool grounded;

        // Hunted orientation object
        public Transform orientation;

        // Hunted rigid body component
        Rigidbody rb;

        public FlashlightToggle flashlightToggle;

        
        public Camera camera;
        public float shake = 0;
        float shakeAmount = 0.7f;
        float decreaseFactor = 1.0f;

        void Start()
        {
            // Prevent rotation based on physics interactions
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            flashlightToggle = GetComponent<FlashlightToggle>();
            if (flashlightToggle == null)
            {
                Debug.Log("flashlight toggle not found");
            }
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

            if (shake > 0) {
                camera.transform.localPosition = Random.insideUnitSphere * shakeAmount;
                shake -= Time.deltaTime * decreaseFactor;

            } else {
                shake = 0.0f;
                camera.transform.localPosition = Vector3.zero;
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
        
        public void ToggleFlashlightOff()
        {
            StartCoroutine(FadeOutLight());
        }
        
        public void ToggleFlashlightOn()
        {
            StartCoroutine(FadeInLight());
        }

        IEnumerator FadeOutLight()
        {
            float startIntensityOuter = flashlightToggle.outer.intensity;
            float startIntensityMid = flashlightToggle.mid.intensity;
            float startIntensityInner = flashlightToggle.inner.intensity;

            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                flashlightToggle.outer.intensity = Mathf.Lerp(startIntensityOuter, 0, elapsedTime / fadeDuration);
                flashlightToggle.mid.intensity = Mathf.Lerp(startIntensityMid, 0, elapsedTime / fadeDuration);
                flashlightToggle.inner.intensity = Mathf.Lerp(startIntensityInner, 0, elapsedTime / fadeDuration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            flashlightToggle.isOn = false;
        }
        
        IEnumerator FadeInLight()
        {
            flashlightToggle.enabled = true;
            float endIntensityOuter = 1.5f;
            float endIntensityMid = 1.2f;
            float endIntensityInner = 1.5f;

            float elapsedTime = 0f;
            
            while (elapsedTime < fadeDuration)
            {
                flashlightToggle.outer.intensity = Mathf.Lerp(0, endIntensityOuter, elapsedTime / fadeDuration);
                flashlightToggle.mid.intensity = Mathf.Lerp(0, endIntensityMid, elapsedTime / fadeDuration);
                flashlightToggle.inner.intensity = Mathf.Lerp(0, endIntensityInner, elapsedTime / fadeDuration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
    }

    }
