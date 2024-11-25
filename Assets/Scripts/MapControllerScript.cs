using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControllerScript : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrianPosition;
    public LayerMask terrainMask;
    PlayerMovementPhysic pm;

    void Start()
    {
        pm = player.GetComponent<PlayerMovementPhysic>();  // Assurez-vous d'avoir la référence du PlayerMovementPhysic
    }

    void Update()
    {
        ChunckChecker();
    }

    void ChunckChecker()
    {
        // Droite
        if (pm.moveDir.x > 0 && pm.moveDir.z == 0)
        {
            if (Physics.OverlapSphere(player.transform.position + new Vector3(20, 0, 0), checkerRadius, terrainMask).Length == 0)
            {
                noTerrianPosition = player.transform.position + new Vector3(20, 0, 0);
                SpawnChunk();
            }
        }
        // Gauche
        if (pm.moveDir.x < 0 && pm.moveDir.z == 0)
        {
            if (Physics.OverlapSphere(player.transform.position + new Vector3(-20, 0, 0), checkerRadius, terrainMask).Length == 0)
            {
                noTerrianPosition = player.transform.position + new Vector3(-20, 0, 0);
                SpawnChunk();
            }
        }
        // Haut
        if (pm.moveDir.x == 0 && pm.moveDir.z > 0)
        {
            if (Physics.OverlapSphere(player.transform.position + new Vector3(0, 0, 20), checkerRadius, terrainMask).Length == 0)
            {
                noTerrianPosition = player.transform.position + new Vector3(0, 0, 20);
                SpawnChunk();
            }
        }
        // Bas
        if (pm.moveDir.x == 0 && pm.moveDir.z < 0)
        {
            if (Physics.OverlapSphere(player.transform.position + new Vector3(0, 0, -20), checkerRadius, terrainMask).Length == 0)
            {
                noTerrianPosition = player.transform.position + new Vector3(0, 0, -20);
                SpawnChunk();
            }
        }

        // Haut-Droit
        if (pm.moveDir.x > 0 && pm.moveDir.z > 0)
        {
            if (Physics.OverlapSphere(player.transform.position + new Vector3(20, 0, 20), checkerRadius, terrainMask).Length == 0)
            {
                noTerrianPosition = player.transform.position + new Vector3(20, 0, 20);
                SpawnChunk();
            }
        }

        // Haut-Gauche
        if (pm.moveDir.x < 0 && pm.moveDir.z > 0)
        {
            if (Physics.OverlapSphere(player.transform.position + new Vector3(-20, 0, 20), checkerRadius, terrainMask).Length == 0)
            {
                noTerrianPosition = player.transform.position + new Vector3(-20, 0, 20);
                SpawnChunk();
            }
        }

        // Bas-Droit
        if (pm.moveDir.x > 0 && pm.moveDir.z < 0)
        {
            if (Physics.OverlapSphere(player.transform.position + new Vector3(20, 0, -20), checkerRadius, terrainMask).Length == 0)
            {
                noTerrianPosition = player.transform.position + new Vector3(20, 0, -20);
                SpawnChunk();
            }
        }

        // Bas-Gauche
        if (pm.moveDir.x < 0 && pm.moveDir.z < 0)
        {
            if (Physics.OverlapSphere(player.transform.position + new Vector3(-20, 0, -20), checkerRadius, terrainMask).Length == 0)
            {
                noTerrianPosition = player.transform.position + new Vector3(-20, 0, -20);
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        int rand = UnityEngine.Random.Range(0, terrainChunks.Count);
        Instantiate(terrainChunks[rand], noTerrianPosition, Quaternion.identity);
    }
}
