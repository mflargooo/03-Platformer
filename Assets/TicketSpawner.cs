using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketSpawner : MonoBehaviour
{
    public Transform[] ticketSpawns;
    public GameObject ticket;
    public BirdBehavior birdBehavior;

    void Start()
    {
        SpawnTicket();
    }


    public void SpawnTicket()
    {
        if (ticketSpawns.Length == 0)
            return;

        int index = Random.Range(0, ticketSpawns.Length);
        GameObject spawnedTicket = Instantiate(ticket, ticketSpawns[index].position, Quaternion.identity);

        if (birdBehavior != null)
        {
            birdBehavior.ticketTransform = spawnedTicket.transform;
            birdBehavior.BeginCountdown();
        }
    }
}
