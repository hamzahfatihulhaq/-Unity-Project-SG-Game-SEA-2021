using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeCount : MonoBehaviour
{
    [SerializeField] private Image[] life;
    public int lifeRemaining;
    [SerializeField] private float timeDelay;

    private Animator anim;
    private Rigidbody2D rb;

    private int damage = 1;

    [SerializeField] AudioSource deathSoundEffect;
    [SerializeField] AudioSource respownSoundEffect;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void HealthPlayer()
    {
        for (int i = 0; i < damage; i++)
        {
            lifeRemaining--;
            if (lifeRemaining < 0)
            {
                lifeRemaining = 0;
            }
            life[lifeRemaining].enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FallDetector"))
        {
            HealthPlayer();
            StartCoroutine(DeadAnim());
        }
    }
    public IEnumerator DeadAnim()
    {
        deathSoundEffect.Play();
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("IsDeath");
        yield return new WaitForSeconds(timeDelay);

        if(lifeRemaining <= 0)
        {
            anim.SetTrigger("IsRespown");
            RestartLevel();
        }

        respownSoundEffect.Play();
        FindObjectOfType<RespowPoint>().respown();
        anim.SetTrigger("IsRespown");

        yield return new WaitForSeconds(timeDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
