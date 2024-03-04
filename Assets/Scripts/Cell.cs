using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public void Interact(GameObject obj)
    {

    }
    public float weight { get; private set; }
    public bool searched { get; private set; }

    public void SetWeight(float weight)
    {
        this.weight = weight;
    }

    public void SetSearched(bool b)
    {
        searched = b;
    }
}
