using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Sprite> collectedItems = new List<Sprite>(); 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Sprite item)
    {
        if (collectedItems.Count < 7) 
        {
            collectedItems.Add(item);
        }
    }
}
