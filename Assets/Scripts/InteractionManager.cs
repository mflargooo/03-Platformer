using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown() 
    {
        if (GameManager.Instance.collectedItems.Count == 7) 
        {
            SceneManager.LoadScene("1"); 
        }
    }
}


