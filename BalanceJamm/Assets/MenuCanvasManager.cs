using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvasManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup creditPanel;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private bool isPanelVisible = false;
    // CanvasGroup bileşeni referansı
    private CanvasGroup canvasGroup;

    // Fade süresi
    [SerializeField] private float fadeDuration0 = 1.5f;

    private void Start()
    {
        // CanvasGroup bileşenini al
        canvasGroup = GetComponent<CanvasGroup>();

        // Ba�lang��ta tamamen g�r�nmez yap
        canvasGroup.alpha = 0f;

        // Fade In ba�lat
        StartCoroutine(FadeInScene());
    }

    private IEnumerator FadeInScene()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration0)
        {
            // Alpha de�erini zamanla 0'dan 1'e ��kar
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration0);

            // Ge�en s�reyi art�r
            elapsedTime += Time.deltaTime;

            // Bir frame bekle
            yield return null;
        }

        // Fade tamamland���nda alpha de�erini tam g�r�n�r olarak ayarla
        canvasGroup.alpha = 1f;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        creditPanel.gameObject.SetActive(false);
    }

    public void ToggleCreditPanel()
    {
        if (isPanelVisible)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            StartCoroutine(FadeIn());
        }

        isPanelVisible = !isPanelVisible;
    }
    private IEnumerator FadeIn()
    {
        creditPanel.gameObject.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            creditPanel.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        creditPanel.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            creditPanel.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        creditPanel.alpha = 0f;
        creditPanel.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
