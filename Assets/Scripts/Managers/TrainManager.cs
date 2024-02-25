using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    Cell[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = GridManager.GetGrid();
        int gridSize = Mathf.RoundToInt(Mathf.Sqrt(grid.Length));

        foreach (Cell c in grid)
        {
            c.SetWeight(Random.Range(0f, 20f));
        }

        List<Cell> path = GenerateTrack.randomPath(grid, Random.Range(0, gridSize), 0);

        for(int i = 0; i < path.Count; i++)
        {
            path[i].name = "Path: " + i.ToString();
            path[i].transform.position += Vector3.up;
        }
    }
}
