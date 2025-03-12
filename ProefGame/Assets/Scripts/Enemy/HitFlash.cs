using UnityEngine;
using System.Collections;

public class HitFlash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Flash()
    {
        StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }
}