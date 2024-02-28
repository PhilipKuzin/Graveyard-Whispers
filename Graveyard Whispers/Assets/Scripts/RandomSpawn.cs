using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject enemy;
    public GameObject grave;
    public List<Transform> spawnPoints;
    private List<Transform> tempSpawnPoints;
    private Vector3 _spawnPositionShift = new Vector3 (0f, 1.0f, 0f);


    private float timeBtwSpawn = 5f;
    private float timer = 0f;

    private void Start()
    {
        tempSpawnPoints = new List<Transform>(spawnPoints);
    }

    private void Update()
    {
        if (MainCharacter.Instance == null)
            gameObject.SetActive(false);

        if (tempSpawnPoints.Count > 0)
        {
            timer += Time.deltaTime;
            if (timer >= timeBtwSpawn)
            {
                timer = 0f;

                int spawnIndex = Random.Range(0, tempSpawnPoints.Count);
                Transform spawnPoint = tempSpawnPoints[spawnIndex];
                
                Instantiate(enemy, spawnPoint.position + _spawnPositionShift, Quaternion.identity);
                Instantiate(grave, spawnPoint.position, Quaternion.identity);
                tempSpawnPoints.RemoveAt(spawnIndex);
            }
        }
    }
}