using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{

    public float speed;
    private float Move;
    public float jump;
    private Rigidbody2D rb;
    public bool isGrounded;
    public static bool respawn = false;
    public int deathCounter = 0;
    private TextMeshPro textMeshPro;
    private int points = 0;

    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        textMeshPro = GetComponentInChildren<TextMeshPro>();

        // Check if the TextMesh component was found
        if (textMeshPro != null)
        {
            textMeshPro.text = "Deaths: " + deathCounter + "\nPoints: " + GameManager.Instance.points;
        }
        else
        {
            Debug.LogError("TextMesh component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        respawn = false;

        if(this.points != GameManager.Instance.points)
        {
            this.points = GameManager.Instance.points;
            textMeshPro.text = "Deaths: " + deathCounter + "\nPoints: " + GameManager.Instance.points;
        }

        if(!GameManager.Instance.paused)
        {
            Move = Input.GetAxis("Horizontal");

            rb.velocity = new Vector2(speed * Move, rb.velocity.y);

            Vector2 rayStart;

            //if bigger, different raycast
            if(gameObject.transform.localScale.Equals(new Vector3(1, 2, 1)))
            {
                rayStart = new Vector2(transform.position.x, transform.position.y - 1f);
            }
            else
            {
                rayStart = new Vector2(transform.position.x, transform.position.y - 0.5f);
            }

            Debug.DrawRay(rayStart, Vector2.down * groundCheckDistance, Color.red);
            // Debug.Log("Player Position: " + transform.position);
            // Debug.Log("Ground Check Distance: " + groundCheckDistance);

            isGrounded = Physics2D.Raycast(rayStart, Vector2.down, groundCheckDistance, groundLayer);
            //Debug.Log("Is Jumping: " + isGrounded);

            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(new Vector2(rb.velocity.x, jump));
                //isJumping = true;
            }

            //TESTING
            if(Input.GetKeyDown(KeyCode.Q))
            {
                GameManager.Instance.paused = true;
                rb.velocity = new Vector2(0f, 0f);
            }
        }
        else
        {
            //TESTING
            if(Input.GetKeyDown(KeyCode.Q))
            {
                GameManager.Instance.paused = false;
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(!GameManager.Instance.paused)
        {
            // if(other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("WallFloor"))
            // {
            //     Debug.Log("other: " + other.transform.position.y + " player: " + transform.position.y);
            //     if(other.transform.position.y < transform.position.y)
            //     {
            //         isJumping = false;
            //     }
            // }

            if(other.gameObject.CompareTag("death"))
            {
                if(gameObject.transform.localScale.Equals(new Vector3(1, 2, 1)))
                {
                    gameObject.transform.localScale -= new Vector3(0, 1, 0);
                }
                else
                {
                    respawn = true;
                    rb.position = new Vector2(-8.62f, 0.88f);
                    deathCounter++;
                    textMeshPro.text = "Deaths: " + deathCounter + "\nPoints: " + GameManager.Instance.points;
                }
            }

            //doesnt work b/c movement sets velocity instead of force, copilot recommends using wall raycast above 
            //and if touching wall, dont let player move in that direction
            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("WallFloor"))
            {
                rb.AddForce(new Vector2(-Mathf.Sign(rb.velocity.x) * 100f, 0f));
            }

            //if other object is tagged as "pauseBarrier" then set the game to pause
            // if(other.gameObject.CompareTag("pauseBarrier"))
            // {
            //     GameManager.Instance.paused = true;
            //     rb.velocity = new Vector2(0, 0);
            // }
        }
    }
}
