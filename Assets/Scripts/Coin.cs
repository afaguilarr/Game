using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody2D rigid_body;
    public float impulse;
    public float lifetime;
    public bool is_alive = false;

    // Update is called once per frame
    void Start()
    {
        impulse = 3;
        rigid_body = GetComponent<Rigidbody2D>();
        rigid_body.AddForce(Vector2.up * impulse, ForceMode2D.Impulse);
        lifetime = 0;
    }

    private void Update()
    {
        if (is_alive)
        {
            lifetime++;
        }

        if (lifetime >= 25)
        {
            Destroy(this.gameObject);
        }
    }

}
