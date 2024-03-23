using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oni : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float attackRange;
    [SerializeField] private float chaseRange;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private Animator anim;

    [SerializeField] private float loseChaseTime;
    private int health;

    private bool isChasing;
    private bool isAttacking;

    private Rigidbody2D rb;
    private PlayerController2D player;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        player = FindObjectOfType<PlayerController2D>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Idle());
    }
    public void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Idle()
    {
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        anim.SetBool("isChasing", false);
        while(!isChasing)
        {
            if (!player)
            {
                yield return null;
                continue;
            }

            if(Random.Range(0f, 1f) < .01f)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            }

            Vector3 diff = player.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, diff, diff.magnitude, (1 << LayerMask.NameToLayer("Player")) & (1 << LayerMask.NameToLayer("Ground")));
            if (hit) print(hit.collider.name);
            if (hit && hit.collider.gameObject == player.gameObject && diff.magnitude <= chaseRange && diff.x * transform.localScale.x > 0)
            {
                isChasing = true;
            }

            if (isChasing)
            {
                StartCoroutine(Chase());
                break;
            }
            yield return null;
        }
    }

    IEnumerator Chase()
    {
        anim.SetBool("isChasing", true);
        float lct = 0f;
        
        while(isChasing)
        {
            if(!player)
            {
                isChasing = false;
                StartCoroutine(Idle());
                break;
            }

            Vector3 diff = player.transform.position - transform.position;
            Vector3 dir = (diff.x > 0 ? 1 : -1) * Vector3.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, diff, diff.magnitude, (1 << LayerMask.NameToLayer("Player")) & (1 << LayerMask.NameToLayer("Ground")));

            transform.localScale = new Vector3(dir.x, transform.localScale.y, transform.localScale.z);
            rb.velocity = dir * chaseSpeed + Vector3.up * rb.velocity.y;

            if (diff.magnitude <= attackRange && !isAttacking)
            {
                anim.SetBool("isAttacking", true);
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                yield return new WaitForSeconds(1.1f);
                anim.SetBool("isAttacking", false);
            }

            if (player && hit.collider.gameObject != player.gameObject && !isAttacking)
                lct += Time.deltaTime;
            else 
                lct = 0f;

            if(lct >= loseChaseTime)
            {
                isChasing = false;
                StartCoroutine(Idle());
                break;
            }
            yield return null;
        }
    }

    public void SetChaseRange(float range)
    {
        chaseRange = range;
        StopAllCoroutines();
        StartCoroutine(Chase());
        isChasing = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Projectile")
        {
            health--;
        }

        if(health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (player)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (player.transform.position - transform.position).normalized * attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + (player.transform.position - transform.position).normalized * attackRange, transform.position + (player.transform.position - transform.position).normalized * chaseRange);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + (player.transform.position - transform.position).normalized * chaseRange, player.transform.position);
        }
    }
}
