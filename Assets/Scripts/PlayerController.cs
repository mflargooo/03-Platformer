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
        isGrounded = Physics.OverlapBox(groundCheckCenter.position, groundCheckBoxSize / 2f, Quaternion.identity, whatIsGround).Length > 0 && rb.velocity.y <= 0;

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            velocity.y = jumpSpeed;
        }
        else if (!reachedJumpPeak && Input.GetKeyUp(KeyCode.Space))
        {
            reachedJumpPeak = true;
            velocity.y *= .5f;
        }
        else if (velocity.y <= 0f)
        {
            fallSpeed += Physics.gravity.y * (fallGravityMultiplier - 1f) * Time.deltaTime;
        }

        if (isGrounded)
        {
            fallSpeed = 0f;
            reachedJumpPeak = false;
        }
        else fallSpeed = Mathf.Clamp(fallSpeed + baseGravityMultipler * Physics.gravity.y * Time.deltaTime, -maxFallSpeed, 0f);

        velocity.x = Mathf.SmoothDamp(velocity.x, input.x * moveSpeed, ref currXVel, xVelSmoothTime);
        velocity.y += fallSpeed;

        rb.velocity = velocity;
    }
}
