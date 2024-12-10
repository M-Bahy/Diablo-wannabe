using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class Hoverable_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    private Image buttonImage;
    private CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FadeToSprite(hoverSprite));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FadeToSprite(defaultSprite));
    }

    private IEnumerator FadeToSprite(Sprite newSprite)
    {
        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = 1 - t / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 0;
        buttonImage.sprite = newSprite;

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }
}