//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MapControllerScript : MonoBehaviour
//{
//    public List<GameObject> terrainChunks;
//    public GameObject player;
//    public float checkerRadius;
//    Vector3 noTerrianPosition;
//    public LayerMask terrainMask;
//    PlayerMovementPhysic pm;
//    void Start()
//    {
       
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        ChunckChecker();
//    }

//    void ChunckChecker()
//    {
//        // Right
//        if(pm.moveDir.x > 0 && pm.moveDir.y == 0)
//        {
//            if(!Physics2D.OverlapCircle(player.transform.position + new Vector3(20,0,0), checkerRadius, terrainMask))
//            {
//                noTerrianPosition = player.transform.position + new Vector3(20,0,0);
//                SpawnChunk();
//            }
//        }
//        // Left
//        if(pm.moveDir.x < 0 && pm.moveDir.y == 0)
//        {
//            if(!Physics2D.OverlapCircle(player.transform.position + new Vector3(-20,0,0), checkerRadius, terrainMask))
//            {
//                noTerrianPosition = player.transform.position + new Vector3(-20,0,0);
//                SpawnChunk();
//            }
//        }
//        // Up
//        if(pm.moveDir.x == 0 && pm.moveDir.y > 0)
//        {
//            if(!Physics2D.OverlapCircle(player.transform.position + new Vector3(0,20,0), checkerRadius, terrainMask))
//            {
//                noTerrianPosition = player.transform.position + new Vector3(0,20,0);
//                SpawnChunk();
//            }
//        } 
//        // Down
//        if(pm.moveDir.x == 0 && pm.moveDir.y < 0)
//        {
//            if(!Physics2D.OverlapCircle(player.transform.position + new Vector3(0,-20,0), checkerRadius, terrainMask))
//            {
//                noTerrianPosition = player.transform.position + new Vector3(0,-20,0);
//                SpawnChunk();
//            }
//        }
//        // Right Up
//        if(pm.moveDir.x > 0 && pm.moveDir.y > 0)
//        {
//            if(!Physics2D.OverlapCircle(player.transform.position + new Vector3(20,20,0), checkerRadius, terrainMask))
//            {
//                noTerrianPosition = player.transform.position + new Vector3(20,20,0);
//                SpawnChunk();
//            }
//        }
//        // Left Up
//        if(pm.moveDir.x < 0 && pm.moveDir.y > 0)
//        {
//            if(!Physics2D.OverlapCircle(player.transform.position + new Vector3(-20,20,0), checkerRadius, terrainMask))
//            {
//                noTerrianPosition = player.transform.position + new Vector3(-20,20,0);
//                SpawnChunk();
//            }
//        }
//        // Left Down
//        if(pm.moveDir.x < 0 && pm.moveDir.y < 0)
//        {
//            if(!Physics2D.OverlapCircle(player.transform.position + new Vector3(-20,-20,0), checkerRadius, terrainMask))
//            {
//                noTerrianPosition = player.transform.position + new Vector3(-20,-20,0);
//                SpawnChunk();
//            }
//        }
//        // Right Down
//        if(pm.moveDir.x > 0 && pm.moveDir.y < 0)
//        {
//            if(!Physics2D.OverlapCircle(player.transform.position + new Vector3(20,-20,0), checkerRadius, terrainMask))
//            {
//                noTerrianPosition = player.transform.position + new Vector3(20,-20,0);
//                SpawnChunk();
//            }
//        }
//    }

//    void SpawnChunk()
//    {
//        int rand = Random.Range(0, terrainChunks.Count);
//        Instantiate(terrainChunks[rand],noTerrianPosition,Quaternion.identity);
//    }
//}
