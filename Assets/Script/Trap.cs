using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int contactDamage = 1;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("aku kena");

            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            player.HitSide(transform.position.x > player.transform.position.x || transform.position.y > player.transform.position.y);
        }
    }
}
