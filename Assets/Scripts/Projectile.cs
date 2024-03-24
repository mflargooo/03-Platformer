using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Shoot sh;

    [SerializeField] private float rotateSpeed;
    public void Launch(Vector3 launchVector, Shoot sh)
    {
        rb.velocity = launchVector;
        this.sh = sh;
    }

    public void Track(Transform player)
    {
        StartCoroutine(TrackPlayer(player));
    }

    IEnumerator TrackPlayer(Transform player)
    {
        while(true)
        {
            if ((player.transform.position - transform.position).magnitude > 12f)
                Hit();
            yield return null;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.tag == "Enemy")
        {
            Hit();
        }
    }

    void Hit()
    {
        sh.UpdateCurrProjCount(int.Parse(name), transform);
        Destroy(gameObject);
    }
}
