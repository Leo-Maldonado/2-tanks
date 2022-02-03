using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{

    
    // Array to keep track of where tiles should be placed
    int[,] mapArray;

    // Tilemap we use
    Tilemap tilemap;

    // Base tile for the tilemap (should be a rule tile)
    public TileBase tile;

    // Sets width and height of generated terrain, and interval for Perlin noise function (lower interval means more randomness)
    public int terrainWidth, terrainHeight, perlinInterval;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, true);
        // Get tilemap
        tilemap = GetComponent<Tilemap>();
        // Generate terrain once
        GenerateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        // For now, generate new terrain every time the space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateTerrain();
        }
    }

    // Generate new random terrain
    void GenerateTerrain()
    {
        float seed = Random.Range(0f, 100f);
        mapArray = GenerateArray(terrainWidth, terrainHeight, true);
        RenderMap(PerlinNoiseSmooth(mapArray, seed, perlinInterval), tilemap, tile);

    }

    // Generates an array with specified width, height, and either empty or full (0s or 1s)
    public static int[,] GenerateArray(int width, int height, bool empty)
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
    public static void RenderMap(int[,] map, Tilemap tilemap, TileBase tile)
    {
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
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
