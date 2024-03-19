using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainManager : MonoBehaviour
{
    Cell[,] grid;
    [Min(min: 1)]
    [SerializeField] private int puzzleSize;
    [SerializeField] private bool randomlyGenerate;
    [SerializeField] private string[] puzzleCodes;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int maxTracks;
    [SerializeField] private float randDestroyTrackPercent;
    [SerializeField] private float completeTime;
    private float timer;

    [SerializeField] private TMP_Text timerText;
    private bool cabooseFailed;

    /* 0 is left, 1 is right, 2 is left */
    [SerializeField] private Material[] tracks;
    [SerializeField] private GameObject caboosePrefab;
    [SerializeField] private float cabooseSpeed;

    GameObject pathParent;
    private List<Track> path;
    private GameObject[] cabooses = new GameObject[3];

    [SerializeField] GridManager gridManager;
    float tileSpacing;
    float tileSize;

    void Setup()
    {
        timer = completeTime + .999f;
        
        tileSpacing = gridManager.GetTileSpacing();
        tileSize = gridManager.GetTileSize();

        if (randomlyGenerate)
        {
            grid = gridManager.SpawnGrid(puzzleSize);
            path = GenerateTrack.randomPath(grid, Random.Range(2, puzzleSize - 2), 0, maxTracks);

            /* Randomly give all cells a rotation and type */
            foreach (Cell c in grid)
            {
                if (path.Contains(c.GetComponent<Track>())) continue;
                int pieceType = Random.Range(0, 3);
                c.GetComponent<Track>().SetPieceType(pieceType);
                c.GetComponent<MeshRenderer>().material = tracks[pieceType];
                c.transform.eulerAngles = new Vector3(90, Random.Range(0, 4) * 90, transform.eulerAngles.z);
            }

            /* Create and add src and dst to path */
            Track src = Instantiate(cellPrefab, path[0].transform.position - Vector3.forward * (tileSpacing + tileSize), cellPrefab.transform.rotation).GetComponent<Track>();
            Track dst = Instantiate(cellPrefab, path[path.Count - 1].transform.position + Vector3.forward * (tileSpacing + tileSize), cellPrefab.transform.rotation).GetComponent<Track>();
            path.Insert(0, src);
            path.Add(dst);

            /* Setup source track */
            src.SetPieceType(1);
            src.GetComponent<MeshRenderer>().material = tracks[1];
            src.SetCorrectRotation(0);
            src.IsRotatedCorrectly();
            src.Lock();

            /* Setup destination track */
            dst.SetPieceType(1);
            dst.GetComponent<MeshRenderer>().material = tracks[1];
            dst.SetCorrectRotation(0);
            dst.IsRotatedCorrectly();
            dst.Lock();

            /* Choose and set path */
            for (int i = 1; i < path.Count - 1; i++)
            {
                Track prev = path[i - 1];
                Track curr = path[i];
                Track next = path[i + 1];

                Vector3 prevToCurr = curr.transform.position - prev.transform.position;
                Vector3 currToNext = next.transform.position - curr.transform.position;

                int delAngle = (int)Vector3.SignedAngle(prevToCurr, currToNext, Vector3.up);
                int type = (delAngle + 90) / 90;
                curr.SetPieceType(type);
                int correctRotation = Mathf.RoundToInt(curr.correctRotation + delAngle + 360) % 360;
                next.SetCorrectRotation(correctRotation);
                next.transform.eulerAngles = new Vector3(90, correctRotation, transform.eulerAngles.z);

                curr.GetComponent<MeshRenderer>().material = tracks[type];
            }

            /* Collect path into children of parent */
            pathParent = new GameObject();
            pathParent.name = "Path";
            pathParent.transform.parent = transform;
            for (int i = 0; i < path.Count; i++)
            {
                path[i].name = "PATH: " + i.ToString();
                path[i].transform.parent = pathParent.transform;
            }

            Debug.Log(EncodeGrid(grid, path));
        }
        else
        {
            (grid, path) = DecodeGrid(puzzleCodes[Random.Range(0, puzzleCodes.Length)]);

            /* Create and add src and dst to path */
            Track src = Instantiate(cellPrefab, path[0].transform.position - Vector3.forward * (tileSpacing + tileSize), cellPrefab.transform.rotation).GetComponent<Track>();
            Track dst = Instantiate(cellPrefab, path[path.Count - 1].transform.position + Vector3.forward * (tileSpacing + tileSize), cellPrefab.transform.rotation).GetComponent<Track>();
            path.Insert(0, src);
            path.Add(dst);

            /* Setup source track */
            src.SetPieceType(1);
            src.GetComponent<MeshRenderer>().material = tracks[1];
            src.SetCorrectRotation(0);
            src.IsRotatedCorrectly();
            src.Lock();

            /* Setup destination track */
            dst.SetPieceType(1);
            dst.GetComponent<MeshRenderer>().material = tracks[1];
            dst.SetCorrectRotation(0);
            dst.IsRotatedCorrectly();
            dst.transform.eulerAngles = new Vector3(90, 0, transform.eulerAngles.z);
            dst.Lock();

            /* Collect path into children of parent */
            GameObject pathParent = new GameObject();
            pathParent.name = "Path";
            pathParent.transform.parent = transform;
            for (int i = 0; i < path.Count; i++)
            {
                path[i].name = "PATH: " + i.ToString();
                path[i].transform.parent = pathParent.transform;
            }
        }

        if (grid == null)
        {
            Debug.Log("ERROR IN CREATING GRID");
            return;
        }

        /* Randomize correct path rotations */
        for (int i = 1; i < path.Count - 1; i++)
        {
            path[i].transform.eulerAngles = new Vector3(90, Random.Range(0, 4) * 90, transform.eulerAngles.z);
            path[i].IsRotatedCorrectly();
        }

        foreach (Cell c in grid)
        {
            if (path.Contains(c.GetComponent<Track>())) continue;

            if (Random.Range(0f, 1f) < randDestroyTrackPercent)
            {
                Destroy(c.gameObject);
            }
        }
    }

    public IEnumerator RestartPuzzle()
    {
        if (gridManager.GetGridRoot()) Destroy(gridManager.GetGridRoot());
        if (pathParent) Destroy(pathParent);
        path = new List<Track>();

        Setup();

        while (timer >= 1f)
        {
            timer -= Time.deltaTime;
            timerText.text = ((int)timer).ToString();
            yield return null;
        }
        foreach (Cell c in grid)
        {
            if (c)
            {
                c.GetComponent<Track>().Lock();
            }
        }
        timerText.text = ((int)timer).ToString();
        StartCoroutine(SpawnCaboose());
    }

    private IEnumerator SpawnCaboose()
    {
        for (int i = 0; i < 3 && !cabooseFailed; i++)
        {
            GameObject caboose = Instantiate(caboosePrefab, path[0].transform.position + Vector3.up * .01f - Vector3.forward * 1f, caboosePrefab.transform.rotation);
            cabooses[i] = caboose;
            caboose.transform.parent = transform;
            caboose.name = "Caboose " + i.ToString();
            StartCoroutine(Caboose(caboose));
            yield return new WaitForSeconds(1f / cabooseSpeed);
        }
    }

    private IEnumerator Caboose(GameObject caboose)
    {
        Rigidbody rb = caboose.GetComponent<Rigidbody>();
        int i = 0;
        while (i < path.Count && !cabooseFailed)
        {
            Track target = path[i];
            cabooseFailed = !target.IsRotatedCorrectly();

            Vector3 toTarget = target.transform.position + Vector3.up * .01f - caboose.transform.position;
            while (toTarget.magnitude > .1f && !cabooseFailed)
            {
                rb.velocity = toTarget.normalized * cabooseSpeed;
                yield return null;
                toTarget = target.transform.position + Vector3.up * .01f - caboose.transform.position;
            }
            i++;
        }

        if (i < path.Count)
        {
            rb.velocity = Vector3.zero;
            Debug.Log(caboose.name + " ended with Fail");
        }
        else Debug.Log(caboose.name + " ended with Success");
    }

    void SetTrackInfo (Track track, char cellType)
    {
        int pieceType = 0;
        int correctRotation = 0;

        switch (cellType)
        {
            case '0':
                pieceType = 0;
                correctRotation = 0;
                break;
            case '1':
                pieceType = 0;
                correctRotation = 90;
                break;
            case '2':
                pieceType = 0;
                correctRotation = 180;
                break;
            case '3':
                pieceType = 0;
                correctRotation = 270;
                break;
            case '4':
                pieceType = 1;
                correctRotation = 0;
                break;
            case '5':
                pieceType = 1;
                correctRotation = 90;
                break;
            case '6':
                pieceType = 2;
                correctRotation = 0;
                break;
            case '7':
                pieceType = 2;
                correctRotation = 90;
                break;
            case '8':
                pieceType = 2;
                correctRotation = 180;
                break;
            case '9':
                pieceType = 2;
                correctRotation = 270;
                break;
        }

        track.SetPieceType(pieceType);
        track.SetCorrectRotation(correctRotation);
        track.GetComponent<MeshRenderer>().material = tracks[pieceType];
        track.transform.eulerAngles = new Vector3(90, correctRotation, transform.eulerAngles.z);
    }

    //TILE TYPE, 0, 1, 2, 3 is left turn with rotation 0, 90, 180, and -90
    //           4, 5       is straight with rotation 0, 90
    //           6, 7, 8, 9 is right turn with rotation 0, 90, 180, and -90
    // <BOARDSIZE>,<PCOUNT>,<TOPLEFT-->BOTTOMRIGHT TILE TYPE = 0-9, if in path precede with (i = index in path) ex>*
    //ex 
    //          *******
    //          *  |  *
    //          *  |  *
    //    *******************
    //    * ___ *  |  * ___ *
    //    *     *  |  *     *
    //    *******************
    //    *  __ * |__ * __  *
    //    *  |  *     *  |  *
    //    *******************
    //    *  __ * ___ * __| *
    //    *  |  *     *     *
    //    *******************
    //    *  |  *
    //    *  |  *
    //    *******
    // = 3,3,0,1,5(6)456(5)7(4)0(1)6(2)5(3)3 (INCORRECT, TRANSPOSE THIS)
    // then once path is generated, randomize all rotations.
    private string EncodeGrid(Cell[,] grid, List<Track> path)
    {
        int boardSize = Mathf.RoundToInt(Mathf.Sqrt(grid.Length));

        string boardInfo = "";

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                string pathPrefix = "";

                Track cell = grid[i, j].GetComponent<Track>();
                int type = cell.pieceType;
                int rotation = ((int) cell.gameObject.transform.eulerAngles.y + 360) % 360;

                if (path.Contains(cell))
                {
                    pathPrefix = "(" + cell.name.Substring(6) + ")";
                }

                switch (type, rotation)
                {
                    case (0, 0):
                        boardInfo += pathPrefix + '0';
                        break;
                    case (0, 90):
                        boardInfo += pathPrefix + '1';
                        break;
                    case (0, 180):
                        boardInfo += pathPrefix + '2';
                        break;
                    case (0, 270):
                        boardInfo += pathPrefix + '3';
                        break;
                    case (1, 0):
                        boardInfo += pathPrefix + '4';
                        break;
                    case (1, 90):
                        boardInfo += pathPrefix + '5';
                        break;
                    case (1, 180):
                        boardInfo += pathPrefix + '4';
                        break;
                    case (1, 270):
                        boardInfo += pathPrefix + '5';
                        break;
                    case (2, 0):
                        boardInfo += pathPrefix + '6';
                        break;
                    case (2, 90):
                        boardInfo += pathPrefix + '7';
                        break;
                    case (2, 180):
                        boardInfo += pathPrefix + '8';
                        break;
                    case (2, 270):
                        boardInfo += pathPrefix + '9';
                        break;
                }
            }
        }

        return boardSize.ToString() + "," + (path.Count - 2).ToString() + "," + boardInfo;
    }

    private (Cell[,], List<Track>) DecodeGrid(string code)
    {
        string[] split = code.Split(",");
        int gridSize = int.Parse(split[0]);
        int pathSize = int.Parse(split[1]);
        string pathString = split[2];
        Cell[,] grid = gridManager.SpawnGrid(gridSize);
        Track[] pathArray = new Track[pathSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Track cell = grid[i, j].GetComponent<Track>();

                char cellType = pathString[0];
                if(cellType == '(')
                {
                    int parenEnd = pathString.IndexOf(')');
                    int pathIdx = int.Parse(pathString.Substring(1, parenEnd - 1)) - 1;
                    cellType = pathString[parenEnd + 1];

                    pathArray[pathIdx] = cell.GetComponent<Track>();

                    pathString = pathString.Substring(parenEnd + 2);
                }
                else
                {
                    pathString = pathString.Substring(1);
                }

                SetTrackInfo(cell, cellType);
            }
        }

        List<Track> path = new List<Track>();
        for (int i = 0; i < pathSize; i++)
        {
            path.Add(pathArray[i]);
        }

        return (grid, path);
    }
}
