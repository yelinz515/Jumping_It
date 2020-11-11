using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioSource death;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        death = GetComponent<AudioSource>();
    }

    public void JumpedOn()
    {
        anim.SetTrigger("Death");
        death.Play();
        rb.velocity = Vector2.zero;
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
}
