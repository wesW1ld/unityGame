using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxDestructable : MonoBehaviour
{
    private bool isDestroyed = false;

    void Update()
    {
        // If the box is marked for destruction and hasn't been destroyed yet
        if(isDestroyed)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check if the player is falling onto the box
            if (other.transform.position.y < transform.position.y)
            {
                isDestroyed = true;
                
                // Modify the player's scale if it's a certain size
                if(other.gameObject.transform.localScale.Equals(new Vector3(1, 1, 1)))
                {
                    other.gameObject.transform.localScale += new Vector3(0, 1, 0);
                }
            }
        }
    }
}