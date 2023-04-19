using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] tilePrefabs;
    public GameObject player;
    public int tilesPerSide = 10;
    public float tileLength = 1f;

    private List<Vector3Int> spawnedTilePositions = new List<Vector3Int>();

    private void Start()
    {
        SpawnTilesAroundPlayer();
    }

    private void Update()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3Int playerTilePosition = tilemap.WorldToCell(playerPosition);
        if (!spawnedTilePositions.Contains(playerTilePosition))
        {
            SpawnTilesAroundPlayer();
        }
    }

    private void SpawnTilesAroundPlayer()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3Int playerTilePosition = tilemap.WorldToCell(playerPosition);

        int tilesPerHalfSide = tilesPerSide / 2;
        Vector3Int minTilePosition = playerTilePosition - new Vector3Int(tilesPerHalfSide, tilesPerHalfSide, 0);
        Vector3Int maxTilePosition = playerTilePosition + new Vector3Int(tilesPerHalfSide, tilesPerHalfSide, 0);

        for (int x = minTilePosition.x; x <= maxTilePosition.x; x++)
        {
            for (int y = minTilePosition.y; y <= maxTilePosition.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (!spawnedTilePositions.Contains(tilePosition))
                {
                    TileBase tile = tilePrefabs[RandomPrefabIndex()];
                    tilemap.SetTile(tilePosition, tile);
                    spawnedTilePositions.Add(tilePosition);
                }
            }
        }
    }

    private int RandomPrefabIndex()
    {
        if (tilePrefabs == null || tilePrefabs.Length == 0)
        {
            return 0;
        }

        return Random.Range(0, tilePrefabs.Length);
    }
}