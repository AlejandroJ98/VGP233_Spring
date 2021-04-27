using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    public float spawnPosZ = 20.0f;
    private float xRange = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            int animalIndex = Random.Range(0 , 3);
            Vector3 spawnPos = new Vector3(Random.Range(-xRange, xRange), 0.0f, spawnPosZ);
            Instantiate(animalPrefabs[animalIndex], new Vector3(0, 0, spawnPosZ), animalPrefabs[0].transform.rotation);
        }
    }
}
