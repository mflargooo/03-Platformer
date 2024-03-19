using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform triggerLeft;
    [SerializeField] private Transform triggerRight;
    [SerializeField] private Transform tpPointLeft;
    [SerializeField] private Transform tpPointRight;
    [SerializeField] private GameObject leftLevelParent;
    [SerializeField] private GameObject rightLevelParent;

    [SerializeField] private Vector2 triggerSize;
    [SerializeField] private Vector2 triggerOffset;

    [SerializeField] private UnityEvent leftToRight;
    [SerializeField] private UnityEvent rightToLeft;

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
            Mathf.Abs(TransitionManager.playerTransform.position.x - (triggerLeft.position.x + triggerOffset.x)) <= triggerSize.x * .5f && 
            Mathf.Abs(TransitionManager.playerTransform.position.y - (triggerLeft.position.y + triggerOffset.y)) <= triggerSize.y * .5f)
        {
            print("LEFT TO RIGHT TRIGGERED");
            StartCoroutine(TransitionManager.DoTransition(tpPointRight, leftLevelParent, rightLevelParent, 1));
            leftToRight.Invoke();
        }

        /* Right to Left */
        else if (!TransitionManager.isTransitioning && 
                 Mathf.Abs(TransitionManager.playerTransform.position.x - (triggerRight.position.x + triggerOffset.x)) <= triggerSize.x * .5f && 
                 Mathf.Abs(TransitionManager.playerTransform.position.y - (triggerRight.position.y + triggerOffset.y)) <= triggerSize.y * .5f)
        {
            print("RIGHT TO LEFT TRIGGERED");
            StartCoroutine(TransitionManager.DoTransition(tpPointLeft, rightLevelParent, leftLevelParent, -1));
            rightToLeft.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(triggerLeft.position, triggerRight.position);
        Gizmos.DrawWireCube(triggerLeft.position + (Vector3)triggerOffset, triggerSize);
        Gizmos.DrawWireCube(triggerRight.position + (Vector3)triggerOffset, triggerSize);
    }
}
