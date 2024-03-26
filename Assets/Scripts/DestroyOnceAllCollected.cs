using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnceAllCollected : MonoBehaviour
{
    [SerializeField] private GameObject[] toCollect;

    private void Update()
    {
        bool destroy = true;
        foreach (GameObject obj in toCollect)
        {
            destroy = destroy && !obj;
        }

        if(destroy)
        {
            Destroy(gameObject);
        }
    }
}
