using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainDestroyer : MonoBehaviour
{
    // Tilemap to destroy
    public Tilemap tilemap;

    // Gold and diamond tiles to check if one is destroyed
    public BetterRuleTile goldTile;
    public BetterRuleTile diamondTile;

    private TurnManager turnManager;

    // Get turn manager to correctly award any points earned
    private void Start()
    {
        turnManager = GameObject.FindObjectOfType<TurnManager>();
    }

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
                TileBase tile = tilemap.GetTile(tilePos);
                // Need to check if tile is within the radius distance to ensure exposion is circular and not square
                // -> this means we are checking a few unnecessary tiles but I can't currently think of how to do this more efficiently
                if (tile != null && Vector3.Distance(tilePos, tilemap.WorldToCell(explosionLocation)) <= radius)
                {
                    // Award player points for destroying gold
                    if (tile.Equals(goldTile))
                    {
                        // Give player 2 points if it is player 1's turn (since turns change directly after shooting)
                        if (turnManager.IsPlayerTurn(1))
                        {
                            GameObject.Find("Tank2").GetComponent<Tank>().turnPoints += 1000;
                        }
                        else
                        {
                            GameObject.Find("Tank1").GetComponent<Tank>().turnPoints += 1000;
                        }
                    }
                    // Award player even more points for destroying diamond
                    if (tile.Equals(diamondTile))
                    {
                        // Give player 2 points if it is player 1's turn (since turns change directly after shooting)
                        if (turnManager.IsPlayerTurn(1))
                        {
                            GameObject.Find("Tank2").GetComponent<Tank>().turnPoints += 100000;
                        }
                        else
                        {
                            GameObject.Find("Tank1").GetComponent<Tank>().turnPoints += 100000;
                        }
                    }
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
