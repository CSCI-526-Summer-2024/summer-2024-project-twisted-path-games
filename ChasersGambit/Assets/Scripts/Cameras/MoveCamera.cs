using UnityEngine;

namespace Cameras
{
    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraPosition;

        void Update()
        {
            // Match the camera position with the hunted position
            // This exists because adding a camera to a rigid body is apparently quite buggy
            transform.position = cameraPosition.position;
        }
    }
}