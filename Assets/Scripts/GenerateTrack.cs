using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTrack : MonoBehaviour
{
    public static List<Track> randomPath(Cell[,] grid, int si, int sj, int maxTracks)
    {
        int dir = -1;
        int gridSize = Mathf.RoundToInt(Mathf.Sqrt(grid.Length));

        List<Track> path = new List<Track>();
        path.Add(grid[si, sj].GetComponent<Track>());
        grid[si, sj].SetSearched(true);

        while (sj < gridSize - 1 && maxTracks > 0)
        {
            dir = Random.Range(0, 3);
            if (dir == 2 && si > 1 /* left */)
            {
                if (!grid[si - 1, sj].searched)
                    si--;
            }
            else if (dir == 0 /* up */)
            {
                if (!grid[si, sj + 1].searched)
                    sj++;
            }
            else if (/* dir == 1  && right */ si < gridSize - 1)
            {
                if (!grid[si + 1, sj].searched)
                    si++;
            }

            if (!path.Contains(grid[si, sj].GetComponent<Track>()))
            {
                grid[si, sj].SetSearched(true);
                path.Add(grid[si, sj].GetComponent<Track>());
                maxTracks--;
            }
        }

        while (sj < gridSize)
        {
            if (!path.Contains(grid[si, sj].GetComponent<Track>()))
            {
                grid[si, sj].SetSearched(true);
                path.Add(grid[si, sj].GetComponent<Track>());
            }
            sj++;
        }

        return path;
    }

}
