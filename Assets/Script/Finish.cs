using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Finish : MonoBehaviour
{
    private AudioSource finishSound;

    private bool levelComplated = false;
    // Start is called before the first frame update
    void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !levelComplated)
        {
            finishSound.Play();
            levelComplated = true;
            Invoke("ComplateLevel", 2f);
            //ComplateLevel();
        }
    }

    private void ComplateLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
