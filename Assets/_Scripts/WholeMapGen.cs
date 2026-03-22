using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

public class WholeMapGen : MonoBehaviour
{
    public Vector2Int size;
    public Vector2 scale;
    public int mapWidth;
    public Material groundMat;

    void Start()
    {
        for(int x = 1; x <= mapWidth; x++)
        {
            for(int z = 1; z <= mapWidth; z++)
            {
                GenerateChunk(new Vector2Int(x, z));
            }
        }
    }

    void GenerateChunk(Vector2Int gridPos)
    {
        GameObject chunk = new GameObject();
        chunk.transform.parent = transform;
        chunk.transform.localPosition = new Vector3(size.x * gridPos.x, 0, size.y * gridPos.y);
        MapGenerator chunkMapGen = chunk.AddComponent<MapGenerator>();
        chunkMapGen.Init(size, scale, new Vector2Int(gridPos.x * size.x, gridPos.y * size.y));
        chunk.GetComponent<MeshRenderer>().material = groundMat;
    }
}
