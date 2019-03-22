using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{

    float posX;
    bool mov = true;
    public float velocidad;
    public float killer_jump = 5;
    public float scary_jump = 6;
    public PlayerController mario;
    public CameraFollow mario_camera;
    public Sprite dead_mario_sprite;
    public Animator animator;
    public SpriteRenderer sprite_renderer;
    public Rigidbody2D rigid_body;

    void Sart()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        rigid_body.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (mov)
        {
            posX = velocidad;
            transform.GetComponent<SpriteRenderer>().flipX = true;
            animator.SetBool("Walking", true);
        }
        else
        {
            posX = velocidad * -1;
            transform.GetComponent<SpriteRenderer>().flipX = false;
            animator.SetBool("Walking", true);
        }
        transform.Translate(posX, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        string object_tag = col.gameObject.tag;
        if (col.contacts[0].normal == new Vector2(1, 0) || col.contacts[0].normal == new Vector2(-1, 0))
        {
            if (mov)
            {
                mov = false;
            }
            else
            {
                mov = true;
            }
        }
        if (object_tag == "Player")
        {
            if (col.contacts[0].normal == new Vector2(1, 0) || col.contacts[0].normal == new Vector2(-1, 0))
            {
                if (mario.grande)
                {
                    col.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * scary_jump,
                    ForceMode2D.Impulse);
                    mario.transform.localScale = new Vector3(2, 2, 2);
                    mario.grande = false;
                }
                else
                {
                    mario.anim.SetBool("Walk", false);
                    mario.anim.SetBool("Dead", true);
                    col.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * killer_jump,
                    ForceMode2D.Impulse);
                    col.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    mario_camera.smoothing = 0;
                    mario.enabled = false;
                }
            }
            if (col.contacts[0].normal == new Vector2(0, -1))
            {
                animator.SetBool("Walking", false);
                transform.localScale = new Vector3(2, 0.5f, 2);
                velocidad = 0;
                transform.Translate(0, -0.5f, 0);
                StartCoroutine(WaitUntilKillingFinishes());
            }
        }
    }

    public IEnumerator WaitUntilKillingFinishes()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
