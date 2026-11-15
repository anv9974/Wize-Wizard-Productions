using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public Color flashColor = new Color(1f, 0.3f, 0.3f); // tweak for a stronger or paler red
    public float flashTime = 0.1f;

    private SpriteRenderer sr;
    private Color originalColor;
    private Coroutine routine;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void Flash()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FlashRoutine());
    }

    public void CustomFlash(Color customColor, float customTime)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(CustomFlashRoutine(customColor, customTime));
    }

    IEnumerator FlashRoutine()
    {
        sr.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        sr.color = originalColor;
        routine = null;
    }

    IEnumerator CustomFlashRoutine(Color customColor, float customTime)
    {
        sr.color = customColor;
        yield return new WaitForSeconds(customTime);
        sr.color = originalColor;
        routine = null;
    }
}