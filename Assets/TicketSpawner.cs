using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketSpawner : MonoBehaviour
{
    public Transform[] ticketSpawns;
    public GameObject ticket;
    public float checkInterval = 5f;
    public float transitionDuration = 1f;
    private Coroutine moveCoroutine;

    void Start()
    {

        SpawnTicket();
        StartCoroutine(CheckAndMoveTicket());
    }

    IEnumerator CheckAndMoveTicket()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            MoveTicketToRandomSpawnPoint();
        }
    }

    void SpawnTicket()
    {
        ticket.transform.position = ticketSpawns[0].position;
        FloatingObject floatingObject = ticket.GetComponent<FloatingObject>();
        if (floatingObject != null)
        {
            floatingObject.UpdateStartPosition(ticket.transform.position);
        }
    }

    void MoveTicketToRandomSpawnPoint()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        if (ticketSpawns.Length == 0) return;
        int randomIndex = Random.Range(0, ticketSpawns.Length);
        Vector3 newPosition = (Vector2)ticketSpawns[randomIndex].position;
        moveCoroutine = StartCoroutine(TransitionToNewPosition(newPosition));
    }

    IEnumerator TransitionToNewPosition(Vector3 newPosition)
    {
        if (ticket)
        {
            Vector3 startPos = ticket.transform.position;
            float t = 0.0f;

            while (t < 1.0f)
            {
                t += Time.deltaTime / transitionDuration;
                ticket.transform.position = Vector3.Lerp(startPos, newPosition, t);
                yield return null;
            }

            ticket.transform.position = newPosition;


            FloatingObject floatingObject = ticket.GetComponent<FloatingObject>();
            if (floatingObject != null)
            {
                floatingObject.UpdateStartPosition(newPosition);
            }
        }
    }
}
