using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour {

    float posX;
    bool mov = true;
    public float velocidad;
    public PlayerController player;
	
	// Update is called once per frame
	void Update () {

        if (mov)
        {
            posX = velocidad;
        }
        else
        {
            posX = velocidad * -1;
        }

        transform.Translate(posX, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        string object_tag = col.gameObject.tag;
        if (col.contacts[0].normal == new Vector2(1,0) || col.contacts[0].normal == new Vector2(-1, 0))
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
            Destroy(this.gameObject);
            player.transform.localScale = new Vector3(2.7f,2.7f,2.7f);
            player.grande = true;
        }

    }
    
}
