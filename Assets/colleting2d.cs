using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colleting2d : MonoBehaviour

{
    public float amplitude = 0.5f;
    public float speed = 0.5f;

    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = originalPosition + Vector3.up * Mathf.Sin(Time.time * speed) * amplitude;
    }

}
