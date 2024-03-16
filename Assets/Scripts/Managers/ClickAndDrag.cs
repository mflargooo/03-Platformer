using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    private Selectable selectedGameObject;
    Ray mouseRay;
    private RaycastHit[] hits;

    [SerializeField] private Transform defaultParent;
    [SerializeField] private float clickDetectRange;
    [SerializeField] private float selectedObjDepth;
    [SerializeField] private Camera cam;

    public void SetCamera(Camera cam)
    {
        this.cam = cam;
    }

    private void Update()
    {
        mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(mouseRay, clickDetectRange);

        if (Input.GetMouseButtonDown(0))
            MouseZeroDown();
        else if (Input.GetMouseButtonUp(0))
            MouseZeroUp();
        else if (Input.GetMouseButtonDown(1))
            MouseOneDown();
        if(selectedGameObject)
            UpdateSelectedObj();
    }

    public void SelectGameObject(Selectable obj)
    {
        selectedGameObject = obj;
        obj.OnSelect();
    }

    public void DeselectGameObject()
    {
        selectedGameObject.OnDeselect();
        selectedGameObject = null;
    }

    private void UpdateSelectedObj()
    {
        float scalar = (selectedObjDepth - mouseRay.origin.z) / mouseRay.direction.z;
        selectedGameObject.transform.position = mouseRay.direction * scalar + mouseRay.origin;
    }

    private void MouseZeroDown()
    {
        if (selectedGameObject)
            DeselectGameObject();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.tag == "Selectable") {
                SelectGameObject(hits[i].collider.gameObject.GetComponent<Selectable>());
            }
        }
    }

    private void MouseOneDown()
    {
        Selectable otherSelectable = null;
        if (!selectedGameObject)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                GameObject hit = hits[i].collider.gameObject;
                if (hit.tag == "Selectable")
                {
                    otherSelectable = hit.GetComponent<Selectable>();
                    break;
                }
            }

            if (otherSelectable)
            {
                otherSelectable.ResetPos();
                otherSelectable.transform.parent = defaultParent;
            }
        }
        else
        {
            selectedGameObject.ResetPos();
            selectedGameObject.transform.parent = defaultParent;
            DeselectGameObject();
        }
    }

    private void MouseZeroUp()
    {
        if (selectedGameObject)
        {
            GameObject slot = null;
            Selectable otherSelectable = null;
            for (int i = 0; i < hits.Length; i++)
            {
                GameObject hit = hits[i].collider.gameObject;
                if (hit.tag == "Slot")
                {
                    slot = hit;
                    break;
                }
                else if (hit != selectedGameObject.gameObject && hit.tag == "Selectable")
                {
                    otherSelectable = hit.GetComponent<Selectable>();
                }
            }
            if (slot)
            {
                Swap(selectedGameObject, slot);
            }
            else if (otherSelectable && selectedGameObject.transform.parent.tag == "Slot")
            {
                Swap(otherSelectable, selectedGameObject.transform.parent.gameObject);
            }
            else
            {
                selectedGameObject.ResetPos();
                selectedGameObject.transform.parent = defaultParent;
            }
            DeselectGameObject();
        }
    }

    private void Swap(Selectable selected, GameObject slot)
    {
        Transform slotCurrObj;
        try
        {
            slotCurrObj = slot.transform.GetChild(0);
        }
        catch
        {
            slotCurrObj = null; 
        }
        Transform selectedParent = selected.transform.parent;

        if (slotCurrObj && selectedParent.tag == "Slot")
        {
            slotCurrObj.transform.parent = selectedParent;
            slotCurrObj.transform.localPosition = -Vector3.forward * .5f;
        }
        else if (slotCurrObj)
        {
            slotCurrObj.transform.parent = defaultParent;
            slotCurrObj.GetComponent<Selectable>().ResetPos();

        }
        selected.transform.parent = slot.transform;
        selected.transform.localPosition = -Vector3.forward * .5f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(mouseRay.origin, mouseRay.direction * clickDetectRange);
    }
}
