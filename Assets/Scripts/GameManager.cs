using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool paused = false;
    public int points = 0;
    public int lives = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Makes sure the object persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

// use GameManager.Instance.variableName to access
