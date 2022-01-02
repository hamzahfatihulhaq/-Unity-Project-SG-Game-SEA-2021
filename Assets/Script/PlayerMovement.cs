using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask jumpAble;

    Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    public GameObject fallDetector;
    private enum MovementState { idle, running, jumping, falling}

    // hit
    private bool hitSideRight = false;
    private bool isInvisible = false;
    private bool isTakingDamage = false;

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource takeHitSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector3(rb.velocity.y, jumpForce, 0);
        }
        AnimationUpdate(dirX);

        //falldetector
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void AnimationUpdate(float dirX)
    {
        MovementState state;

        if (dirX < 0)
        {
            sprite.flipX = true;
            //anim.SetBool("Running", true);
            state = MovementState.running;
        }
        else if(dirX > 0)
        {
            sprite.flipX = false;
            //anim.SetBool("Running", true);
            state = MovementState.running;
        }
        else
        {
            //anim.SetBool("Running", false);
            state = MovementState.idle;
        }
        if(rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("State", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, .0f, Vector2.down, .1f, jumpAble);
    }

    private void Invisble(bool invisibility)
    {
        isInvisible = invisibility;
    }

    public void HitSide(bool rightSide)
    {
        hitSideRight = rightSide;

        // int liveRemain
        if (!isInvisible)
        {
            FindObjectOfType<LifeCount>().HealthPlayer();
            int liveRemaining = FindObjectOfType<LifeCount>().lifeRemaining;

            if (liveRemaining <= 0)
            {
                StartCoroutine(FindObjectOfType<LifeCount>().DeadAnim());
            }
            else
            {
                StartDamegeAnim();
            }
        }
    }

    void StartDamegeAnim()
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            Invisble(true);

            float hitForceX = 8f;
            float hitForceY = 8f;

            if (hitSideRight)
            {
                hitForceX = -hitForceX;
            }

            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);

            StartCoroutine(Damege());
        }

        StopDamageAnimation();
    }

    void StopDamageAnimation()
    {
        // player kebal
        isTakingDamage = false;
        StartCoroutine(FlashAfterDamage());
    }

    private IEnumerator FlashAfterDamage()
    {
        float flashDelay = .1f;

        for (int i = 0; i < 20; i++)
        {
            sprite.color = Color.clear;
            yield return new WaitForSeconds(flashDelay);
            sprite.color = Color.white;
            yield return new WaitForSeconds(flashDelay);
        }

        Invisble(false);
    }


    IEnumerator Damege()
    {
        takeHitSoundEffect.Play();
        anim.SetTrigger("TakeHit");
        yield return new WaitForSeconds(2f);
    }
}
