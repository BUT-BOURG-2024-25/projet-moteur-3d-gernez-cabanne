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


    void SpawnProps()
    {
        foreach(GameObject pSP in propSpawnPoints)
        {
            int rand = Random.Range(0, propPrefabs.Count); 
            Instantiate(propPrefabs[rand], pSP.transform.position, Quaternion.identity);
        }
    }


}