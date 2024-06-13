using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    private Light flashlight;

    private void Start()
    {
        flashlight = GetComponent<Light>();

        if (flashlight == null)
        {
            Debug.LogError("No Light component found on this GameObject.");
        }
    }

    void Update()
    {
        if (flashlight != null && Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
