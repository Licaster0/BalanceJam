using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishHouse : MonoBehaviour
{
    [SerializeField] private GameObject finishMessage;
    [SerializeField] private GameObject destroyGift;
    [SerializeField] private GameObject finishMenu;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            finishMessage.SetActive(true);
            destroyGift.SetActive(false);
            StartCoroutine(FinishGame());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            finishMessage.SetActive(false);
        }
    }

    IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(1f);
        finishMenu.SetActive(true);
    }
}
