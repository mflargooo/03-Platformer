using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : Cell
{
    /* 0 is left, 1 is straight, 2 is right */
    public int pieceType;
    public int correctRotation;

    public bool isCorrect;
    public void Rotate()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.RoundToInt((transform.eulerAngles.y + 90) % 360), transform.eulerAngles.z);
        isCorrect = IsRotatedCorrectly();
    }

    public void SetPieceType(int type)
    {
        pieceType = type;

    }

    public void SetCorrectRotation(int rotation)
    {
        correctRotation = rotation;
    }

    public bool IsRotatedCorrectly()
    {
        isCorrect = (correctRotation == Mathf.RoundToInt(transform.eulerAngles.y)) || (pieceType == 1 && (correctRotation == Mathf.RoundToInt((transform.eulerAngles.y + 180) % 360)));
        return isCorrect;
    }
}
