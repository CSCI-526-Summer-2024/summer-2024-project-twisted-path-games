using UnityEngine;

public class PulsingLight : MonoBehaviour
{
    public Light pointLight;  // Reference to the Point Light component
    public float minIntensity = 0.2f;
    public float maxIntensity = 0.4f;
    public float minRange = 0.5f;
    public float maxRange = 1.0f;
    public float pulseSpeed = 0.4f;

    private float currentIntensity;
    private float currentRange;
    private bool increasing = true;

    void Start()
    {
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
        }
        currentIntensity = minIntensity;
        currentRange = minRange;
    }

    void Update()
    {
        if (increasing)
        {
            currentIntensity += pulseSpeed * Time.deltaTime;
            currentRange += pulseSpeed * Time.deltaTime;

            if (currentIntensity >= maxIntensity || currentRange >= maxRange)
            {
                //currentIntensity = maxIntensity;
                //currentRange = maxRange;
                increasing = false;
            }
        }
        else
        {
            currentIntensity -= pulseSpeed * Time.deltaTime;
            currentRange -= pulseSpeed * Time.deltaTime;

            if (currentIntensity <= minIntensity || currentRange <= minRange)
            {
                //currentIntensity = minIntensity;
                //currentRange = minRange;
                increasing = true;
            }
        }

        pointLight.intensity = currentIntensity;
        pointLight.range = currentRange;
    }
}
