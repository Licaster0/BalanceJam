using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUnlocked : MonoBehaviour
{
    [SerializeField] private GameObject UpgradePartical;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement.Instance.canDash = true;
            Instantiate(UpgradePartical, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
