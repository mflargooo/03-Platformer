using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float interactionRadius;
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private KeyCode interactKey;
    Collider2D[] hits;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, 1 << LayerMask.NameToLayer("Interactable"));
        GameObject closest = GetClosestHit(hits);

        if (closest)
        {
            interactPrompt.SetActive(true);
            if (Input.GetKeyDown(interactKey))
            {
                closest.GetComponent<IInteractable>().Interact();
            }
        }
        else
        {
            interactPrompt.SetActive(false);
        }
    }

    GameObject GetClosestHit(Collider2D[] hits)
    {
        float closest = interactionRadius + 1f;
        GameObject closestObj = null;
        foreach (Collider2D col in hits)
        {
            Vector2 pos1 = col.gameObject.transform.position;
            Vector2 pos2 = transform.position;
            if ((pos2 - pos1).magnitude < closest)
            {
                closestObj = col.gameObject;
            }
        }
        return closestObj;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
