using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectOrientation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localScale = transform.root.localScale;
    }
}
