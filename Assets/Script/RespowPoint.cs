using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespowPoint : MonoBehaviour
{
    private Vector3 respownPoint;
    // Start is called before the first frame update
    void Start()
    {
        respownPoint = transform.position;        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RespownPoint"))
        {
            respownPoint = transform.position;
        }
    }
    public void respown()
    {
        transform.position = respownPoint;
    }
}
