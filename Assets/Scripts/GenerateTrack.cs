using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTrack : MonoBehaviour
{
    public static List<Cell> randomPath (Cell[,] grid, int si, int sj, int maxTracks)
    {
        int dir = -1;
        int gridSize = Mathf.RoundToInt(Mathf.Sqrt(grid.Length));

        List<Cell> path = new List<Cell>();
        path.Add(grid[si, sj]);
        grid[si, sj].SetSearched(true);

        while (sj < gridSize - 1 && maxTracks > 0)
        {
            dir = Random.Range(0, 3);
            if(dir == 0 && si > 1 /* left */)
            {
                if (!grid[si - 1, sj].searched)
                    si--;
            }
            else if(dir == 1 /* up */)
            {
                if(!grid[si, sj + 1].searched)
                    sj++;
            }
            else if(/* dir == 2  && right */ si < gridSize - 1)
            {
                if(!grid[si + 1, sj].searched)
                    si++;
            }

            if (!path.Contains(grid[si, sj]))
            {
                grid[si, sj].SetSearched(true);
                path.Add(grid[si, sj]);
                maxTracks--;
            }
        }

        while (sj < gridSize)
        {
            if (!path.Contains(grid[si, sj]))
            {
                grid[si, sj].SetSearched(true);
                path.Add(grid[si, sj]);
            }
            sj++;
        }

        return path;
    }
}
