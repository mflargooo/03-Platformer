using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public class TrailNode
    {
        public Vector3 position { get; set; }
        public bool groundNode { get; set; }

        public TrailNode(Vector3 position, bool isGrounded)
        {
            this.position = position;
            groundNode = isGrounded;
        }
    }

    [SerializeField] private PlayerController2D pc;
    private Rigidbody2D rb;
    [SerializeField] private GameObject[] trailers;
    private LinkedList<TrailNode> trail = new LinkedList<TrailNode>();

    [SerializeField] private float maxMoveDistance;

    [SerializeField] private float timeBetweenCheckpoints;
    private float timer;

    private bool gotStopMovingPoint;
    // Start is called before the first frame update
    void Start()
    {
        rb = pc.GetComponent<Rigidbody2D>();
        
        TrailNode initial = new TrailNode(transform.position, pc.isGrounded);
        trail.AddLast(initial);
        
        for (int i = 0; i < trailers.Length; i++)
        {
            StartCoroutine(FollowTrail(trailers[i], i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude >= .1f)
        {
            gotStopMovingPoint = false;
            if (timer <= 0f)
            {
                timer = timeBetweenCheckpoints;
                trail.AddLast(new TrailNode(transform.position, pc.isGrounded));
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else if (!gotStopMovingPoint)
        {
            gotStopMovingPoint = true;
            trail.AddLast(new TrailNode(transform.position, pc.isGrounded));
        }
    }

    IEnumerator FollowTrail(GameObject obj, int i)
    {
        LinkedListNode<TrailNode> curr = trail.First;
        Vector3 toFollow = curr.Value.position;
        while (true)
        {
            if (rb.velocity.magnitude >= .1f || !curr.Value.groundNode)
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, toFollow, maxMoveDistance);

                if ((obj.transform.position - toFollow).magnitude < .1f)
                {
                    obj.transform.position = toFollow;
                    curr = curr.Next;
                    toFollow = curr.Value.position;
                    if (i == trailers.Length - 1) trail.RemoveFirst();
                }
            }
            yield return null;
        }
    }
}