using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityTemplateProjects;

public class WorldGenerator : MonoBehaviour
{
    private BlockClasses _blockManager;
    //public GameObject blockObject;
    public GameObject chunkObject;
    public int oneChunkSize = 16;
    public int chunksPerDirection = 2;

    public int maxHeight = 10;
    public ChunkGenVariables vars;

    public void CreateWorld(BlockClasses blocks)
    {
        GameObject chunkInstance = Instantiate(chunkObject, transform.position, new Quaternion(0,0,0,0), transform);
        MeshFilter chunkInstanceMesh = chunkInstance.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        
        /*
        Vector3[] vertices = new Vector3[3];//chunkInstanceMesh.sharedMesh.vertices;
        vertices[0] = new Vector3(-1, 0, 0);
        vertices[1] = new Vector3(1, 0, 0);
        vertices[2] = new Vector3(0, 1.7f, 0);
        mesh.vertices = vertices;
        */

        mesh.vertices = MeshUtils.createWorldMesh(blocks);
        
        //mesh.uv = MeshUtils.GetUvs(3);
        
        mesh.triangles = MeshUtils.GetTris(mesh.vertices.Length);
        
        chunkInstanceMesh.sharedMesh = mesh;

        MeshCollider meshCollider = chunkInstance.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = mesh;
        }
        
        /*
        foreach (Block block in blocks.world)
        {
            GameObject blockInstance = Instantiate(BlockObject, block.pos.ToUnity() + transform.position, new Quaternion(0,0,0,0), transform);
        }
        */
    }

    public void Generate(int seed, float noiseScale, int maxHeight, int chunkSize, int chunks, ChunkGenVariables vars)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        if (seed == 0)
        {
            seed = Random.Range(-100000, 100000);
        }
        
        for (int x = 0; x < chunks; x++)
        {
            int genPosX = chunkSize * x;
            for (int z = 0; z < chunks; z++)
            {
                int genPosZ = chunkSize * z;
                GenerateSingleChunk(seed, noiseScale, maxHeight, new Vector2(genPosX,genPosZ), new Vector2(genPosX + chunkSize, genPosZ + chunkSize), vars);
            }
        }
    }

    public void GenerateSingleChunk(int seed, float noiseScale, int maxHeight, Vector2 startPos, Vector2 endPos, ChunkGenVariables vars)
    {
        _blockManager.GenerateWorld(seed, noiseScale, maxHeight,startPos, endPos, vars);
        CreateWorld(_blockManager);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _blockManager = new BlockClasses();
        Generate(0, 2, maxHeight, oneChunkSize, chunksPerDirection, vars);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Generate(0, 2, maxHeight, oneChunkSize, chunksPerDirection, vars);
        }
    }
}
