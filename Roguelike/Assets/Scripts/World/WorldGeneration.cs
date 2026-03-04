using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject corner;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject obstacle;

    [Header("World Settings")]
    [SerializeField] private int worldLength = 10;
    [SerializeField] private int worldWidth = 10;
    [SerializeField] private float tileSize = 5f;
    [SerializeField] private float tileHeight = 1.5f;
    [SerializeField] private int obstaclesAmount = 6;
    
    private List<Vector3> availableFloorPositions = new List<Vector3>();
    private List<Vector3> usedEntityPosition = new List<Vector3>();
    private List<Vector3> usedObstaclePosition = new List<Vector3>();

    private GameObject spawnedPlayer;
    
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

    private void Start()
    {
        GenerateWorld();
        GenerateEntities();
        GenerateObstacles();
    }

    private void GenerateWorld()
    {
        Instantiate(corner, origin, cornerRotations[Corner.BottomLeft]);

        for (int x = 1; x < worldLength - 1; x++)
            Instantiate(wall, origin + new Vector3(tileSize * x, 0, 0), wallRotations[Wall.South]);

        Instantiate(corner, origin + new Vector3(tileSize * (worldLength - 1), 0, 0), cornerRotations[Corner.BottomRight]);

        for (int z = 1; z < worldWidth - 1; z++)
        {
            Instantiate(wall, origin + new Vector3(0, 0, tileSize * z), wallRotations[Wall.West]);

            for (int x = 1; x < worldLength - 1; x++)
            {
                Vector3 position = origin + new Vector3(tileSize * x, 0, tileSize * z);
                
                Instantiate(floor, origin + position, Quaternion.identity);
                
                availableFloorPositions.Add(position);
                
            }

            Instantiate(wall, origin + new Vector3(tileSize * (worldLength - 1), 0, tileSize * z), wallRotations[Wall.East]);
        }

        Instantiate(corner, origin + new Vector3(0, 0, tileSize * (worldWidth - 1)), cornerRotations[Corner.TopLeft]);

        for (int x = 1; x < worldLength - 1; x++)
            Instantiate(wall, origin + new Vector3(tileSize * x, 0, tileSize * (worldWidth - 1)), wallRotations[Wall.North]);

        Instantiate(corner, origin + new Vector3(tileSize * (worldLength - 1), 0, tileSize * (worldWidth - 1)), cornerRotations[Corner.TopRight]);
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
        int enemies = (worldLength + worldWidth) / 2;

        Vector3 origin = availableFloorPositions[0];
        
        origin.y += tileHeight;

        spawnedPlayer = Instantiate(player, origin, Quaternion.identity);
        
        for (int i = 1; i < enemies; i++)
        {
            Quaternion  rotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);
            
            GameObject spawnedEnemy = Instantiate(enemy, RandomGenerateEntities(), rotation);

            EnemyVision script = spawnedEnemy.GetComponent<EnemyVision>();
            script.SetPlayer(spawnedPlayer.transform);
        }
        
    }

    private Vector3 RandomGenerateObstacles()
    {
        Vector3 position = availableFloorPositions[Random.Range(0, availableFloorPositions.Count)];

        if (usedObstaclePosition.Contains(position) || usedEntityPosition.Contains(position)) return RandomGenerateObstacles();
        
        usedObstaclePosition.Add(position);

        return position;
    }
    
    private Vector3 RandomGenerateEntities()
    {
        Vector3 position = availableFloorPositions[Random.Range(0, availableFloorPositions.Count)];

        if (usedEntityPosition.Contains(position)) return RandomGenerateEntities();
        
        usedEntityPosition.Add(position);
        
        position.y += tileHeight;
        
        return position;
    }
};