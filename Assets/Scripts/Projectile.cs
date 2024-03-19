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

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            sh.UpdateCurrProjCount(1);
            Destroy(gameObject);
        }
    }
}
