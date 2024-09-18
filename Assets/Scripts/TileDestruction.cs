using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestruction : MonoBehaviour
{  
    Vector3Int tilePos;
    private Tilemap Tilemap;

    // Start is called before the first frame update
    void Start()
    {
        Tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.transform.position.y < transform.position.y || gameObject.CompareTag("death"))
            {
                Debug.Log("Contact Point: " + other.contacts[0].point);

                //gets the position of the tile that collided with the player
                tilePos = Tilemap.WorldToCell(other.contacts[0].point);
                Debug.Log("Tile Position: " + tilePos);

                //removes tile
                Tilemap.SetTile(tilePos, null);
            }
        }
    }
}

