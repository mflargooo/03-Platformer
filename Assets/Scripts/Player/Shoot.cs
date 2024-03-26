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

    [SerializeField] private float shootCooldown;
    private float stcd;

    CapybaraCluster cc = null;

    [SerializeField] private Projectile capybaraProjectile;
    private bool canShoot;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapybaraCluster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currProjCount > maxProjCount) currProjCount = maxProjCount;

        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (stcd <= 0 && canShoot && Input.GetMouseButtonDown(0) && currProjCount > 0)
        {
            ShootProj(cc.GetFirstCapybaraOrder());
            cc.RemoveFirstCapybaraOrder();
            stcd = shootCooldown;
        }
        else if (stcd > 0)
        {
            stcd -= Time.deltaTime;
        }
    }

    void ShootProj(int id)
    {
        float c = (transform.position.z - mouseRay.origin.z) / mouseRay.direction.z;

        mousePos = (Vector2)(mouseRay.direction * c + mouseRay.origin);
        playerPos = (Vector2)transform.position;
        launchVector = Vector2.ClampMagnitude(mousePos - playerPos, 3f);

        Projectile proj = Instantiate(capybaraProjectile, playerPos, capybaraProjectile.transform.rotation);
        proj.name = id.ToString();
        proj.Track(transform);
        float acute = Vector3.Dot(launchVector, Input.GetAxisRaw("Horizontal") * Vector3.right);
        proj.Launch(launchVector * (maxLaunchSpeed / 3f + (acute >= 0 ? .5f : 0)), this);

        cc.GetCapybaraTransform(id).gameObject.SetActive(false);

        currProjCount--;

        SoundManager.PlayThrowSound();
    }

    public void IncrementProjCount()
    {
        maxProjCount++;
        currProjCount++;
    }

    public void UpdateCurrProjCount(int id, Transform proj)
    {
        currProjCount++;
        cc.GetCapybaraTransform(id).position = proj.position;
        cc.GetCapybaraTransform(id).gameObject.SetActive(true);
        cc.AddCapybaraOrder(id);
    }

    public void CanShoot(bool b)
    {
        canShoot = b;
    }
}
