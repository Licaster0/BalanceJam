using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject UpgradePartical;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement.Instance.extraJumps += 1;
            Instantiate(UpgradePartical, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
