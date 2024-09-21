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
                Debug.Log(other.contacts.Length);
                for(int i = 0; i < other.contacts.Length; i++)
                {
                    Debug.Log("Contact Point: " + other.contacts[i].point);
                }

                Vector2 contactPoint = other.contacts[other.contacts.Length - 1].point;
                //contactPoint.y -= 1;
                Vector2 rcontactPoint = new Vector2(Mathf.Round(contactPoint.x), Mathf.Round(contactPoint.y));

                tilePos = Tilemap.WorldToCell(contactPoint);

                Debug.Log("Contact Point: " + contactPoint + "Tile Position: " + tilePos 
                + "rContact Point: " + rcontactPoint + "Player Position: " + other.transform.position);

                Tilemap.SetTile(tilePos, null);
            }
        }
    }
}

