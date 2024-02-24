using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float currXVel;
    [SerializeField] private float xVelSmoothTime;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float baseGravityMultiplier;
    [SerializeField] private float fallGravityMultiplier;
    [SerializeField] private float coyoteTime;
    private float coyTime;

    Vector2 input;
    Vector2 velocity;
    bool reachedJumpPeak;
    [SerializeField] private float maxFallSpeed;

    [Header("Grounded")]
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDist;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Movement();
    }

    void Movement()
    {
        //float groundCheckDistScaled = groundCheckDist * Mathf.Abs(rb.velocity.y < 0 ? rb.velocity.y * .25f : 0) / maxFallSpeed;
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDist + transform.localScale.y, whatIsGround);
        isGrounded = (hit.collider != null && hit.collider.tag == "Ground");

        if(Input.GetKeyDown(KeyCode.Space) && coyTime > 0f)
        {
            velocity.y = jumpSpeed;
            coyTime = 0f;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && !reachedJumpPeak)
        {
            velocity.y *= .35f;
            reachedJumpPeak = true;
        }
        else if (rb.velocity.y <= 0f && !isGrounded)
        {
            reachedJumpPeak = true;
            velocity.y += Physics.gravity.y * (fallGravityMultiplier - baseGravityMultiplier) * Time.deltaTime;
        }

        if(isGrounded && coyTime <= 0f)
        {
            coyTime = coyoteTime;
        }
        else if (coyTime > 0f)
        {
            coyTime -= Time.deltaTime;
        }

        if (isGrounded && velocity.y < 0f)
        {
            reachedJumpPeak = false;
            velocity.y = 0;
        }
        else
        {
            velocity.y += Physics.gravity.y * baseGravityMultiplier * Time.deltaTime;
        }

        velocity.y = Mathf.Clamp(velocity.y, -maxFallSpeed, jumpSpeed);

        velocity.x = Mathf.SmoothDamp(velocity.x, input.x * moveSpeed, ref currXVel, xVelSmoothTime);
        rb.velocity = velocity;
    }

    private void OnDrawGizmos()
    {
        if (rb)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (groundCheckDist * rb.velocity.y / maxFallSpeed + transform.localScale.y));
        }
    }
}
