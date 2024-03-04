using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    Cell[,] grid;
    [SerializeField] private Material tmp;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int maxTracks;
    [SerializeField] private float randDestroyTrackPercent;

    /* 0 is left, 1 is right, 2 is left */
    [SerializeField] private Material[] tracks;
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

        List<Track> path = GenerateTrack.randomPath(grid, Random.Range(2, gridSize - 2), 0, maxTracks);
        
        Track src = Instantiate(cellPrefab, path[0].transform.position - Vector3.forward * (tileSpacing + tileSize), cellPrefab.transform.rotation).GetComponent<Track>();
        
        src.SetPieceType(1);
        src.SetCorrectRotation(0);
        src.IsRotatedCorrectly();

        path.Insert(0, src);

        GameObject pathParent = new GameObject();
        pathParent.name = "Path";
        pathParent.transform.parent = transform;
        for (int i = 0; i < path.Count; i++)
        {
            path[i].name = "Path: " + i.ToString();
            path[i].transform.parent = pathParent.transform;
            path[i].GetComponent<MeshRenderer>().material = tmp;
        }

        foreach (Cell c in grid)
        {
            if (!path.Contains(c.GetComponent<Track>()) && Random.Range(0f, 1f) < randDestroyTrackPercent)
            {
                Destroy(c.gameObject);
            }

            c.GetComponent<MeshRenderer>().material = tracks[Random.Range(0, 3)];
            c.transform.eulerAngles = new Vector3(90, Random.Range(0, 4) * 90, transform.eulerAngles.z);
        }

        for (int i = 1; i < path.Count - 1; i++)
        {
            Track prev = path[i - 1];
            Track curr = path[i];
            Track next = path[i + 1];

            Vector3 prevToCurr = curr.transform.position - prev.transform.position;
            Vector3 currToNext = next.transform.position - curr.transform.position;

            int delAngle = (int) Vector3.SignedAngle(prevToCurr, currToNext, Vector3.up);
            int type = (delAngle + 90) / 90;
            curr.SetPieceType(type);
            next.SetCorrectRotation(Mathf.RoundToInt(curr.correctRotation + delAngle + 360) % 360);

            curr.GetComponent<MeshRenderer>().material = tracks[type];

            next.transform.eulerAngles = new Vector3(90, Random.Range(0, 4) * 90, transform.eulerAngles.z);
            curr.IsRotatedCorrectly();
        }

        path[path.Count - 1].SetPieceType(1);
        path[path.Count - 1].GetComponent<MeshRenderer>().material = tracks[1];
        path[path.Count - 1].IsRotatedCorrectly();
    }
}
