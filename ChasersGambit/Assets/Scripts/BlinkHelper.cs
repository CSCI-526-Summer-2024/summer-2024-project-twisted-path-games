using UnityEngine;
using UnityEngine.UI;

public class BlinkHelper : MonoBehaviour
{
    public Image glowSprite;
    public float glowSpeed = 1f;
    private Color originalColor;

    void Start()
    {
        if (glowSprite != null)
        {
            originalColor = glowSprite.color;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            glowSprite.gameObject.SetActive(false);
        }
        if (glowSprite != null)
        {
            float alpha = (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f; // Sin wave for pulsing effect
            Color newColor = originalColor;
            newColor.a = alpha;
            glowSprite.color = newColor;
        }
    }
}