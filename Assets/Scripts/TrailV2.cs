using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailV2 : MonoBehaviour
{
    [SerializeField] private GameObject[] trailers;
    [SerializeField] private float minDistToMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Follow(GameObject obj, GameObject follow)
    {
        Vector3 diff = follow.transform.position - obj.transform.position;
        while (true)
        {
            if (diff.magnitude > minDistToMove)
            {
                
            }
        }
    }
}
