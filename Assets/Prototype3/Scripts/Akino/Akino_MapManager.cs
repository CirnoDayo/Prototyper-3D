using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Akino_MapManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject homeTile;
    public List<GameObject> mapTiles;
    public bool doorDeleted;
    [Header("Private")]
    [SerializeField] NavMeshSurface navigationMesh;
    [SerializeField] Quaternion[] rotations;
    [SerializeField] GameObject lastSpawnedTile;

    private void Awake()
    {
        rotations = new Quaternion[4];
        rotations[0] = Quaternion.Euler(0, 0, 0);
        rotations[1] = Quaternion.Euler(0, 90, 0);
        rotations[2] = Quaternion.Euler(0, 180, 0);
        rotations[3] = Quaternion.Euler(0, 270, 0);
    }

    private void Start()
    {
        int rotationIndex = Random.Range(0,rotations.Length);
        Quaternion homeRotation = rotations[rotationIndex];
        lastSpawnedTile = Instantiate(homeTile, Vector3.zero, homeRotation);
        UpdateNavMesh();
    }

    public void UpdateMap()
    {
        int tileIndex = Random.Range(0,mapTiles.Count);
        Vector3 newTilePosition = lastSpawnedTile.transform.position + lastSpawnedTile.transform.forward * 50;

        int rotationIndex = Random.Range(0, rotations.Length);
        Quaternion tileRotation = rotations[rotationIndex];

        lastSpawnedTile = Instantiate(mapTiles[tileIndex], newTilePosition, tileRotation);
    }

    public void UpdateNavMesh()
    {
        navigationMesh.BuildNavMesh();
    }
}
