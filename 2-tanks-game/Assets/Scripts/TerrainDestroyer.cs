using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainDestroyer : MonoBehaviour
{
    // Tilemap to destroy
    public Tilemap tilemap;

    // Destroy terrain at the explosion location with the specified explosion radius
    public void DestroyTerrain(Vector3 explosionLocation, int radius)
    {
        // Convert explosion location to tile coordinates
        Vector3Int explosionTile = tilemap.WorldToCell(explosionLocation);

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector3Int tilePos = explosionTile + new Vector3Int(x, y, 0);
                // Need to check if tile is within the radius distance to ensure exposion is circular and not square
                // -> this means we are checking a few unnecessary tiles but I can't currently think of how to do this more efficiently
                if ((tilemap.GetTile(tilePos) != null) && (Vector3.Distance(tilePos, tilemap.WorldToCell(explosionLocation)) <= radius))
                {
                    DestroyTile(tilePos);
                }
            }
        }
    }

    // Destroy the specified tile
    void DestroyTile(Vector3Int tilePos)
    {
        tilemap.SetTile(tilePos, null);
    }
}
