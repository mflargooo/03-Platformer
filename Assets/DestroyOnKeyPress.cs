using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnKeyPress : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            Destroy(gameObject);
        }
    }
}
