using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Akino_MapManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject homeTile;
    public List<GameObject> mapTiles;
    public GameObject instancedTile;
    public Button startButton;
    [Header("Variables")]
    public bool divisible;
    public bool rerolling;
    public bool doorDeleted = true;
    public Transform doorDirection = null;
    public Transform[] doorDirectionList;
    public Quaternion nextTileRotation;
    [Header("Private")]
    [SerializeField] Lan_WaveSpawner waveSpawner;
    [SerializeField] NavMeshSurface navigationMesh;
    [SerializeField] Quaternion[] rotations;
    [SerializeField] GameObject lastSpawnedTile;
    [SerializeField] Akino_DoorDeletion doorScript;

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
        waveSpawner = GetComponent<Lan_WaveSpawner>();
        int rotationIndex = Random.Range(0, rotations.Length);
        Quaternion homeRotation = rotations[rotationIndex];
        lastSpawnedTile = Instantiate(homeTile, Vector3.zero, homeRotation);
        doorDirectionList[0] = lastSpawnedTile.transform;
        doorDirection = lastSpawnedTile.transform;
        doorScript = lastSpawnedTile.GetComponentInChildren<Akino_DoorDeletion>();

        UpdateNavMesh();
    }

    

    private void Update()
    {
        if (rerolling && waveSpawner.divisible())
        {
            startButton.interactable = false;
            if (!doorDeleted || doorScript.touchedGrass)
            {
                int tileIndex = Random.Range(0, mapTiles.Count);
                Vector3 newTilePosition = Vector3.zero;
                RandomEntrance();
                newTilePosition = lastSpawnedTile.transform.position + doorDirection.rotation * new Vector3(0f, 0f, 1f) * 50;
                int rotationIndex = Random.Range(0, rotations.Length);
                Quaternion tileRotation = rotations[rotationIndex];
                Destroy(instancedTile);
                StartCoroutine(RerollTile(tileIndex, newTilePosition, tileRotation));
            }
            else
            {
                lastSpawnedTile = instancedTile;
                instancedTile = null;
                doorScript = null;
               
                //Debug.Log("finished");
                UpdateNavMesh();
                
                rerolling = false;
                doorDeleted = false;

                int numberOfChildren = lastSpawnedTile.transform.childCount;
    
                for (int i = 0; i < numberOfChildren; i++)
                {
                    if (lastSpawnedTile.transform.GetChild(i).name == "DoorCollider")
                    {
                        doorDirectionList[i] = lastSpawnedTile.transform.GetChild(i);
                        Debug.Log("doorDirection:" + i + " " + doorDirectionList[i]);
                    }
                }
                Lan_EventManager.NewSpawnedPoint();
                
            }
        }
    }
    public void RandomEntrance()//Select door direction
    {
        Debug.Log("Random is running");
        int numberOfLegitEntrance = 0;
        
        foreach (Transform doorEntrance in doorDirectionList)
        {
            if (doorEntrance != null)
            {
                numberOfLegitEntrance++;
                
            }
        }
        Debug.Log(numberOfLegitEntrance);
        int rand = Random.Range(0, numberOfLegitEntrance);
        Debug.Log("Random number is: " + rand);
        doorDirection = doorDirectionList[rand].transform;
    }
    private void LateUpdate()
    {
        if (doorScript == null)
        {
            doorScript = doorDirection.GetComponent<Akino_DoorDeletion>();
        }
    }

    IEnumerator RerollTile(int tileIndex, Vector3 newTilePosition, Quaternion tileRotation)
    {
        rerolling = false;
        instancedTile = Instantiate(mapTiles[tileIndex], newTilePosition, tileRotation);
        yield return new WaitForSeconds(0.1f);
        rerolling = true;
    }

    public void UpdateMap()
    {
        rerolling = true;
    }

    public void UpdateNavMesh()
    {
        navigationMesh.BuildNavMesh();
    }
    
}
