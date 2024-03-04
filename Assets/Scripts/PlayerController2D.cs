using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private float currXVel;
    [Header("Movement")]
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
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckDist;

    [Header("Animation and Model")]
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject model;
    private float inAirAngleVel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Movement();
        AnimationParams();
    }

    void Movement()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDist,  whatIsGround);
        isGrounded = (hit.collider != null && hit.collider.tag == "Ground");

        if (Input.GetKeyDown(KeyCode.Space) && coyTime > 0f)
        {
            model.transform.eulerAngles = Vector3.zero;
            velocity.y = jumpSpeed;
            coyTime = 0f;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && !reachedJumpPeak)
        {
            velocity.y *= .35f;
            reachedJumpPeak = true;
        }
        else if (velocity.y <= 0f && !isGrounded)
        {
            reachedJumpPeak = true;
            velocity.y += Physics.gravity.y * (fallGravityMultiplier - baseGravityMultiplier) * Time.deltaTime;
        }

        if (isGrounded && coyTime <= 0f && velocity.y <= 0)
        {
            coyTime = coyoteTime;
        }
        else if (coyTime > 0f)
        {
            coyTime -= Time.deltaTime;
        }

        if (isGrounded && velocity.y <= 0f)
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

    void AnimationParams()
    {
        transform.localScale = input.x != 0 ? new Vector3(input.x, 1f, 1f) : transform.localScale;
        model.transform.rotation = Quaternion.Euler(Vector3.forward * Mathf.SmoothDampAngle(model.transform.rotation.eulerAngles.z, 1.5f * transform.localScale.x * rb.velocity.y, ref inAirAngleVel, .1f));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("Speed X", Mathf.Abs(velocity.x));
        anim.SetFloat("Velocity Y", velocity.y);
    }

    private void OnDrawGizmos()
    {
        if (rb)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * groundCheckDist);
        }
    }
}
