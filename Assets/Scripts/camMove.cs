using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class camMove : MonoBehaviour
{

    private Vector2 pausePos;
    private float xMove;
    private float yMove;
    public float speed;
    private Rigidbody2D rb;
    private Vector3 previousPosition;

    private Vector3Int spikePos;
    public GameObject spikePrefab;
    public Grid grid;

    Tilemap[] tilemaps;
    //update if more are added, or change to list for dynmaic size
    Tilemap[] floorTilemaps = new Tilemap[1];

    //spike thats a visual for placement
    private GameObject vSpike;
    public GameObject VspikePrefab;
    private Vector3 newPos;
    private Vector3Int newPos2;
    private SpriteRenderer vSprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //gets all the tilemaps in the scene and puts them into an array
        tilemaps = FindObjectsOfType<Tilemap>();
        int j = 0;
        for(int i = 0; i < tilemaps.Length; i++)
        {
            if(tilemaps[i].gameObject.CompareTag("Floor") || tilemaps[i].gameObject.CompareTag("WallFloor"))
            {
                floorTilemaps[j] = tilemaps[i];
                j++;
            }
        }

        previousPosition = transform.position;

        pausePos = transform.position;
        vSpike = Instantiate(VspikePrefab, pausePos, Quaternion.identity);
        vSprite = vSpike.GetComponent<SpriteRenderer>();
        vSprite.color = new Color(1f, 1f, 1f, 0.5f);
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
                Destroy(vSpike);
            }
            else
            {
                xMove = Input.GetAxis("Horizontal");
                yMove = Input.GetAxis("Vertical");

                rb.velocity = new Vector2(speed * xMove, speed * yMove);

                //only updates sprite if the cam position has changed
                if (transform.position != previousPosition)
                {
                    previousPosition = transform.position;
                
                    newPos = new Vector3(transform.position.x, transform.position.y, 3f);
                    newPos2 = grid.WorldToCell(newPos);
                    vSpike.transform.position = grid.GetCellCenterWorld(newPos2);
                    if(PlacementRules(newPos2, true))
                    {
                        vSprite.color = new Color(1f, 1f, 1f, 0.7f);
                    }
                    else
                    {
                        vSprite.color = new Color(1f, 1f, 1f, 0.25f);
                    }
                }

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    spikePos = grid.WorldToCell(transform.position);
                    Vector3 pos = grid.GetCellCenterWorld(spikePos);
                    //pos.y -= 0.2f;

                    if(PlacementRules(spikePos, false))
                    {
                        Instantiate(spikePrefab, pos, Quaternion.identity);
                        GameManager.Instance.points++;
                    }
                }
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                pausePos = transform.position;
                vSpike = Instantiate(VspikePrefab, pausePos, Quaternion.identity);
                vSprite = vSpike.GetComponent<SpriteRenderer>();
                vSprite.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }

    private Boolean PlacementRules(Vector3Int spikePos, Boolean isVisual)
    {

        //checks if there is a tile already at the position, not allowed
        for(int i = 0; i < tilemaps.Length; i++)
        {
            if(tilemaps[i].GetTile(spikePos) != null)
            {
                if(!isVisual)
                {
                    Debug.Log("Tile already exists at position");
                }
                return false;
            }
        }

        // Check if there is a spike or player at the position
        Vector3 worldPos = grid.GetCellCenterWorld(spikePos);
        Collider2D[] colliders = Physics2D.OverlapPointAll(worldPos);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("death"))
            {
                if (!isVisual)
                {
                    Debug.Log("Spike already exists at position");
                }
                return false;
            }
            else if (collider.CompareTag("Player"))
            {
                if (!isVisual)
                {
                    Debug.Log("Player already exists at position");
                }
                return false;
            }
        }

        Vector3Int below = new Vector3Int(spikePos.x, spikePos.y - 1, spikePos.z);
        Boolean tileBelow = false;

        //checks if there is a tile below the position, required
        for(int i = 0; i < floorTilemaps.Length; i++)
        {
            if(floorTilemaps[i].GetTile(below) != null)
            {
                tileBelow = true;
            }
        }

        if(!tileBelow)
        {
            if(!isVisual)
            {
                Debug.Log("No eligible tiles below position");
            }
            return false;
        }

        return true;
    }
}