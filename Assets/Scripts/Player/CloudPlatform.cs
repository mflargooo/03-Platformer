using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPlatform : MonoBehaviour
{
    [SerializeField] private Transform falseFloorPoint;
    [SerializeField] private GameObject falseFloorPrefab;
    [SerializeField] private GameObject falseFloor;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckDist;

    RaycastHit2D groundCheck;
    private bool isGrounded;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        groundCheckPoint.localPosition = col.offset - col.size / 2;

        rb = FindObjectOfType<PlayerController2D>().GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        groundCheck = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDist, whatIsGround);
        isGrounded = groundCheck.collider;

        if(!isGrounded && !falseFloor && Mathf.Abs(rb.velocity.y) <= .025f)
        {
            falseFloor = Instantiate(falseFloorPrefab, falseFloorPoint.position, falseFloorPoint.rotation);
            falseFloor.transform.parent = falseFloorPoint;
            falseFloor.transform.localScale = Vector3.one;
        }
        else if (isGrounded && falseFloor)
        {
            Destroy(falseFloor);
        }
    }
}
