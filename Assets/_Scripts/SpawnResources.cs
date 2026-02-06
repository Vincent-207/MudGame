using System.Collections.Generic;
using UnityEngine;

public class SpawnResources : MonoBehaviour
{
    [SerializeField] GameObject resourcePrefab, resourceHolder;
    public int xSize, zSize;
    public float xScale, yScale, spawnScale, spawnThreshold, spawnChance;
    public bool Regen = false;
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
        Vector3 spawnPos = new Vector3(x * spawnScale,0, z * spawnScale);
        Instantiate(resourcePrefab, spawnPos, Quaternion.identity, resourceHolder.transform);

    }
}
