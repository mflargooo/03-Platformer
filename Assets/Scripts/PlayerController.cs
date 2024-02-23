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
    [SerializeField] private float baseGravityMultipler;
    [SerializeField] private float fallGravityMultiplier;
    [SerializeField] private float coyoteTime;
    private float coyTime;

    Vector2 input;
    Vector2 velocity;
    private float fallSpeed;
    bool reachedJumpPeak;
    [SerializeField] private float maxFallSpeed;

    [Header("Grounded")]
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector3 groundCheckBoxSize;
    [SerializeField] private Transform groundCheckCenter;

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
        isGrounded = Physics.OverlapBox(groundCheckCenter.position, groundCheckBoxSize * .5f, Quaternion.identity, whatIsGround).Length > 0 && rb.velocity.y <= 0;

        if (!isGrounded && coyTime > 0f) coyTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && coyTime >= 0f)
        {
            velocity.y = jumpSpeed;
            fallSpeed = 0f;
        }
        else if (!reachedJumpPeak && Input.GetKeyUp(KeyCode.Space))
        {
            reachedJumpPeak = true;
            velocity.y *= .25f;
        }
        else if (velocity.y <= 0f)
        {
            reachedJumpPeak = true;
            fallSpeed += Physics.gravity.y * (fallGravityMultiplier - 1f) * Time.deltaTime;
        }

        else fallSpeed = Mathf.Clamp(fallSpeed + baseGravityMultipler * Physics.gravity.y * Time.deltaTime, -maxFallSpeed, 0f);

        if (isGrounded && velocity.y <= 0f)
        {
            fallSpeed = 0f;
            reachedJumpPeak = false;
            velocity.y = 0f;
            coyTime = coyoteTime;
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, input.x * moveSpeed, ref currXVel, xVelSmoothTime);
        velocity.y += fallSpeed;

        rb.velocity = velocity;
    }
}
