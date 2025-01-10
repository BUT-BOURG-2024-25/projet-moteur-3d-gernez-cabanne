using UnityEngine;
using System.Collections.Generic;

public class ChunkLoader : MonoBehaviour
{
    [Header("Chunk Settings")]
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private float chunkSize = 10f;
    [SerializeField] private int visibleChunks = 3;  // Nombre de chunks visibles à tout moment
    [SerializeField] private float generationDistance = 30f;  // Distance entre la génération de chaque chunk

    private Transform playerTransform;
    private Queue<GameObject> activeChunks;  // Liste des chunks actuellement actifs
    private Vector3 lastPlayerPosition;  // Dernière position du joueur pour générer un chunk
    private float lastGenerationTime;  // Pour limiter la fréquence des générations

    private void Start()
    {
        playerTransform = Camera.main.transform;
        activeChunks = new Queue<GameObject>();

        lastPlayerPosition = playerTransform.position;
        lastGenerationTime = Time.time;

        GenerateInitialChunks();
    }

    private void Update()
    {
        // Vérifier si le joueur a parcouru la distance spécifiée pour générer un nouveau chunk
        if (Vector3.Distance(playerTransform.position, lastPlayerPosition) >= generationDistance)
        {
            lastPlayerPosition = playerTransform.position;

            // Générer un nouveau chunk devant le joueur
            GenerateNextChunk();
        }

        // Supprimer les chunks qui sont trop loin derrière le joueur
        //RemoveDistantChunks();
    }


    private void GenerateInitialChunks()
    {
        Vector3 startPos = playerTransform.position;

        Vector3 spawnPosition = new Vector3(startPos.x, 0, startPos.z);
        GameObject chunk = Instantiate(chunkPrefab, spawnPosition, Quaternion.identity);

        activeChunks.Enqueue(chunk);
    }


    private void GenerateNextChunk()
    {
        // Générer un chunk devant le joueur
        Vector3 spawnPosition = activeChunks.Peek().transform.position + playerTransform.forward * chunkSize;
        GameObject newChunk = Instantiate(chunkPrefab, spawnPosition, Quaternion.identity);

        // Ajouter le nouveau chunk à la liste des chunks actifs
        activeChunks.Enqueue(newChunk);

        // Supprimer le chunk le plus éloigné (derrière le joueur)
        GameObject oldChunk = activeChunks.Dequeue();
        Destroy(oldChunk);
    }

    private void RemoveDistantChunks()
    {
        // Supprimer les chunks qui sont trop loin derrière le joueur
        List<GameObject> chunksToRemove = new List<GameObject>();

        foreach (GameObject chunk in activeChunks)
        {
            if (Vector3.Distance(chunk.transform.position, playerTransform.position) > visibleChunks * chunkSize)
            {
                chunksToRemove.Add(chunk);
            }
        }

        foreach (GameObject chunk in chunksToRemove)
        {
            activeChunks.Dequeue();
            Destroy(chunk);
        }
    }
}
