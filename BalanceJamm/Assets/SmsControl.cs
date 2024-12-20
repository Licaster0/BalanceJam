using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmsControl : MonoBehaviour
{
    [SerializeField] private GameObject giftObject;
    [SerializeField] private GameObject giftSpawnPosition;
    [SerializeField] private GameObject TipPanel;
    [SerializeField] private bool giftActive = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && giftActive == false)
        {
            TipPanel.SetActive(true);
            giftObject.SetActive(true);
            giftActive = true;
        }
        if (collision.gameObject.CompareTag("Player") && giftActive == true)
        {
            TipPanel.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TipPanel.SetActive(false);
        }
    }
}
