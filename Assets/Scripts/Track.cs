using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : Cell
{
    /* 0 is left, 1 is straight, 2 is right */
    public int pieceType { get; private set; }
    public int correctRotation { get; private set; }

    private bool isCorrect;
    private bool locked;
    public void Rotate()
    {
        if (locked) return;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.RoundToInt((transform.eulerAngles.y + 90) % 360), transform.eulerAngles.z);
        isCorrect = IsRotatedCorrectly();
    }

    public void SetPieceType(int type)
    {
        if (locked) return;
        pieceType = type;

    }

    public void SetCorrectRotation(int rotation)
    {
        if (locked) return;
        correctRotation = rotation;
    }

    public bool IsRotatedCorrectly()
    {
        isCorrect = (correctRotation == Mathf.RoundToInt(transform.eulerAngles.y)) || (pieceType == 1 && (correctRotation == Mathf.RoundToInt((transform.eulerAngles.y + 180) % 360)));
        return isCorrect;
    }

    public void Lock()
    {
        locked = true;
    }
}
