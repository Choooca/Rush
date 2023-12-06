using UnityEngine;

public class BubbleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;

    private float spawnTime;
    private float count;

    private void Start()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {
        count += Time.deltaTime;
        if(count > spawnTime) 
        {
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            count = 0;
            Instantiate(bubblePrefab, transform);
        }
    }
}
