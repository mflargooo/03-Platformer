using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Grandma : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private int maxHealth;
    [SerializeField] private float timeUntilBossStart;

    [SerializeField] private Transform[] oniSpawnpoints;
    [SerializeField] private int minOniSpawn;
    [SerializeField] private int maxOniSpawn;

    [SerializeField] private Oni oniPrefab;
    [SerializeField] private List<Oni> spawnedOni;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        healthBar.gameObject.SetActive(false);
    }

    public void StartBoss()
    {
        StartCoroutine(SpawnOni());
        healthBar.gameObject.SetActive(true);
    }

    IEnumerator SpawnOni()
    {
        yield return new WaitForSeconds(timeUntilBossStart);
        int num = Random.Range(minOniSpawn, maxOniSpawn + 1);
        for (int i = 0; i < num; i++)
        {
            Transform spawnpoint = oniSpawnpoints[Random.Range(0, oniSpawnpoints.Length)];
            Oni oni = Instantiate(oniPrefab, spawnpoint.position, oniPrefab.transform.rotation);
            oni.SetChaseRange(20);
            spawnedOni.Add(oni);
            yield return new WaitForSeconds(.5f);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        healthBar.value -= damage;

        if(healthBar.value == 0)
        {
            for(int i = 0; i < spawnedOni.Count; i++)
            {
                Destroy(spawnedOni[i].gameObject);
            }
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
