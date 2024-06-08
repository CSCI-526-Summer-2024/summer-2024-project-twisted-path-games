using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntedCam : MonoBehaviour
{
    // Mouse sensitivity, lowered for build
    public float sensX = 400.0f;
    public float sensY = 400.0f;

    // Reference to the orientation object under the hunted game object
    // This empty game object's purpose is to track the player's orientation
    public Transform orientation;

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
    }

    void Update()
    {
        // Get mouse inputs for first person viewing control
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            mouseX = DampenedMovement(mouseX);
            mouseY = DampenedMovement(mouseY);
        }

        // Rotate the camera left and right
        yRotation += mouseX;

        // Rotate the camera up and down and prevent camera flips
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply camera rotation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Update the rotation of the orientation object
        // Ensures the player's forward direction matches where the camera is looking horizontally
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }
}
