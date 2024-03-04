using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : Cell
{
    public int correctRotation { get; private set; }
    public void Rotate()
    {
        Debug.Log("ROTATED " + name + "!!!!!");
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, (transform.eulerAngles.y + 90f) % 360f, transform.eulerAngles.z);
    }

    public void SetCorrectRotation()
    {

    }

    public bool IsRotatedCorrectly()
    {
        return correctRotation == transform.eulerAngles.y;
    }
}
