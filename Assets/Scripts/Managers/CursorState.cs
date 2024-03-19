using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorState : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform cursor;
    [SerializeField] private Sprite defaultCursor;
    [SerializeField] private Sprite hoveredCursor;

    private static Sprite dc;
    private static Sprite hc;
    private static Transform c;

    // Start is called before the first frame update
    void Start()
    {
        c = cursor;
        dc = defaultCursor;
        hc = hoveredCursor;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        cursor.position = cam.ScreenToWorldPoint(Input.mousePosition) - Vector3.up;
    }

    public static void Default()
    {
        c.GetComponent<SpriteRenderer>().sprite = dc;
    }

    public static void Hovered()
    {
        c.GetComponent<SpriteRenderer>().sprite = hc;
    }
}
