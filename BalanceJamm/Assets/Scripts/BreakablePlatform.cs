using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float shakeDuration = 0.5f; // Sallanma süresi
    [SerializeField] private float shakeIntensity = 0.1f; // Sallanma yoğunluğu
    [SerializeField] private float breakDelay = 0.5f; // Kırılmadan önceki bekleme süresi
    [SerializeField] private float respawnTime = 3f; // Platformun yeniden ortaya çıkma süresi

    [Header("References")]
    [SerializeField] private Collider2D platformCollider; // Platformun çarpışma özelliği
    [SerializeField] private Collider2D otherCollider;
    [SerializeField] private SpriteRenderer platformRenderer; // Görsel kısmı

    [SerializeField] private ParticleSystem breakEffect; // Kırılma efekti


    private Vector3 initialPosition; // Platformun başlangıç pozisyonu
    private bool isBreaking = false; // Platformun şu anda kırılıp kırılmadığını kontrol eder

    private void Start()
    {
        initialPosition = transform.localPosition;
        if (platformCollider == null) platformCollider = GetComponent<Collider2D>();
        if (platformRenderer == null) platformRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBreaking && collision.gameObject.CompareTag("Player")) // Sadece oyuncu ile temas halinde çalışır
        {
            StartCoroutine(HandlePlatformBreak());
        }
    }

    private IEnumerator HandlePlatformBreak()
    {
        isBreaking = true;

        // Sallanma efekti
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            transform.localPosition = initialPosition + (Vector3)Random.insideUnitCircle * shakeIntensity;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (breakEffect != null)
            breakEffect.Play(); // Kırılma efektini oynat
        transform.localPosition = initialPosition; // Platformu eski yerine döndür

        // Kırılma
        yield return new WaitForSeconds(breakDelay);
        platformCollider.enabled = false;
        if (otherCollider != null)
            otherCollider.enabled = false;
        platformRenderer.enabled = false;

        // Yeniden oluşma
        yield return new WaitForSeconds(respawnTime);
        platformCollider.enabled = true;
        if (otherCollider != null)
            otherCollider.enabled = true;
        platformRenderer.enabled = true;

        transform.localPosition = initialPosition; // Pozisyonu sıfırla
        isBreaking = false;
    }
}
