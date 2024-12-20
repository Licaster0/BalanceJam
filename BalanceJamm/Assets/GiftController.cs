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
    [SerializeField] private GameObject giftSpawnParticile;
    [SerializeField] private GameObject giftExplodeParticile;
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
                Instantiate(giftExplodeParticile.transform, gameObject.transform.position, Quaternion.identity);
                gameObject.transform.position = giftSpawnPosition.position;
                Instantiate(giftSpawnParticile.transform, giftSpawnPosition.position, Quaternion.identity);
                return;
            }
            if (PlayerManager.Instance.CurrentHealth <= 0)
            {
                Instantiate(giftExplodeParticile.transform, gameObject.transform.position, Quaternion.identity);
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
