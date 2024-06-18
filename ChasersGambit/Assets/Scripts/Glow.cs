using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    public Material glowMaterial;
    public float minGlow = .3f;
    public float maxGlow = .6f;
    public float glowSpeed = .1f;

    private float currentGlow;
    private bool increasing = true;

    void Start()
    {
        if (glowMaterial == null)
        {
            glowMaterial = GetComponent<Renderer>().material;
        }
        currentGlow = minGlow;
    }

    void Update()
    {
        if (increasing)
        {
            currentGlow += glowSpeed * Time.deltaTime;
            if (currentGlow >= maxGlow)
            {
                currentGlow = maxGlow;
                increasing = false;
            }
        }
        else
        {
            currentGlow -= glowSpeed * Time.deltaTime;
            if (currentGlow <= minGlow)
            {
                currentGlow = minGlow;
                increasing = true;
            }
        }

        glowMaterial.SetColor("_EmissionColor", glowMaterial.color * currentGlow);
    }
}
