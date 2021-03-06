using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TerrainGenerator : MonoBehaviour
{
    // Array to keep track of where tiles should be placed
    int[,] mapArray;

    // Tilemap we use
    Tilemap tilemap;

    // Base tile for the tilemap (should be a rule tile)
    public BetterRuleTile baseTile;

    // Stone tile for lower elevation
    public BetterRuleTile stoneTile;

    // Gold and diamond tiles
    public BetterRuleTile goldTile;
    public BetterRuleTile diamondTile;

    // Sets width and height of generated terrain, and interval for Perlin noise function (lower interval means more randomness)
    public int terrainWidth, terrainHeight, perlinInterval;

    // Tank prefabs to instantiate players
    public GameObject Tank1Prefab;
    public GameObject Tank2Prefab;

    // Tank width to spawn at relative to terrain
    public int tank1X, tank2X;

    // The arrow to spawn
    public GameObject arrow;

    // Turnmanager to check who's turn it is
    public TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        // Get tilemap
        tilemap = GetComponent<Tilemap>();
        // Generate terrain once
        GenerateTerrain();
        // Spawn arrow
        SpawnArrow();
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn arrows every turn if a player hasn't won yet
        if (GameObject.Find("Tank1").GetComponent<Tank>().Health > 0 && GameObject.Find("Tank2").GetComponent<Tank>().Health > 0)
        {
            SpawnArrow();

            // Hide if buy menu up
            if (GameObject.Find("Container"))
            {
                Destroy(FindObjectOfType<Arrow>().gameObject);
            }
            else
            {
                SpawnArrow();
            }

            if (GameObject.Find("PauseMenu"))
            {
                if (GameObject.Find("Arrow(Clone)"))
                {
                    Destroy(FindObjectOfType<Arrow>().gameObject);
                }
                
            }
            else
            {
                SpawnArrow();
            }
        }

       
    }

    // Spawn arrow
    void SpawnArrow()
    {
        GameObject tank1 = GameObject.Find("Tank1");
        GameObject tank2 = GameObject.Find("Tank2");

        if (turnManager.IsPlayerTurn(1)
            && FindObjectOfType<Arrow>() == null
            && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            // Instantiate in front of tank
            Vector3 arrowPos = new Vector3(tank1.transform.position.x, tank1.transform.position.y);
            GameObject arw = Instantiate(arrow, arrowPos, Quaternion.identity);
        }
        if (turnManager.IsPlayerTurn(2)
            && FindObjectOfType<Arrow>() == null
            && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            // Instantiate in front of tank
            Vector3 arrowPos = new Vector3(tank2.transform.position.x, tank2.transform.position.y);
            GameObject arw = Instantiate(arrow, arrowPos, Quaternion.identity);
        }
    }

    // Generate new random terrain
    void GenerateTerrain()
    {
        float seed = Random.Range(0f, 100f);
        mapArray = GenerateArray(terrainWidth, terrainHeight, true, tank1X, tank2X);
        // Map before adding stone, gold, or diamond
        int[,] map = PerlinNoiseSmooth(mapArray, seed, perlinInterval);
        // Add stone and stuff
        map = AddStone(map);
        RenderMap(map, tilemap, baseTile, stoneTile, goldTile, diamondTile, tank1X, tank2X);
        RenderTank(Tank1Prefab, Tank2Prefab, mapArray, tank1X, tank2X, tilemap);
    }

    // Add stone and ores to the map
    // 2 = stone, 3 = gold, 4 = diamond
    int[,] AddStone(int[,] map)
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            // Stone and ores can be found minimum 5 layers below ground
            for (int y = FindLandGivenXCoord(x, map) - 5; y >= 0; y--)
            {
                if (map[x,y] == 1)
                {
                    // Random number to create probabilities
                    int randomNum = Random.Range(1, 10);
                    // Bottom 3 layers are always stone
                    if (y < 4)
                    {
                        map[x, y] = StoneOrOre();
                    }
                    // No dirt under stone
                    else if (map[x, y + 1] >= 2)
                    {
                        map[x, y] = StoneOrOre();
                    }
                    // 40% chance of stone for any eligible block
                    else if (randomNum > 6)
                    {
                        map[x, y] = StoneOrOre();
                    }
                }
            }
        }
        return map;
    }

    // Helper that decides if a given stone block will be stone, gold, or diamond
    int StoneOrOre()
    {
        int randomNum = Random.Range(1, 101);
        // 1% chance of diamonds
        if (randomNum == 100)
        {
            return 4;
        }
        // 2% chance of gold
        else if (randomNum >= 98)
        {
            return 3;
        }
        else
        {
            return 2;
        }
    }

    // Helper that finds the height of the land at a given x coordinate
    int FindLandGivenXCoord(int x, int[,] map)
    {
        for (int y = map.GetUpperBound(1); y >= 0; y--)
        {
            if (map[x, y] != 0)
            {
                return y;
            }
        }
        // This should never happen but if there is no terrain return 0
        return 0;
    }

    // Instantiate the tank players given the map that was generated
    public static void RenderTank(GameObject Tank1Prefab, GameObject Tank2Prefab, int[,] map, int tank1X, int tank2X, Tilemap tilemap)
    {
        // Find highest tile height in given X
        int tank1Y = map.GetUpperBound(1);
        int tank2Y = map.GetUpperBound(1);
        for (int y = map.GetUpperBound(1); y >= 0; y--)
        {
            if (map[tank1X, y] != 0)
            {
                tank1Y = y;
                break;
            }
        }
        for (int y = map.GetUpperBound(1); y >= 0; y--)
        {
            if (map[tank2X, y]!= 0)
            {
                tank2Y = y;
                break;
            }
        }
        // Convert tile to world position and instantiate
        Vector2 tank1Pos = tilemap.GetCellCenterWorld(new Vector3Int(tank1X, tank1Y, 0));
        Vector2 tank2Pos = tilemap.GetCellCenterWorld(new Vector3Int(tank2X, tank2Y, 0));
        tank1Pos.y += 1;
        tank2Pos.y += 1;
        GameObject tank1 = Instantiate(Tank1Prefab, tank1Pos, Quaternion.identity);
        tank1.name = "Tank1";
        GameObject tank2 = Instantiate(Tank2Prefab, tank2Pos, Quaternion.identity);
        tank2.transform.localScale = new Vector3(tank2.transform.localScale.x * -1, tank2.transform.localScale.y, tank2.transform.localScale.z);
        tank2.name = "Tank2";
    }

    // Generates an array with specified width, height, and either empty or full (0s or 1s)
    public static int[,] GenerateArray(int width, int height, bool empty, int tank1X, int tank2X)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }

    // Render tilemap by placing tiles if there is a 1 in a space on the map array
    public static void RenderMap(int[,] map, Tilemap tilemap, BetterRuleTile baseTile, BetterRuleTile stoneTile, BetterRuleTile goldTile, BetterRuleTile diamondTile, int tank1X, int tank2X)
    {
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        GameObject tank1 = GameObject.Find("Tank1");
        GameObject tank2 = GameObject.Find("Tank2");
        Destroy(tank1);
        Destroy(tank2);
        //Ensure theres a platform for the tanks
        for (int y = 0; y < map.GetUpperBound(1); y++)
        {
            map[tank1X - 1, y] = map[tank1X, y];
            map[tank1X + 1, y] = map[tank1X, y];
            map[tank2X - 1, y] = map[tank2X, y];
            map[tank2X + 1, y] = map[tank2X, y];
        }
        //Loop through the width of the map
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 0 = no tile, 1 = base tile, 2 = stone tile, 3 = gold tile, 4 = diamond tile
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), baseTile);
                }
                if (map[x, y] == 2)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), stoneTile);
                }
                if (map[x, y] == 3)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), goldTile);
                }
                if (map[x, y] == 4)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), diamondTile);
                }
            }
        }
    }

    // Randomize map using smoothed Perlin noise function
    public static int[,] PerlinNoiseSmooth(int[,] map, float seed, int interval)
    {
        //Smooth the noise and store it in the int array
        if (interval > 1)
        {
            int newPoint, points;
            //Used to reduced the position of the Perlin point
            float reduction = 0.5f;

            //Used in the smoothing process
            Vector2Int currentPos, lastPos;
            //The corresponding points of the smoothing. One list for x and one for y
            List<int> noiseX = new List<int>();
            List<int> noiseY = new List<int>();

            //Generate the noise
            for (int x = 0; x < map.GetUpperBound(0); x += interval)
            {
                newPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x, (seed * reduction))) * map.GetUpperBound(1));
                noiseY.Add(newPoint);
                noiseX.Add(x);
            }

            points = noiseY.Count;

            //Start at 1 so we have a previous position already
            for (int i = 1; i < points; i++)
            {
                //Get the current position
                currentPos = new Vector2Int(noiseX[i], noiseY[i]);
                //Also get the last position
                lastPos = new Vector2Int(noiseX[i - 1], noiseY[i - 1]);

                //Find the difference between the two
                Vector2 diff = currentPos - lastPos;

                //Set up what the height change value will be
                float heightChange = diff.y / interval;
                //Determine the current height
                float currHeight = lastPos.y;

                //Work our way through from the last x to the current x
                for (int x = lastPos.x; x < currentPos.x; x++)
                {
                    for (int y = Mathf.FloorToInt(currHeight); y > 0; y--)
                    {
                        map[x, y] = 1;
                    }
                    currHeight += heightChange;
                }
            }
        }
        else
        {
            //Defaults to a normal Perlin gen
            map = PerlinNoise(map, seed);
        }

        return map;
    }

    // Non smoothed Perlin noise function used as a helper to the smoothed one
    public static int[,] PerlinNoise(int[,] map, float seed)
    {
        int newPoint;
        //Used to reduced the position of the Perlin point
        float reduction = 0.5f;
        //Create the Perlin
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            newPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x, seed) - reduction) * map.GetUpperBound(1));

            //Make sure the noise starts near the halfway point of the height
            newPoint += (map.GetUpperBound(1) / 2);
            for (int y = newPoint; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }
        return map;
    }
}
