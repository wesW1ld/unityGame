using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMove : MonoBehaviour
{

    private Vector2 pausePos;
    private float xMove;
    private float yMove;
    public float speed;

    private Rigidbody2D rb;
    public GameObject myPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.paused)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                rb.velocity = new Vector2(0f, 0f);
                transform.position = new Vector3(pausePos.x, pausePos.y, transform.position.z);
            }

            xMove = Input.GetAxis("Horizontal");
            yMove = Input.GetAxis("Vertical");

            rb.velocity = new Vector2(speed * xMove, speed * yMove);

            if(Input.GetKeyDown(KeyCode.T))
            {
                GameObject instance = Instantiate(myPrefab, transform.position, Quaternion.identity);
                instance.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y, 1);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                pausePos = transform.position;
            }
        }
    }
}
