using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject planePrefab; // Référence au prefab de plane
    public Transform player;      // Référence au joueur
    public int planeSize = 10;    // Taille d'un plane (assurez-vous que le prefab a cette taille)
    public int renderDistance = 5; // Nombre de planes à générer autour du joueur

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
        Vector3 movement = player.position - lastPlayerPosition;

        if (movement.magnitude >= planeSize) // Crée de nouveaux planes si le joueur avance d'une taille de plane
        {
            GenerateTerrainAroundPlayer();
            lastPlayerPosition = player.position;
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

        if (!generatedPlanes.Contains(planeCoord)) // Vérifie si le plane est déjà généré
        {
            Vector3 position = new Vector3(x * planeSize, 0, z * planeSize);
            Instantiate(planePrefab, position, Quaternion.identity);
            generatedPlanes.Add(planeCoord);
        }
    }
}
