using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform triggerLeft;
    [SerializeField] private Transform triggerRight;
    [SerializeField] private Transform tpPointLeft;
    [SerializeField] private Transform tpPointRight;
    [SerializeField] private GameObject leftLevelParent;
    [SerializeField] private GameObject rightLevelParent;

    [SerializeField] private Vector2 waypointSize;
    [SerializeField] private Vector2 waypointOffset;

    private void Start()
    {
        if (tpPointLeft == null)
        {
            tpPointLeft = triggerLeft;
        }

        if (tpPointRight == null)
        {
            tpPointRight = triggerRight;
        }
    }
    // Update is called once per frame
    void Update()
    {
        /* Left to Right */
        if(!TransitionManager.isTransitioning && 
            Mathf.Abs(TransitionManager.playerTransform.position.x - (triggerLeft.position.x + waypointOffset.x)) <= waypointSize.x * .5f && 
            Mathf.Abs(TransitionManager.playerTransform.position.y - (triggerLeft.position.y + waypointOffset.y)) <= waypointSize.y * .5f)
        {
            print("LEFT TO RIGHT TRIGGERED");
            StartCoroutine(TransitionManager.DoTransition(tpPointRight, leftLevelParent, rightLevelParent, 1));
        }

        /* Right to Left */
        else if (!TransitionManager.isTransitioning && 
                 Mathf.Abs(TransitionManager.playerTransform.position.x - (triggerRight.position.x + waypointOffset.x)) <= waypointSize.x * .5f && 
                 Mathf.Abs(TransitionManager.playerTransform.position.y - (triggerRight.position.y + waypointOffset.y)) <= waypointSize.y * .5f)
        {
            print("RIGHT TO LEFT TRIGGERED");
            StartCoroutine(TransitionManager.DoTransition(tpPointLeft, rightLevelParent, leftLevelParent, -1));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(triggerLeft.position, triggerRight.position);
        Gizmos.DrawWireCube(triggerLeft.position + (Vector3)waypointOffset, waypointSize);
        Gizmos.DrawWireCube(triggerRight.position + (Vector3)waypointOffset, waypointSize);
    }
}
