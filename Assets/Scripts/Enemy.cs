using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f;
    private float direction = 1f;
    private Rigidbody2D rb;
    private float startx;
    private float starty;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startx = rb.position.x;
        starty = rb.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.paused)
        {
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);

            if(Movement.respawn)
            {
                rb.position = new Vector2(startx, starty);
                Debug.Log("Respawned");
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("WallFloor"))
        {
            direction *= -1;
        }
    }
}
