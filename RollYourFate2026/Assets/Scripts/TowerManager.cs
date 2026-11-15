using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public GameObject[] towerPrefabs;
    [SerializeField] private int archerTowerCount;
    private GameObject archerTower;

    [SerializeField] private int magicTowerCount;
    private GameObject magicTower;

    [SerializeField] private int bombTowerCount;
    private GameObject bombTower;

    [SerializeField] private Transform center;
    [SerializeField] private float radius;


    private List<GameObject> Towers = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        archerTower = towerPrefabs[0];
        magicTower = towerPrefabs[1];
        bombTower = towerPrefabs[2];

        for (int i = 0; i < archerTowerCount; i++) Towers.Add(archerTower);
        for (int i = 0; i < magicTowerCount; i++) Towers.Add(magicTower);
        for (int i = 0; i < bombTowerCount; i++) Towers.Add(bombTower);

        // Randomizes Towers in List
        for (int i = Towers.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (Towers[i], Towers[j]) = (Towers[j], Towers[i]);
        }
        
        SpawnTowersInCircle(center.position, radius);

        for (int i = 0; i < archerTowerCount; i++) Towers.Add(archerTower);
        for (int i = 0; i < magicTowerCount; i++) Towers.Add(magicTower);
        for (int i = 0; i < bombTowerCount; i++) Towers.Add(bombTower);

        // Randomizes Towers in List
        for (int i = Towers.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (Towers[i], Towers[j]) = (Towers[j], Towers[i]);
        }
        SpawnTowersInCircleSpace(center.position, radius * 1.6f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTowersInCircle(Vector3 centerPos, float circleRadius)
    {
        int count = Towers.Count;
        if (count == 0) return;

        float angleStep = 2f * Mathf.PI / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * circleRadius;

            // Last argument parents each tower under this manager
            Instantiate(Towers[i], centerPos + offset, Quaternion.identity, transform);
        }
    }

    private void SpawnTowersInCircleSpace(Vector3 centerPos, float circleRadius)
    {
        bool done = false;
        int count = Towers.Count;
        if (count == 0) return;

        float angleStep = 2f * Mathf.PI / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * circleRadius;

            // Last argument parents each tower under this manager
            if (!done)
            {
                Instantiate(Towers[i], centerPos + offset, Quaternion.identity, transform);
                done = true;
            }
            else done = false;
            
        }
    }
}
