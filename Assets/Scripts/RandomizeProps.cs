using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeProps : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;
    void Start()
    {
        SpawnProps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnProps()
    {
        foreach(GameObject pSP in propSpawnPoints)
        {
            int rand = Random.Range(0, propSpawnPoints.Count);
            Instantiate(propPrefabs[rand], pSP.transform.position, Quaternion.identity);
        }
    }
}
