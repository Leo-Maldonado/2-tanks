using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MountainMaker : MonoBehaviour
{
    // Tile to spawn when landing
    public BetterRuleTile dirtTile;

    // Tilemap to add tile to
    Tilemap tilemap;

    // Radius of how far away from the explosion land is added
    int dirtRadius = 8;

    private void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Find tanks to avoid spawning dirt on top of them
        GameObject tank1 = GameObject.Find("Tank1");
        GameObject tank2 = GameObject.Find("Tank2");
        // Convert explosion location to tile coordinates
        Vector3Int explosionTile = tilemap.WorldToCell(transform.position);
        for (int x = -dirtRadius; x <= dirtRadius; x++)
        {
            for (int y = -dirtRadius; y <= dirtRadius; y++)
            {
                Vector3Int tilePos = explosionTile + new Vector3Int(x, y, 0);
                // Need to check if tile is within the radius distance to ensure exposion is circular and not square
                // -> this means we are checking a few unnecessary tiles but I can't currently think of how to do this more efficiently
                if (Vector3.Distance(tilePos, explosionTile) <= dirtRadius
                    && !nearTank(tilePos, tank1, tank2))
                {
                    tilemap.SetTile(tilePos, dirtTile);
                }
            }
        }
    }

    // Function to check if a tile is too close to a tank to make it dirt
    private bool nearTank(Vector3Int tilePos, GameObject tank1, GameObject tank2)
    {
        Vector3Int tank1Tile = tilemap.WorldToCell(tank1.transform.position);
        Vector3Int tank2Tile = tilemap.WorldToCell(tank2.transform.position);
        // Return false if both tanks are more than 3 tiles away
        if (Vector3.Distance(tilePos, tank1Tile) > 3 && Vector3.Distance(tilePos, tank2Tile) > 3)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
