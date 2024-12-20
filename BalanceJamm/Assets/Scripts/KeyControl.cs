using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyControl : MonoBehaviour
{
    [SerializeField] private GameObject kapı;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (kapı != null)
                kapı.SetActive(false);

            transform.rotation = Quaternion.Euler(0, 0, -32);
        }
    }
}
