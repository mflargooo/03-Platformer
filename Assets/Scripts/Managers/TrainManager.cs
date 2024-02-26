using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    Cell[,] grid;
    [SerializeField] private Material tmp;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int maxTracks;
    // Start is called before the first frame update
    void Start()
    {
        GridManager gridManager = FindObjectOfType<GridManager>();
        grid = GridManager.GetGrid();
        float tileSpacing = gridManager.GetTileSpacing();
        float tileSize = gridManager.GetTileSize();
        int gridSize = Mathf.RoundToInt(Mathf.Sqrt(grid.Length));

        foreach (Cell c in grid)
        {
            c.SetWeight(Random.Range(0f, 20f));
        }

        List<Cell> path = GenerateTrack.randomPath(grid, Random.Range(2, gridSize - 2), 0, maxTracks);
        Cell src = Instantiate(cellPrefab, path[0].transform.position - Vector3.forward * (tileSpacing + tileSize), cellPrefab.transform.rotation).GetComponent<Cell>();
        path.Insert(0, src);

        GameObject pathParent = new GameObject();
        pathParent.name = "Path";
        pathParent.transform.parent = transform;
        for (int i = 0; i < path.Count; i++)
        {
            path[i].name = "Path: " + i.ToString();
            path[i].transform.position += Vector3.up;
            path[i].transform.parent = pathParent.transform;
            path[i].GetComponent<MeshRenderer>().material = tmp;
        }
    }
}
