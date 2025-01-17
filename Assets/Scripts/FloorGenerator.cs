using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject planePrefab;
    public Transform player;
    public int planeSize = 10;
    public int renderDistance = 5;

    private Vector3 lastPlayerPosition;
    private readonly HashSet<Vector2> generatedPlanes = new HashSet<Vector2>();

    void Start()
    {
        if (!planePrefab)
        {
            Debug.LogError("Assignez un prefab de plane dans l'inspecteur !");
            return;
        }

        lastPlayerPosition = player.position;
        GenerateInitialTerrain();
    }

    void Update()
    {
        if (player != null) {
            Vector3 movement = player.position - lastPlayerPosition;

            if (movement.magnitude >= planeSize)
            {
                GenerateTerrainAroundPlayer();
                lastPlayerPosition = player.position;
            }
        }
    }

    void GenerateInitialTerrain()
    {
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                GeneratePlaneAt(x, z);
            }
        }
    }

    void GenerateTerrainAroundPlayer()
    {
        int playerX = Mathf.FloorToInt(player.position.x / planeSize);
        int playerZ = Mathf.FloorToInt(player.position.z / planeSize);

        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                GeneratePlaneAt(playerX + x, playerZ + z);
            }
        }
    }

    void GeneratePlaneAt(int x, int z)
    {
        Vector2 planeCoord = new Vector2(x, z);

        if (!generatedPlanes.Contains(planeCoord))
        {
            Vector3 position = new Vector3(x * planeSize, 0, z * planeSize);
            Instantiate(planePrefab, position, Quaternion.identity);
            generatedPlanes.Add(planeCoord);
        }
    }
}
