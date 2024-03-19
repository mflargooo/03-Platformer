using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float maxLaunchSpeed;
    private int maxProjCount = 0;
    private int currProjCount = 0;
    Vector3 mousePos;
    Vector3 playerPos;
    Vector3 launchVector;
    Ray mouseRay;

    [SerializeField] private Projectile capybaraProjectile;
    private bool canShoot;

    // Update is called once per frame
    void Update()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float c = (transform.position.z - mouseRay.origin.z) / mouseRay.direction.z;
        if (canShoot && Input.GetMouseButtonDown(0) && currProjCount > 0)
        {
            Debug.Log("SHOT");
            mousePos = (Vector2)(mouseRay.direction * c + mouseRay.origin);
            playerPos = (Vector2)transform.position;
            launchVector = Vector2.ClampMagnitude(mousePos - playerPos, maxLaunchSpeed);

            Projectile proj = Instantiate(capybaraProjectile, playerPos, capybaraProjectile.transform.rotation);
            proj.Launch(launchVector, this);

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
