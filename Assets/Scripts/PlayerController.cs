using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float speed = 5;
    public float jump;
    public float posX = 0;
    bool floor = true;
    bool duct = false;
    public Rigidbody2D rigid_body;
    public SpriteRenderer sprite_renderer;
    public Animator anim;
    public GameObject hongo;
    public Coin coin;
    public Sprite inactive_box_sprite;
    public bool grande;


    // Use this for initialization
    void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        rigid_body.freezeRotation = true;
        anim = transform.GetComponent<Animator>();
        grande = false;
    }


    // Update is called once per frame
    void Update()
    {
        jump = 10;

        float x_movement = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        transform.Translate(x_movement, 0, 0);

        if (x_movement < 0)
        {
            anim.SetBool("Walk", true);
            transform.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (x_movement > 0)
        {
            anim.SetBool("Walk", true);
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            posX += speed * Time.deltaTime;
            sprite_renderer.flipX = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            posX -= speed * Time.deltaTime;
            sprite_renderer.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            if (floor || duct)
            {
                rigid_body.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                floor = false;
                duct = false;
                anim.SetBool("Jump", true);
            }
        }

        transform.Translate(posX, 0, 0);

        if (Input.GetKey(KeyCode.DownArrow) && duct)
        {
            StartCoroutine(WaitUntilDuctTrip());
            posX = 20.88f;
            transform.position = new Vector3(posX, 0, 0);
        }

        posX = 0;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        string object_tag = col.gameObject.tag;

        if ((object_tag == "floor" || object_tag == "box" ||
               object_tag == "mushroom_box" || object_tag == "coin_box") &&
               col.contacts[0].normal == new Vector2(0, 1))
        {
            floor = true;
            duct = false;
            anim.SetBool("Jump", false);
        }

        if (object_tag == "duct" && col.contacts[0].normal == new Vector2(0, 1))
        {
            duct = true;
            floor = false;
            anim.SetBool("Jump", false);
        }

        if (object_tag == "mushroom_box")
        {
            if (col.contacts[0].normal == new Vector2(0, -1) &&
            col.gameObject.GetComponent<SpriteRenderer>().sprite != inactive_box_sprite)
            {
                Vector2 pos_mushroom = col.transform.position;
                Instantiate(hongo, new Vector3(pos_mushroom.x, (pos_mushroom.y + 0.5f), 0),
                Quaternion.identity);
                col.gameObject.GetComponent<SpriteRenderer>().sprite = inactive_box_sprite;
                col.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (object_tag == "coin_box")
        {
            if (col.contacts[0].normal == new Vector2(0, -1) &&
            col.gameObject.GetComponent<SpriteRenderer>().sprite != inactive_box_sprite)
            {
                Vector2 pos = col.transform.position;
                Coin new_coin = (Coin)Instantiate(coin, new Vector3(pos.x, (pos.y + 0.5f), 0),
                Quaternion.identity);
                new_coin.is_alive = true;
                col.gameObject.GetComponent<SpriteRenderer>().sprite = inactive_box_sprite;
                col.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (object_tag == "box")
        {
            if (col.contacts[0].normal == new Vector2(0, -1) && this.grande)
            {
                col.gameObject.GetComponent<Animator>().SetBool("Broken", true);
                StartCoroutine(WaitUntilBlockIsDestroyed(col.gameObject));
            }
        }
    }

    public IEnumerator WaitUntilDuctTrip()
    {
        yield return new WaitForSeconds(1);
    }

    public IEnumerator WaitUntilBlockIsDestroyed(GameObject block)
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(block);
    }
}
