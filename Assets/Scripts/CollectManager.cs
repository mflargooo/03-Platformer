using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectManager : MonoBehaviour
{
    public Inventory inventory;
    public AudioClip audioclip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                AudioSource audioSource = hit.transform.GetComponent<AudioSource>();
                if (hit.transform.CompareTag("Cube"))
                {
                    inventory.AddItemToInventory();
                    hit.transform.gameObject.SetActive(false);
                    Camera.main.GetComponent<AudioSource>().PlayOneShot(audioSource.clip);
                }
            }
        }
    }
}





