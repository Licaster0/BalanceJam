using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GiftController : MonoBehaviour
{
    [SerializeField] private Sprite giftCrackSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LayerMask groundLayer; // Kontrol etmek istediðiniz Layer'ý belirlemek için
    [SerializeField] private Transform giftSpawnPosition;
    private void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            PlayerManager.Instance.CurrentHealth--;
            if (PlayerManager.Instance.CurrentHealth > 0)
            {
                gameObject.transform.position = giftSpawnPosition.position;
                return;
            }
            if (PlayerManager.Instance.CurrentHealth <= 0)
            {
                spriteRenderer.sprite = giftCrackSprite;
                Invoke("LoadSceneMode", 2f);
            }
        }
    }

    private void LoadSceneMode()
    {
        Debug.Log("calisti");
    }
}
