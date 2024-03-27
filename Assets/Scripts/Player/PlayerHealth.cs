using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private int health;
    [SerializeField] private float invincibleTime;
    [SerializeField] private float timeBTWFlashes;
    [SerializeField] private Vector2 knockback;
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] private Animator anim;

    public bool isInvincible { get; private set; }

    [SerializeField] private GameObject heartContainer;
    [SerializeField] private float heartSpacing;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < health; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer.GetComponent<RectTransform>().position + Vector3.right * (heartSpacing * i), heartPrefab.transform.rotation);
            heart.transform.SetParent(heartContainer.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hit" && !isInvincible)
        {
            TakeDamage(1, other.transform.position.x - transform.position.x > 0 ? -1 : 1);
            StartCoroutine(InvincFrames());
        }
    }

    public void TakeDamage(int damage, int dir)
    {
        for (int i = 0; i < damage; i++)
        {
            Destroy(heartContainer.transform.GetChild(health - i - 1).gameObject);
            GetComponent<Rigidbody2D>().velocity = new Vector3(dir * knockback.x, knockback.y);
        }

        health -= damage;


        if(health <= 0f)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            Destroy(gameObject.GetComponent<Shoot>());
            Destroy(gameObject.GetComponent<PlayerController2D>());
            anim.Play("player_death");
            GetComponent<Rigidbody2D>().gravityScale = 2f;
            StartCoroutine(GameOver());
        }
    }

    IEnumerator InvincFrames()
    {
        float numFlashes = invincibleTime / timeBTWFlashes;
        isInvincible = true;
        for (int i = 0; i < numFlashes; i++)
        {
            spriteRend.enabled = false;
            yield return new WaitForSeconds(timeBTWFlashes * .5f);
            spriteRend.enabled = true;
            yield return new WaitForSeconds(timeBTWFlashes * .5f);
        }
        isInvincible = false;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}
