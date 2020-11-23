using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private bool ishurt;
    public bool isice;
    private bool isjump;


    [SerializeField] private Collider2D coll;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private float speed = 5.5f;
    [SerializeField] private float jumpForce = 10f;
    public int cherries = 0;
    // [SerializeField] private TextMeshProUGUI cherryText;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource footstep;
    public int health;

    SpriteRenderer spr;
    Color newcolor;
    Color oldcolor;


    private enum State { idle, running, jumping, falling, hurt, climb }
    private State state = State.idle;

    [HideInInspector] public bool canClimb = false;
    [HideInInspector] public bool bottomLadder = false;
    [HideInInspector] public bool topLadder = false;
    public ladder ladder;
    private float naturalGravity;
    [SerializeField] float climbSpeed = 3f;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // coll = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
        oldcolor = spr.color;
        newcolor = spr.color;
        newcolor.a = 0.75f;
        naturalGravity = rb.gravityScale;
        ishurt = false;
        isice = false;
        isjump = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.climb && canClimb)
        {
            TouchClimb();
            Climb();
        }
        else if (state != State.hurt)
        {
            TouchMovement();
            Movement();
        }
        VelocityState();
        anim.SetInteger("State", (int)state);

        if (isice)
        {
            speed = 10f;
        }
        else
        {
            speed = 5.5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherry.Play();
            cherries += 1;
            // cherryText.text = cherries.ToString();

        }

        else if (collision.tag == "Gem")
        {
            cherry.Play();

        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "iceground")
        {
            isice = true;
            isjump = true;
        }
        if (collision.gameObject.tag == "normalground")
        {
            isice = false;
            isjump = true;
        }


        if (collision.gameObject.tag == "Enemy" && !ishurt)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                isjump = true;
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                ishurt = true;
                HandleHealth();
                spr.color = newcolor;
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
                StartCoroutine(WaitForIt());
            }

        }


        if (collision.gameObject.tag == "thorn" && !ishurt)
        {
            state = State.hurt;
            ishurt = true;
            HandleHealth();
            spr.color = newcolor;
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(hurtForce, rb.velocity.y);
            }
            StartCoroutine(WaitForIt());
        }

    }

    private void HandleHealth()
    {
        health -= 1;
        if (health <= 0)
        {
            GameObject.Find("FallCheck").GetComponent<Fall>().GameOver();
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            state = State.climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladder.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;

        }

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-0.9f, 0.9f);

        }

        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(0.9f, 0.9f);

        }

        if (Input.GetButtonDown("Jump") && isjump)
        {
            Jump();
        }
    }

    private void TouchMovement()
    {
        float hDirection = SimpleInput.GetAxis("Horizontal");

        if (canClimb && Mathf.Abs(SimpleInput.GetAxis("Vertical")) > .1f)
        {
            state = State.climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladder.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;

        }

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-0.9f, 0.9f);

        }

        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(0.9f, 0.9f);

        } 
    }
    private void Jump()
    {
        if (isjump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;
            isjump = false;
        }
    }
    public void TouchJump()
    {
        if (state == State.climb)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rb.gravityScale = naturalGravity;
            // anim.speed = 1f;
            Jump();
        }
        else if (isjump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;
            isjump = false;
        }
    }

    private void VelocityState()
    {
        if (state == State.climb)
        {

        }
        else if (state == State.running)
        {
            if (rb.velocity.y < -4f)
            {
                state = State.falling;
            }
            else if(Mathf.Abs(rb.velocity.x) < 1f)
            {
                state = State.idle;
            }
        }
        else if (state == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.falling;
                if (coll.IsTouchingLayers(ladderLayer))
                {
                    canClimb = true;
                }
            }
        }
        else if (state == State.falling)
        {
            isjump = false;
            if (coll.IsTouchingLayers(ladderLayer))
            {
                canClimb = true;
            }
            if (coll.IsTouchingLayers(ground))
            {
                isjump = true;
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f && !ishurt)
            {
                spr.color = oldcolor;
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    private void Climb()
    {
        isjump = true;
        if (Input.GetButtonDown("Jump"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rb.gravityScale = naturalGravity;
            // anim.speed = 1f;
            Jump();
        }

        float vDirection = Input.GetAxis("Vertical");

        if (vDirection > .1f && !topLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            // anim.speed = 1f;
        }
        else if (vDirection < -.1f && !bottomLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            // anim.speed = 1f;
        }
        else
        {
            // anim.speed = 0f;
            // rb.velocity = Vector2.zero;
        }
    }
    private void TouchClimb()
    {
        isjump = true;

        float vDirection = SimpleInput.GetAxis("Vertical");

        if (vDirection > .1f && !topLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            // anim.speed = 1f;
        }
        else if (vDirection < -.1f && !bottomLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            // anim.speed = 1f;
        }
        else
        {
            // anim.speed = 0f;
            // rb.velocity = Vector2.zero;
        }
    }

    private void Footstep()
    {
        footstep.Play();
    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(1.5f); //2초 기다린다.
        // 수행할 액션들 
        ishurt = false;
    }
}