using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTrainPuzzle : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Vector2 boundary;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lookOffset;

    Vector3 input;
    Vector3 lastDir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        CellSelect();
        Movement();
        Boundary();
    }

    void CellSelect()
    {
        lastDir = (input.magnitude != 0 ? input.normalized * lookOffset : lastDir);

        Physics.Raycast(transform.position + lastDir + Vector3.down * .01f, Vector3.down, out RaycastHit offHit, 1f, 1 << LayerMask.NameToLayer("Track"));
        Physics.Raycast(transform.position, Vector3.down + Vector3.down * .01f, out RaycastHit centerHit, 1, 1 << LayerMask.NameToLayer("Track"));

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (offHit.collider)
            {
                offHit.collider.GetComponent<Track>().Rotate();
            }
            else if (centerHit.collider)
            {
                centerHit.collider.GetComponent<Track>().Rotate();
            }
        }
    }

    void Movement()
    {
        transform.localScale = input.x != 0 ? new Vector3(input.x, 1f, 1f) : transform.localScale;
        rb.velocity = input.normalized * moveSpeed;
    }

    void Boundary()
    {
        if (transform.position.x < -boundary.x)
        {
            transform.position = new Vector3(-boundary.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > boundary.x)
        {
            transform.position = new Vector3(boundary.x, transform.position.y, transform.position.z);
        }

        if (transform.position.z < -boundary.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -boundary.y);
        }
        else if (transform.position.z > boundary.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, boundary.y);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + lastDir, transform.position + lastDir + Vector3.down);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down);
    }
}
