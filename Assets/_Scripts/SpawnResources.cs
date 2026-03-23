using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnResources : MonoBehaviour
{
    [SerializeField] GameObject resourcePrefab, resourceHolder;
    public int xSize, zSize;
    public float xScale, yScale, spawnScale, spawnThreshold, spawnChance;
    public bool Regen = false;
    public GameObject[] spawnTypes;
    public int seed, regionSize = 10;
    void Start()
    {
        GenerateNodes();
    }
    
    void Update()
    {
        if(Regen)
        {
            DestroyAllChildren();
            GenerateNodes();
            Regen = false;
        }
    }

    void DestroyAllChildren()
    {
        List<GameObject> children = new();
        for(int childIndex = 0; childIndex < resourceHolder.transform.childCount; childIndex++)
        {
            children.Add(resourceHolder.transform.GetChild(childIndex).gameObject);
        }

        foreach(GameObject child in children)
        {
            Destroy(child);
        }
    }

    void GenerateNodes()
    {
        for(int z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {   
                float perlinValue = yScale * Mathf.PerlinNoise(x * xScale , z * xScale);
                if(spawnThreshold >= perlinValue)
                {
                    float chance = Random.Range(0f, 1f);
                    if(spawnChance > chance) CreateNode(x, z);
                }
            }
            
        }
    }

    void CreateNode(int x, int z)
    {
        GameObject prefabToSpawn =  spawnTypes[Random.Range(0, spawnTypes.Length)];
        float meshHeight = prefabToSpawn.GetComponent<MeshFilter>().sharedMesh.bounds.size.y;

        RaycastHit hit;
        float ySpawnPos = 0f;
        Vector3 offset = Random.insideUnitCircle;
        offset.z = offset.y; offset.y = 0;

        if(Physics.Raycast(new Vector3(x * spawnScale, 100, z * spawnScale) + offset, Vector3.down, out hit))
        {
            ySpawnPos = hit.point.y;
        }
        ySpawnPos += meshHeight * 0.5f;
        Vector3 spawnPos = new Vector3(x * spawnScale, ySpawnPos, z * spawnScale) + offset * 2;
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, resourceHolder.transform);

        int region = x/regionSize + z/regionSize;
        Random.InitState(region + seed);


        // convert chance to material. 
        // meshRenderer.material = spawnTypes[Random.Range(0, spawnTypes.Length)];
        

    }

    

    
}
