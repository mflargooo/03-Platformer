using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [Min(min: 1)]
    [SerializeField] private int gridSize;
    [SerializeField] private float tileSize;
    [SerializeField] private float tileSpacing;

    private static Cell[,] grid;
    // Start is called before the first frame update
    void Awake()
    {
        grid = new Cell[gridSize, gridSize];
        GameObject gridHolder = new GameObject();
        gridHolder.transform.parent = transform;
        gridHolder.name = "Grid";

        float originOffset = ((gridSize - 1) * (tileSize + tileSpacing)) / 2;
        for (int c = 0; c < gridSize; c++)
        {
            GameObject col = new GameObject();
            col.name = c.ToString();
            col.transform.parent = gridHolder.transform;
            for (int r = 0; r < gridSize; r++)
            {
                Cell cell = Instantiate(cellPrefab, transform.position - new Vector3(1f, 0f, 1f) * originOffset + new Vector3(c, 0f, r) * (tileSize + tileSpacing), cellPrefab.transform.rotation).GetComponent<Cell>();
                grid[c, r] = cell;

                cell.transform.parent = col.transform;
                cell.name = r.ToString();
            }
        }
    }

    public static Cell[,] GetGrid()
    {
        return grid;
    }
}
