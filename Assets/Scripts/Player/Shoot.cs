using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float maxLaunchSpeed;
    private int maxProjCount = 0;
    private int currProjCount;
    Vector3 mousePos;
    Vector3 playerPos;
    Vector3 launchVector;

    [SerializeField] private Projectile capybaraProjectile;
    private bool canShoot;

    // Update is called once per frame
    void Update()
    {
        if (canShoot && Input.GetMouseButtonDown(0) && currProjCount > maxProjCount)
        {
            mousePos = (Vector3)(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerPos = (Vector3)(Vector2)transform.position;
            launchVector = Vector3.ClampMagnitude(mousePos - playerPos, maxLaunchSpeed);

            Projectile proj = Instantiate(capybaraProjectile, playerPos, capybaraProjectile.transform.rotation);
            proj.Launch(launchVector, this);

            currProjCount--;
        }
    }

    public void IncrementProjCount()
    {
        maxProjCount++;
    }

    public void UpdateCurrProjCount(int i)
    {
        currProjCount += i;
    }

    public void CanShoot(bool b)
    {
        canShoot = b;
    }
}
