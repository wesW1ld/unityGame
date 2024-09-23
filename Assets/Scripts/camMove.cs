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

    private Vector3Int spikePos;
    public GameObject spikePrefab;
    public Grid grid;

    Tilemap[] tilemaps;
    //update if more are added, or change to list for dynmaic size
    Tilemap[] floorTilemaps = new Tilemap[2];

    //spike thats a visual for placement
    private GameObject vSpike;
    public GameObject VspikePrefab;

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

                vSpike.transform.position = transform.position;

                if(Input.GetKeyDown(KeyCode.T))
                {
                    spikePos = grid.WorldToCell(transform.position);
                    Vector3 pos = grid.GetCellCenterWorld(spikePos);
                    //pos.y -= 0.2f;

                    if(placementRules(spikePos))
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
            }
        }
    }

    private Boolean placementRules(Vector3Int spikePos)
    {
        Boolean canPlace = true;

        //checks if there is a tile already at the position, not allowed
        for(int i = 0; i < tilemaps.Length; i++)
        {
            if(tilemaps[i].GetTile(spikePos) != null)
            {
                canPlace = false;
                Debug.Log("Tile already exists at position");
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
            canPlace = false;
            Debug.Log("No tile below position");
        }

        return canPlace;
    }
}
