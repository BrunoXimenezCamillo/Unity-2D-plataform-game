using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUp_Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>()._isStageUp = true;
        }
    }
}
