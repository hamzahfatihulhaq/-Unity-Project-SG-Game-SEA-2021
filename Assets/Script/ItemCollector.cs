using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private int itemPoint;
    private int myPoint;
    [SerializeField] private Text textPoint;

    [SerializeField] private AudioSource collectionSoundEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Point"))
        {
            collectionSoundEffect.Play();
            Destroy(collision.gameObject);
            myPoint = myPoint + itemPoint;
            //Debug.Log("Point : " + myPoint);
            textPoint.text = "Point : " + myPoint;

        }
    }
}
