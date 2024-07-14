using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public float minOnTime = 0.1f;    // Minimum time the light stays on
    public float maxOnTime = 0.5f;    // Maximum time the light stays on
    public float minOffTime = 1f;     // Minimum time the light stays off
    public float maxOffTime = 3f;     // Maximum time the light stays off

    private Light flickeringLight;
    private bool isLightOn = false;
    private float nextToggleTime;

    void Start()
    {
        flickeringLight = GetComponent<Light>();
        ToggleLight();
    }

    void Update()
    {
        if (Time.time > nextToggleTime)
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        isLightOn = !isLightOn;
        flickeringLight.enabled = isLightOn;

        if (isLightOn)
        {
            // Light is on, set next toggle time for turning off
            nextToggleTime = Time.time + Random.Range(minOnTime, maxOnTime);
        }
        else
        {
            // Light is off, set next toggle time for turning on
            nextToggleTime = Time.time + Random.Range(minOffTime, maxOffTime);
        }
    }
}
