using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public Vector3 homePos { get; private set; }

    public void SetHomePos(Vector3 pos)
    {
        homePos = pos;
    }

    public void ResetPos()
    {
        transform.position = homePos;
    }

    public virtual void OnSelect() { }
    public virtual void OnDeselect() { }
}
