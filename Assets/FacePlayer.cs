using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [SerializeField] PlayerController2D player;
    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<PlayerController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player)
            transform.localScale = new Vector3(player.transform.position.x - transform.position.x > 0 ? 1 : -1, transform.localScale.y, transform.localScale.z);
    }
}
