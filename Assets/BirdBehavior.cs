using UnityEngine;
using System.Collections;

public class BirdBehavior : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform ticketTransform; // 当前车票的Transform
    private Transform[] possibleNewPositions; // 可能的新位置
    private bool isCarryingTicket = false;

    // 设置可能的新位置
    public void SetPossibleNewPositions(Transform[] newPositions)
    {
        possibleNewPositions = newPositions;
    }

    private void Update()
    {
        // 如果飞鸟拾取了车票并且有一个目标位置
        if (isCarryingTicket && ticketTransform != null && possibleNewPositions != null)
        {
            // 选择一个新位置并将车票移动过去
            int index = Random.Range(0, possibleNewPositions.Length);
            Transform newTargetPosition = possibleNewPositions[index];

            // 飞向新位置
            ticketTransform.position = Vector3.MoveTowards(ticketTransform.position, newTargetPosition.position, moveSpeed * Time.deltaTime);

            // 检查是否到达新位置
            if (Vector3.Distance(ticketTransform.position, newTargetPosition.position) < 0.1f)
            {
                // 到达新位置，放下车票
                isCarryingTicket = false;
                ticketTransform = null; // 飞鸟不再关联车票
            }
        }
    }

    private IEnumerator StartTicketTimer()
    {
        yield return new WaitForSeconds(5f); // 等待5秒
        if (ticketTransform != null)
        {
            // 飞鸟开始携带车票
            isCarryingTicket = true;
        }
    }

    public void BeginCountdown()
    {
        StartCoroutine(StartTicketTimer());
    }

    public void TakeTicket(Transform ticket)
    {
        // 将飞鸟的位置移动到车票位置，并设置飞鸟的ticketTransform
        ticketTransform = ticket;
        BeginCountdown(); // 开始计时器
    }
}
