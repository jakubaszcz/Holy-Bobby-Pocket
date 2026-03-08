using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class WorldGeneration : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject corner;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject collectible;
    [SerializeField] private GameObject zone;
    [SerializeField] private GameObject trap;

    [Header("World Settings")]
    [SerializeField] private int worldLength = 10;
    [SerializeField] private int worldWidth = 10;
    [SerializeField] private float tileSize = 5f;
    [SerializeField] private float tileHeight = 1f;
    [SerializeField] private float heightForTrapSpawn = 0.5f;
    [SerializeField] private float entitiesAmount; 
    [SerializeField] private int obstaclesAmount;
    [SerializeField] private int collectiblesAmount = 5;
    [SerializeField] private int trapsAmount = 10;
    
    private List<Vector3> availableFloorPositions = new List<Vector3>();
    private List<Vector3> usedPlayerPositions = new List<Vector3>();
    private List<Vector3> usedEntityPosition = new List<Vector3>();
    private List<Vector3> usedObstaclePosition = new List<Vector3>();
    private List<Vector3> usedCollectibleLocation = new List<Vector3>();
    private List<Vector3> usedTrapLocation = new List<Vector3>();
    private GameObject spawnedPlayer;
    
    [SerializeField] private NavMeshSurface navMeshSurface;

    
    enum Corner { TopLeft, TopRight, BottomLeft, BottomRight }
    enum Wall { North, East, South, West }

    Dictionary<Corner, Quaternion> cornerRotations = new Dictionary<Corner, Quaternion>();
    Dictionary<Wall, Quaternion> wallRotations = new Dictionary<Wall, Quaternion>();

    private Vector3 origin = Vector3.zero;

    private void Awake()
    {
        cornerRotations[Corner.TopLeft]     = Quaternion.Euler(0, 270, 0);
        cornerRotations[Corner.TopRight]    = Quaternion.Euler(0, 0,   0);
        cornerRotations[Corner.BottomLeft]  = Quaternion.Euler(0, 180, 0);
        cornerRotations[Corner.BottomRight] = Quaternion.Euler(0, 90,  0);

        wallRotations[Wall.North] = Quaternion.Euler(0, 0,   0);
        wallRotations[Wall.East]  = Quaternion.Euler(0, 90,  0);
        wallRotations[Wall.South] = Quaternion.Euler(0, 180, 0);
        wallRotations[Wall.West]  = Quaternion.Euler(0, 270, 0);
    }

    private void OnEnable()
    {
        GameSignals.OnEndGame += OnEndGame;
    }

    private void OnDisable()
    {
        GameSignals.OnEndGame -= OnEndGame;
    }

    private void OnEndGame(bool value)
    {
        if (value)
        {
            SpawnEndZone();
        }
    }

    private void SpawnEndZone()
    {
        Vector3 position = RandomGenerateCollectibles();

        if (usedObstaclePosition.Contains(position))
        {
            SpawnEndZone();
            return;
        }
        
        Instantiate(zone, position, Quaternion.identity);
    }

    private void Start()
    {
        RandomizeValues();
        
        GenerateWorld();
        GenerateObstacles();
        
        navMeshSurface.BuildNavMesh();
        
        GenerateEntities();
        GenerateCollectibles();
        GenerateTraps();
    }

    private void RandomizeValues()
    {
        worldLength = Random.Range(25, 35);
        worldWidth = Random.Range(25, 35);

        entitiesAmount = Random.Range((worldLength + worldWidth) / 2, worldLength + worldWidth);
        obstaclesAmount = Random.Range(worldLength + worldWidth, (worldLength * worldWidth) / 2);
        trapsAmount = Random.Range(worldLength + worldWidth, (worldLength * worldWidth) / 2);

		GameSignals.TriggerOnTotalCollectible(collectiblesAmount);
    }

    private void GenerateWorld()
    {
        Instantiate(corner, origin, cornerRotations[Corner.BottomLeft]);
        Instantiate(floor, origin + new Vector3(0, tileSize, 0), Quaternion.identity);

        for (int x = 1; x < worldLength - 1; x++)
        {
            Instantiate(wall, origin + new Vector3(tileSize * x, 0, 0), wallRotations[Wall.South]);
            Instantiate(floor, origin + new Vector3(tileSize * x, tileSize, 0), Quaternion.identity);
        }

        Instantiate(corner, origin + new Vector3(tileSize * (worldLength - 1), 0, 0), cornerRotations[Corner.BottomRight]);
        Instantiate(floor, origin + new Vector3(tileSize * (worldLength - 1), tileSize, 0), Quaternion.identity);

        for (int z = 1; z < worldWidth - 1; z++)
        {
            Instantiate(wall, origin + new Vector3(0, 0, tileSize * z), wallRotations[Wall.West]);
            Instantiate(floor, origin + new Vector3(0, tileSize, tileSize * z), Quaternion.identity);

            for (int x = 1; x < worldLength - 1; x++)
            {
                Vector3 position = origin + new Vector3(tileSize * x, 0, tileSize * z);
                Vector3 positionRoof = origin + new Vector3(tileSize * x, tileSize, tileSize * z);
                
                Instantiate(floor, origin + position, Quaternion.identity);
                Instantiate(floor, origin + positionRoof, Quaternion.identity);
                
                availableFloorPositions.Add(position);
                
            }

            Instantiate(wall, origin + new Vector3(tileSize * (worldLength - 1), 0, tileSize * z), wallRotations[Wall.East]);
            Instantiate(floor, origin + new Vector3(tileSize * (worldLength - 1), tileSize, tileSize * z), Quaternion.identity);
        }

        Instantiate(corner, origin + new Vector3(0, 0, tileSize * (worldWidth - 1)), cornerRotations[Corner.TopLeft]);
        Instantiate(floor, origin + new Vector3(0, tileSize, tileSize * (worldWidth - 1)), Quaternion.identity);

        for (int x = 1; x < worldLength - 1; x++)
        {
            Instantiate(wall, origin + new Vector3(tileSize * x, 0, tileSize * (worldWidth - 1)), wallRotations[Wall.North]);
            Instantiate(floor, origin + new Vector3(tileSize * x, tileSize, tileSize * (worldWidth - 1)), Quaternion.identity);
        }

        Instantiate(corner, origin + new Vector3(tileSize * (worldLength - 1), 0, tileSize * (worldWidth - 1)), cornerRotations[Corner.TopRight]);
        Instantiate(floor, origin + new Vector3(tileSize * (worldLength - 1), tileSize, tileSize * (worldWidth - 1)), Quaternion.identity);
    }

    private void GenerateObstacles()
    {
        for (int i = 0; i < obstaclesAmount; i++)
        {
            int orientation = Random.Range(0, 2);

            Quaternion rotation = Quaternion.Euler(0f, orientation * 90f, 0f);

            Instantiate(obstacle, RandomGenerateObstacles(), rotation);
        }
    }
    
    private void GenerateEntities()
    {
        Vector3 playerPos = availableFloorPositions[0];
        
        usedPlayerPositions.Add(playerPos);
        
        playerPos.y += tileHeight;

        spawnedPlayer = Instantiate(player, playerPos, Quaternion.identity);
        
        for (int i = 0; i < entitiesAmount; i++)
        {
            Quaternion  rotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);
            
            GameObject spawnedEnemy = Instantiate(enemy, RandomGenerateEntities(), rotation);

            EnemyVision scriptVision = spawnedEnemy.GetComponent<EnemyVision>();
            EnemyBehaviour scriptBehaviour = spawnedEnemy.GetComponent<EnemyBehaviour>();
            scriptVision.SetPlayer(spawnedPlayer.transform);
            scriptBehaviour.SetPlayer(spawnedPlayer.transform);
        }
        
    }

    private void GenerateCollectibles()
    {
        for (int i = 0; i < collectiblesAmount; i++)
        {
            GameObject obj = Instantiate(collectible,  RandomGenerateCollectibles(), Quaternion.identity);
        	obj.tag = "collectible";
		}
    }

    private void GenerateTraps()
    {
        for (int i = 0; i < trapsAmount; i++)
        {
            Instantiate(trap,  RandomGenerateTraps(), Quaternion.identity);
        }
    }
    
    private Vector3 RandomGenerateTraps()
    {
        Vector3 position = availableFloorPositions[Random.Range(0, availableFloorPositions.Count)];

        if (usedObstaclePosition.Contains(position) || usedEntityPosition.Contains(position) || usedTrapLocation.Contains(position) || usedPlayerPositions.Contains(position)) return RandomGenerateTraps();
        
        usedTrapLocation.Add(position);

        position.y += heightForTrapSpawn;
        
        return position;
    }

    private Vector3 RandomGenerateObstacles()
    {
        Vector3 position = availableFloorPositions[Random.Range(0, availableFloorPositions.Count)];

        if (usedObstaclePosition.Contains(position) || usedEntityPosition.Contains(position) || usedPlayerPositions.Contains(position)) return RandomGenerateObstacles();
        
        usedObstaclePosition.Add(position);

        return position;
    }
    
    private Vector3 RandomGenerateEntities()
    {
        Vector3 position = availableFloorPositions[Random.Range(0, availableFloorPositions.Count)];

        if (usedEntityPosition.Contains(position) || usedPlayerPositions.Contains(position)) return RandomGenerateEntities();
        
        usedEntityPosition.Add(position);
        
        position.y += tileHeight;
        
        return position;
    }

    private Vector3 RandomGenerateCollectibles()
    {
        Vector3 position = availableFloorPositions[Random.Range(0, availableFloorPositions.Count)];
        
        if (usedCollectibleLocation.Contains(position) || usedObstaclePosition.Contains(position) || usedEntityPosition.Contains(position) || usedPlayerPositions.Contains(position)) return RandomGenerateCollectibles();
        
        usedCollectibleLocation.Add(position);

        position.y += tileHeight;
        
        return position;
    }
};