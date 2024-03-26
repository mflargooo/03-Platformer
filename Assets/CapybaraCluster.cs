using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapybaraCluster : MonoBehaviour
{
    private Transform player;
    List<int> capybaraOrder = new List<int>();
    [SerializeField] private Transform[] capybaraPoints;
    [SerializeField] private GameObject[] capybaras;

    private PlayerController2D pc;

    public void OnEnable()
    {
        StopAllCoroutines();
        pc = GetComponent<PlayerController2D>();
        player = transform.root;
        for (int i = 0; i < capybaraPoints.Length; i++)
        {
            StartCoroutine(Follow(i));
        }
    }

    public void AddCapybara(int i)
    {
        Transform spriteObj = capybaras[capybaraOrder.Count].transform.GetChild(0).GetChild(0);
        if(capybaraOrder.Count == 6)
        {
            spriteObj = spriteObj.GetChild(0);
        }

        if (i == 0)
            spriteObj.GetChild(5).gameObject.SetActive(true);
        else if (i == 1)
            spriteObj.GetChild(0).GetChild(1).gameObject.SetActive(true);
        else if (i == 2)
            spriteObj.GetChild(0).GetChild(2).gameObject.SetActive(true);
        else if (i == 3)
            spriteObj.GetChild(0).GetChild(3).gameObject.SetActive(true);
        else if (i == 4)
            spriteObj.GetChild(6).gameObject.SetActive(true);
        else if (i == 5)
            spriteObj.GetChild(0).GetChild(4).gameObject.SetActive(true);
        else if (i == 6)
            spriteObj.GetChild(0).GetChild(5).gameObject.SetActive(true);

        capybaras[capybaraOrder.Count].SetActive(true);
        capybaraOrder.Add(capybaraOrder.Count);
    }

    IEnumerator Follow(int i)
    {
        GameObject capybara = capybaras[i];
        Transform target = capybaraPoints[i];
        Vector3 diff;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Animator anim = null;
        if (i < 6)
            anim = capybara.transform.GetChild(0).GetComponent<Animator>();
        
        while(true)
        {
            while(!rb.gameObject.activeInHierarchy)
            {
                yield return null;
                continue;
            }
            diff = target.position - capybara.transform.position;
            if (diff.magnitude <= .75f || i == capybaras.Length - 1)
            {
                capybara.transform.position = target.position;
                capybara.transform.localScale = player.transform.localScale;
            }
            else
            {
                int dir = (diff.x > 0 ? 1 : -1);
                capybara.transform.localScale = new Vector3(dir, capybara.transform.localScale.y, capybara.transform.localScale.z);
                capybara.transform.Translate(diff.normalized * player.GetComponent<PlayerController2D>().GetSpeed() * Time.deltaTime * (diff.magnitude > 3f ? 5f : 1.5f));
            }

            if (i < 6)
            {
                anim.SetFloat("Speed X", (diff.magnitude > .75f ? 3 : Mathf.Abs(rb.velocity.x)));
                anim.SetFloat("Velocity Y", rb.velocity.y);
                anim.SetBool("isGrounded", pc.isGrounded);
            }
            yield return null;
        }
    }

    public Transform GetCapybaraTransform(int i)
    {
        return capybaras[i].transform;
    }

    public int GetFirstCapybaraOrder()
    {
        return capybaraOrder[0];
    }

    public void AddCapybaraOrder(int id)
    {
        capybaraOrder.Add(id);
    }

    public void RemoveFirstCapybaraOrder()
    {
        capybaraOrder.RemoveAt(0);
    }

    public List<int> GetCapybaraOrderList()
    {
        return capybaraOrder;
    }
}
