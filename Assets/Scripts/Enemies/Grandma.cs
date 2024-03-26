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
        StartCoroutine(BossLogic());
    }

    IEnumerator BossLogic()
    {
        yield return new WaitForSeconds(timeUntilBossStart);
        healthBar.gameObject.SetActive(true);
        while (true)
        {
            yield return null;
            if (spawnedOni.Count > 0)
            {
                for (int i = 0; i < spawnedOni.Count; i++)
                {
                    if (!spawnedOni[i]) spawnedOni.RemoveAt(i);
                    i--;
                    yield return null;
                }
                yield return null;
                continue;
            }
            yield return new WaitForSeconds(.5f);

            int num = Random.Range(minOniSpawn, maxOniSpawn + 1);
            for (int i = 0; i < num; i++)
            {
                Transform spawnpoint = oniSpawnpoints[Random.Range(0, oniSpawnpoints.Length)];
                Oni oni = Instantiate(oniPrefab, spawnpoint.position, oniPrefab.transform.rotation);
                spawnedOni.Add(oni);
                yield return new WaitForSeconds(1f);
            }
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
                if(spawnedOni[i])
                    Destroy(spawnedOni[i].gameObject);
            }
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
