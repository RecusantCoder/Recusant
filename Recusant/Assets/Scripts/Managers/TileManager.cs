using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private Transform playerTransform;
    public Tilemap walkableTilemap;
    public Tilemap collisionTilemap;
    public TileBase[] walkableTilePrefabs;
    public TileBase[] collisionTilePrefabs;
    public GameObject player;
    public int tilesPerSide = 10;
    public float tileLength = 1f;
    
    private Vector3 lastPlayerPos;

    private List<Vector3Int> spawnedWalkableTilePositions = new List<Vector3Int>();
    private List<Vector3Int> spawnedCollisionTilePositions = new List<Vector3Int>();

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        CreateTilesAroundPlayer();
    }

    private void Update()
    {
        if (IsPlayerOnEdgeOfTile())
        {
            CreateTilesAheadOfPlayer();
        }
    }

    private bool IsPlayerOnEdgeOfTile()
    {
        Vector3 playerPos = playerTransform.position;
        Vector3Int currentTileCoords = walkableTilemap.WorldToCell(playerPos);
        BoundsInt bounds = new BoundsInt(currentTileCoords.x - 1, currentTileCoords.y - 1, 0, 3, 3, 1);
        foreach (Vector3Int tileCoords in bounds.allPositionsWithin)
        {
            if (!spawnedWalkableTilePositions.Contains(tileCoords))
            {
                return true;
            }
        }
        return false;
    }

    private void CreateTilesAroundPlayer()
    {
        Vector3 playerPos = playerTransform.position;
        int currentX = Mathf.RoundToInt(playerPos.x / tileLength);
        int currentY = Mathf.RoundToInt(playerPos.y / tileLength);
        int tilesPerSideHalf = tilesPerSide / 2;

        for (int x = currentX - tilesPerSideHalf; x <= currentX + tilesPerSideHalf; x++)
        {
            for (int y = currentY - tilesPerSideHalf; y <= currentY + tilesPerSideHalf; y++)
            {
                Vector3Int tileCoords = new Vector3Int(x, y, 0);

                if (!spawnedWalkableTilePositions.Contains(tileCoords))
                {
                    TileBase tilePrefab = walkableTilePrefabs[Random.Range(0, walkableTilePrefabs.Length)];
                    walkableTilemap.SetTile(tileCoords, tilePrefab);
                    spawnedWalkableTilePositions.Add(tileCoords);
                }

                if (!spawnedCollisionTilePositions.Contains(tileCoords))
                {
                    TileBase collisionTilePrefab = collisionTilePrefabs[Random.Range(0, collisionTilePrefabs.Length)];
                    //collisionTilemap.SetTile(tileCoords, collisionTilePrefab);
                    spawnedCollisionTilePositions.Add(tileCoords);
                }
            }
        }
    }
    
    private void CreateTilesAheadOfPlayer()
{
    Vector3 playerPos = playerTransform.position;
    int currentX = Mathf.RoundToInt(playerPos.x / tileLength);
    int currentY = Mathf.RoundToInt(playerPos.y / tileLength);

    // Determine the direction the player is moving
    Vector3 movementDir = playerPos - lastPlayerPos;
    int nextX = movementDir.x > 0 ? currentX + tilesPerSide / 2 + 1 : currentX - tilesPerSide / 2 - 1;
    int nextY = movementDir.y > 0 ? currentY + tilesPerSide / 2 + 1 : currentY - tilesPerSide / 2 - 1;

    // Create the new tiles
    for (int x = nextX - tilesPerSide / 2; x <= nextX + tilesPerSide / 2; x++)
    {
        for (int y = nextY - tilesPerSide / 2; y <= nextY + tilesPerSide / 2; y++)
        {
            Vector3Int tileCoords = new Vector3Int(x, y, 0);

            // Only create the tile if it doesn't exist yet
            if (!spawnedWalkableTilePositions.Contains(tileCoords))
            {
                TileBase tilePrefab = walkableTilePrefabs[Random.Range(0, walkableTilePrefabs.Length)];
                walkableTilemap.SetTile(tileCoords, tilePrefab);
                spawnedWalkableTilePositions.Add(tileCoords);
            }

            if (!spawnedCollisionTilePositions.Contains(tileCoords))
            {
                TileBase collisionTilePrefab = collisionTilePrefabs[Random.Range(0, collisionTilePrefabs.Length)];
                //collisionTilemap.SetTile(tileCoords, collisionTilePrefab);
                spawnedCollisionTilePositions.Add(tileCoords);
            }
        }
    }

    // Remove tiles that are no longer visible
    List<Vector3Int> tilesToRemove = new List<Vector3Int>();
    foreach (Vector3Int tileCoords in spawnedWalkableTilePositions)
    {
        if (Mathf.Abs(tileCoords.x - currentX) > tilesPerSide / 2 + 1 || Mathf.Abs(tileCoords.y - currentY) > tilesPerSide / 2 + 1)
        {
            tilesToRemove.Add(tileCoords);
        }
    }
    foreach (Vector3Int tileCoords in tilesToRemove)
    {
        walkableTilemap.SetTile(tileCoords, null);
        spawnedWalkableTilePositions.Remove(tileCoords);
    }

    lastPlayerPos = playerPos;
}



}


