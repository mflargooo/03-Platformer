using System.Collections;
using UnityEngine;

public class BirdBehavior : MonoBehaviour
{
    public GameObject ticket;
    private Transform ticketTransform;
    private Vector3 originalPosition; // 记录鸟的初始位置

    private float speed = 5f;

    void Start()
    {
        ticketTransform = ticket.transform;
        originalPosition = transform.position; // 在Start时保存鸟的初始位置
    }

    public void PickUpTicket(Vector3 ticketPosition, Vector3 newTicketPosition)
    {
        StartCoroutine(FlyToTicket(ticketPosition, newTicketPosition));
    }

    IEnumerator FlyToTicket(Vector3 ticketPosition, Vector3 newTicketPosition)
    {
        yield return MoveToPosition(ticketPosition); // 飞向票据的位置

        ticketTransform.SetParent(transform); // 捡起票据

        // 禁用FloatingObject脚本
        FloatingObject floatingScript = ticket.GetComponent<FloatingObject>();
        bool wasFloating = floatingScript != null && floatingScript.enabled;
        if (wasFloating)
        {
            floatingScript.enabled = false;
        }

        yield return MoveToPosition(newTicketPosition); // 飞向新位置

        ticketTransform.SetParent(null); // 放下票据

        // 如果票据具有FloatingObject脚本，则重新启用并更新其开始位置
        if (wasFloating)
        {
            floatingScript.enabled = true;
            floatingScript.UpdateStartPosition(ticketTransform.position);
        }

        // 让鸟回到初始位置
        yield return MoveToPosition(originalPosition);
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}
