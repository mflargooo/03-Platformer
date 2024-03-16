using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform waypointLeft;
    [SerializeField] private Transform waypointRight;
    [SerializeField] private GameObject leftLevelParent;
    [SerializeField] private GameObject rightLevelParent;

    [SerializeField] private Vector2 waypointSize;
    [SerializeField] private Vector2 waypointOffset;

    // Update is called once per frame
    void Update()
    {
        /* Left to Right */
        if(!TransitionManager.isTransitioning && 
            Mathf.Abs(TransitionManager.playerTransform.position.x - (waypointLeft.position.x + waypointOffset.x)) <= waypointSize.x * .5f && 
            Mathf.Abs(TransitionManager.playerTransform.position.y - (waypointLeft.position.y + waypointOffset.y)) <= waypointSize.y * .5f)
        {
            print("LEFT TO RIGHT TRIGGERED");
            StartCoroutine(TransitionManager.DoTransition(waypointRight, leftLevelParent, rightLevelParent, 1));
        }

        /* Right to Left */
        else if (!TransitionManager.isTransitioning && 
                 Mathf.Abs(TransitionManager.playerTransform.position.x - (waypointRight.position.x + waypointOffset.x)) <= waypointSize.x * .5f && 
                 Mathf.Abs(TransitionManager.playerTransform.position.y - (waypointRight.position.y + waypointOffset.y)) <= waypointSize.y * .5f)
        {
            print("RIGHT TO LEFT TRIGGERED");
            StartCoroutine(TransitionManager.DoTransition(waypointLeft, rightLevelParent, leftLevelParent, -1));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(waypointLeft.position, waypointRight.position);
        Gizmos.DrawWireCube(waypointLeft.position + (Vector3)waypointOffset, waypointSize);
        Gizmos.DrawWireCube(waypointRight.position + (Vector3)waypointOffset, waypointSize);
    }
}
