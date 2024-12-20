using System.Collections;
using UnityEngine;

public class SmsControl : MonoBehaviour
{
    [SerializeField] private GameObject giftObject;
    [SerializeField] private GameObject giftSpawnPosition;
    [SerializeField] private CanvasGroup tipPanelCanvasGroup; // CanvasGroup eklendi
    [SerializeField] private bool giftActive = false;

    [SerializeField] private float fadeDuration = 0.5f; // Fade s�resi
    private Coroutine fadeCoroutine; // Fade coroutine'ini takip etmek i�in

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && giftActive == false)
        {
            TipPanelFadeIn();
            giftObject.SetActive(true);
            giftActive = true;
        }
        else if (collision.gameObject.CompareTag("Player") && giftActive == true)
        {
            TipPanelFadeIn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TipPanelFadeOut();
        }
    }

    private void TipPanelFadeIn()
    {
        // Panel fade in yap�lacak
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCanvasGroup(tipPanelCanvasGroup, tipPanelCanvasGroup.alpha, 1, fadeDuration));
    }

    private void TipPanelFadeOut()
    {
        // Panel fade out yap�lacak
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCanvasGroup(tipPanelCanvasGroup, tipPanelCanvasGroup.alpha, 0, fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        // Ba�lang�� alpha de�erini ayarla
        canvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        // Fade i�lemi tamamland�ktan sonra kesin alpha de�erini ayarla
        canvasGroup.alpha = endAlpha;

        // G�r�nmezse etkile�imi kapat
        canvasGroup.interactable = endAlpha > 0;
        canvasGroup.blocksRaycasts = endAlpha > 0;
    }
}
