using UnityEngine;
using UnityEngine.Rendering;

namespace Cameras
{
    public class HuntedCam : MonoBehaviour
    {
        // Mouse sensitivity, lowered for build
        #if UNITY_WEBGL && !UNITY_EDITOR
            public float sensX = 200.0f;
            public float sensY = 200.0f;  
        #else
            public float sensX = 200.0f;
            public float sensY = 200.0f;
        #endif

        // Reference to the orientation object under the hunted game object
        // This empty game object's purpose is to track the player's orientation
        public Transform orientation;

        public GameObject flashlight;

        // Camera rotation
        float xRotation;
        float yRotation;
 
        public static float DampenedMovement (float value) {

            if (Mathf.Abs (value) > 1f) {
                // best value is 0.5 but better make it user-adjustable
                return Mathf.Lerp (value, Mathf.Sign (value), 0.5f);
            }
            return value;
        }

        void Start()
        {
            // Lock the cursor to the center of the game window for first person view
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            
            xRotation = orientation.eulerAngles.x;
            yRotation = orientation.eulerAngles.y;
        }

        void Update()
        {
            if (SplashScreen.isFinished)
            {
                // Get mouse inputs for first person viewing control
                float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
                float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    mouseX = DampenedMovement(mouseX);
                    mouseY = DampenedMovement(mouseY);
                }
                
                // Rotate the camera left and right
                yRotation += mouseX;
                
                // Rotate the camera up and down and prevent camera flips
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -50f, 50f);
                
                // Apply camera rotation
                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                
                // Update the rotation of the orientation object
                // Ensures the player's forward direction matches where the camera is looking horizontally
                orientation.rotation = Quaternion.Euler(0, yRotation, 0);
                
                // Apply rotation to the flashlight
                flashlight.transform.rotation = Quaternion.Euler(xRotation + 90, yRotation, 0);
            }
        }
    }
}
