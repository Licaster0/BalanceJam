using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GiftController : MonoBehaviour
{
    [SerializeField] private Sprite giftCrackSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LayerMask groundLayer; // Kontrol etmek istedi�iniz Layer'� belirlemek i�in
    [SerializeField] private LayerMask playerLayer; // Kontrol etmek istedi�iniz Layer'� belirlemek i�in
    [SerializeField] private Transform giftSpawnPosition;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject giftSpawnParticile;
    [SerializeField] private GameObject giftExplodeParticile;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject gameOverScene;
    private void Start()
    {

    }
    private void FixedUpdate()
    {
        if (!IsGrounded() && rb.gravityScale != 2)
        {
            rb.gravityScale = Mathf.Lerp(rb.gravityScale, 3, Time.fixedDeltaTime * 2); // Yer�ekimini yava��a art�r
        }
        else
        {
            rb.gravityScale = 1; // Yerdeyken yer�ekimini normale d�nd�r
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, playerLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            PlayerManager.Instance.CurrentHealth--;
            if (PlayerManager.Instance.CurrentHealth > 0)
            {
                Instantiate(giftExplodeParticile.transform, gameObject.transform.position, Quaternion.identity);
                gameObject.transform.position = giftSpawnPosition.position;
                Instantiate(giftSpawnParticile.transform, giftSpawnPosition.position, Quaternion.identity);
                return;
            }
            if (PlayerManager.Instance.CurrentHealth <= 0)
            {
                Instantiate(giftExplodeParticile.transform, gameObject.transform.position, Quaternion.identity);
                spriteRenderer.sprite = giftCrackSprite;
                gameOverScene.SetActive(true);
                PlayerManager.Instance.giftCrashed = true;
            }
        }
    }
}
