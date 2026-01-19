using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapGenerator : MonoBehaviour
{
    public int xSize, zSize;
    public float xScale, yScale;
    int[] triangles;
    Vector3[] verticies;
    public bool Regen;
    void Start()
    {
        GenerateMap();
    }
    void Update()
    {
        if(Regen)
        {
            GenerateMap();
            Regen = false;
        }
    }
    void GenerateMap()
    {
        verticies = new Vector3[(xSize + 1) * (zSize + 1)];
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {   
                float y = yScale * Mathf.PerlinNoise(x * xScale , z * xScale);
                verticies[i] = (new Vector3(x, y, z));
                i++;            
            }
            
        }

        int vert = 0, tris = 0;
        triangles = new int[xSize * zSize * 6];
        for(int z = 0; z < zSize; z++)
        {
            for(int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = (vert);
                triangles[tris + 1] = (vert + xSize + 1);
                triangles[tris + 2] = (vert + 1);
                triangles[tris + 3] = (vert + 1);
                triangles[tris + 4] = (vert + xSize + 1);
                triangles[tris + 5] = (vert + xSize + 2);
                
                vert++;
                tris += 6;
            }
            vert++;
        }



        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
