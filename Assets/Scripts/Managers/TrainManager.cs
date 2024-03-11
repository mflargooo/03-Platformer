using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainManager : MonoBehaviour
{
    Cell[,] grid;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int maxTracks;
    [SerializeField] private float randDestroyTrackPercent;
    [SerializeField] private float completeTime;
    private float timer;

    [SerializeField] private TMP_Text timerText;
    private bool ended;
    private bool cabooseFailed;

    /* 0 is left, 1 is right, 2 is left */
    [SerializeField] private Material[] tracks;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject caboosePrefab;
    [SerializeField] private float cabooseSpeed;

    private List<Track> path;
    private List<GameObject> cabooses = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        timer = completeTime + .999f;
        GridManager gridManager = FindObjectOfType<GridManager>();
        grid = GridManager.GetGrid();
        float tileSpacing = gridManager.GetTileSpacing();
        float tileSize = gridManager.GetTileSize();
        int gridSize = Mathf.RoundToInt(Mathf.Sqrt(grid.Length));

        foreach (Cell c in grid)
        {
            c.SetWeight(Random.Range(0f, 20f));
        }

        path = GenerateTrack.randomPath(grid, Random.Range(2, gridSize - 2), 0, maxTracks);
        
        Track src = Instantiate(cellPrefab, path[0].transform.position - Vector3.forward * (tileSpacing + tileSize), cellPrefab.transform.rotation).GetComponent<Track>();
        Track dst = Instantiate(cellPrefab, path[path.Count-1].transform.position + Vector3.forward * (tileSpacing + tileSize), cellPrefab.transform.rotation).GetComponent<Track>();
        
        src.SetPieceType(1);
        src.GetComponent<MeshRenderer>().material = tracks[1];
        src.SetCorrectRotation(0);
        src.IsRotatedCorrectly();
        src.Lock();

        path.Insert(0, src);
        path.Add(dst);

        GameObject pathParent = new GameObject();
        pathParent.name = "Path";
        pathParent.transform.parent = transform;
        for (int i = 0; i < path.Count; i++)
        {
            path[i].name = "Path: " + i.ToString();
            path[i].transform.parent = pathParent.transform;
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

        dst.SetPieceType(1);
        dst.GetComponent<MeshRenderer>().material = tracks[1];
        dst.SetCorrectRotation(0);
        dst.IsRotatedCorrectly();
        dst.transform.eulerAngles = new Vector3(90, 0, transform.eulerAngles.z);
        dst.Lock();
    }

    private void Update()
    {
        if (timer >= 1f)
        {
            timer -= Time.deltaTime;
            timerText.text = ((int)timer).ToString();
        }
        else if (!ended)
        {
            ended = true;
            timerText.text = ((int)timer).ToString();
            Destroy(player);
            StartCoroutine(SpawnCaboose());
        }
    }

    private IEnumerator SpawnCaboose()
    {
        for (int i = 0; i < 3 && !cabooseFailed; i++)
        {
            GameObject caboose = Instantiate(caboosePrefab, path[0].transform.position + Vector3.up * .01f - Vector3.forward * 1f, caboosePrefab.transform.rotation);
            cabooses.Add(caboose);
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
}
