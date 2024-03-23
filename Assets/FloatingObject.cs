using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float amplitude = 0.5f; 
    public float frequency = 1f;
    private Vector3 startPos;
    private float tempVal;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tempVal = Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = startPos + new Vector3(0, tempVal, 0);
    }
    public void UpdateStartPosition(Vector3 newStartPos)
    {
        startPos = newStartPos;
    }
}
