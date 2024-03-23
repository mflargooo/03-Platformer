using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketSpawner : MonoBehaviour
{
    public Transform[] ticketSpawns;
    public GameObject ticket;
    public BirdBehavior birdBehavior;

    private float timer = 0f;
    private float spawnDelay = 10f; 

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnDelay)
        {
            MoveTicketToRandomSpawnPoint();
            timer = 0f; 
        }
    }

    void MoveTicketToRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, ticketSpawns.Length);
        Vector3 newPosition = ticketSpawns[randomIndex].position;

        birdBehavior?.PickUpTicket(ticket.transform.position, newPosition);
    }
}
