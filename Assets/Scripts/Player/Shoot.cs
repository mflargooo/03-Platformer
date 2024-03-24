using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float maxLaunchSpeed;
    private int maxProjCount;
    private int currProjCount;
    Vector3 mousePos;
    Vector3 playerPos;
    Vector3 launchVector;
    Ray mouseRay;

    [SerializeField] private Projectile capybaraProjectile;
    private bool canShoot;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CanShoot(true);
        IncrementProjCount();
        IncrementProjCount();
    }

    // Update is called once per frame
    void Update()
    {
        if (currProjCount > maxProjCount) currProjCount = maxProjCount;

        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float c = (transform.position.z - mouseRay.origin.z) / mouseRay.direction.z;
        if (canShoot && Input.GetMouseButtonDown(0) && currProjCount > 0)
        {
            mousePos = (Vector2)(mouseRay.direction * c + mouseRay.origin);
            playerPos = (Vector2)transform.position;
            launchVector = Vector2.ClampMagnitude(mousePos - playerPos, 3f);

            Projectile proj = Instantiate(capybaraProjectile, playerPos, capybaraProjectile.transform.rotation);
            float acute = Vector3.Dot(launchVector, Input.GetAxisRaw("Horizontal") * Vector3.right);
            proj.Launch(launchVector * (maxLaunchSpeed / 3f + (acute >= 0 ? .5f : 0)), this);

            currProjCount--;
        }
    }

    public void IncrementProjCount()
    {
        maxProjCount++;
        currProjCount++;
    }

    public void UpdateCurrProjCount(int i)
    {
        currProjCount += i;
    }

    public void CanShoot(bool b)
    {
        Debug.Log("SHOOT STATUS: " + b);
        canShoot = b;
    }
}
