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
    [SerializeField] private bool isBossMinion;

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
        isChasing = false;
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, diff, diff.magnitude, ~(1 << LayerMask.NameToLayer("IgnorePlayer")));

            if (isBossMinion)
            {
                transform.localScale = new Vector3(diff.x > 0 ? 1 : -1, transform.localScale.y, transform.localScale.z);
            }

            if (isBossMinion || (hit && hit.collider.gameObject == player.gameObject && diff.magnitude <= chaseRange && diff.x * transform.localScale.x > 0))
            {
                StopAllCoroutines();
                isChasing = true;
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, diff, diff.magnitude, ~(1 << LayerMask.NameToLayer("IgnorePlayer")));

            transform.localScale = new Vector3(dir.x, transform.localScale.y, transform.localScale.z);
            rb.velocity = dir * chaseSpeed + Vector3.up * rb.velocity.y;

            if (lct < loseChaseTime && (!hit || hit.collider.tag != "Player"))
            {
                lct += Time.deltaTime;
                yield return null;
                continue;
            }
            else if (lct >= loseChaseTime)
            {
                StopAllCoroutines();
                isChasing = false;
                StartCoroutine(Idle());
                break;
            }
            else
            {
                lct = 0f;
            }

            if (diff.magnitude <= attackRange)
            {
                anim.SetBool("isAttacking", true);
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                yield return new WaitForSeconds(1.1f);
                anim.SetBool("isAttacking", false);
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            TakeDamage();
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
