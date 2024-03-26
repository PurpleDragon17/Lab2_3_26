using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TileGen : MonoBehaviour
{
   

    //Code based on example scense 6
    private int xWidth;
    private int zWidth;
    private Mesh mesh;
    private Vector3[] vertices;
    private Texture2D noiseTex;

    [SerializeField]
    private GameObject thing1Prefab;

    [SerializeField]
    NoiseMapGen noiseMapGeneration;

    [SerializeField]
    private MeshRenderer tileRenderer;

    [SerializeField]
    private MeshFilter meshFilter;

    [SerializeField]
    private MeshCollider meshCollider;

    [SerializeField]
    private float mapScale;

    [SerializeField]
    private TerrainType[] terrainTypes;

    [SerializeField]
    private float heightMultipier;

    [SerializeField]
    private AnimationCurve heightCurve;

    [SerializeField]
    private NoiseMapGen.Wave[] waves;

    private Texture2D tileTexture;

    //[SerializeField]
    private GameObject lvl;


    private GameObject[] lel;

    

    // public int[] heightvalues;

    //public float[,] heightMap;

   



    private void Start()
    {
        lel = GameObject.FindGameObjectsWithTag("LVL");
        lvl = lel[0];
        //GenerateTile();
      
    }
    public TileMapData GenerateTile()
    {
       // LvGen meow = lvl.GetComponent<LvGen>();
        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
        int tileWidth = tileDepth;

        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;
        float[,] heightMap = this.noiseMapGeneration.GenerateNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, waves);
       //Getting the Hightmap Here!!  
        Texture2D tileTexture = BuildTexture(heightMap);
        this.tileRenderer.material.mainTexture = tileTexture;
        UpdateMeshVertices(heightMap);
        TileMapData tileMapData = new TileMapData(heightMap);
        return tileMapData;
      


    }

    private Texture2D BuildTexture(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];
                TerrainType terrainType = ChooseTerrainType(height);
                colorMap[colorIndex] = terrainType.color;
            }
        }

        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();

        return tileTexture;
    }

    [System.Serializable]
    public class TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }

    TerrainType ChooseTerrainType(float height)

    {
        foreach (TerrainType terrainType in terrainTypes)
        {
            if (height < terrainType.height)
            {
                return terrainType;
            }
        }
        return terrainTypes[terrainTypes.Length - 1];
    }

   
        private void UpdateMeshVertices(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Vector3[] meshVertices = this.meshFilter.mesh.vertices;

        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                float height = heightMap[zIndex, xIndex];
                Vector3 vertex = meshVertices[vertexIndex];
                meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultipier, vertex.z);
                vertexIndex++;
            }
        }

        // Update the mesh vertices
        this.meshFilter.mesh.vertices = meshVertices;
        this.meshFilter.mesh.RecalculateBounds();
        this.meshFilter.mesh.RecalculateNormals();
        this.meshCollider.sharedMesh = this.meshFilter.mesh;

       

    }
}
public class TileMapData
{
  
    public float[,] heightMap;
    public Dictionary<Vector2Int, float[,]> heightmaps = new Dictionary<Vector2Int, float[,]>();
    public List<Vector2Int> generationOrder = new List<Vector2Int>();

    // Set the height map data
    public TileMapData(float[,] heightMap)
    {
        this.heightMap = heightMap;
    }

    public void SetHeightmapData(Vector2Int tilePosition, float[,] heightmap)
    {
        if (!heightmaps.ContainsKey(tilePosition))
        {
            heightmaps.Add(tilePosition, heightmap);
            generationOrder.Add(tilePosition);
        }
        else
        {
            Debug.LogWarning("Heightmap data already exists for tile at position " + tilePosition);
        }
    }

    // Method to retrieve height value at a specific position
    public float[,] GetHeightmapData(Vector2Int tilePosition)
    {
        if (heightmaps.ContainsKey(tilePosition))
        {
            return heightmaps[tilePosition];
        }
        else
        {
            Debug.LogWarning("No heightmap data found for tile at position " + tilePosition);
            return null;
        }
    }
    public List<Vector2Int> GetGenerationOrder()
    {
        return generationOrder;
    }
    public Dictionary<Vector2Int, float[,]> tileHeightMaps = new Dictionary<Vector2Int, float[,]>();

    // Method to set heightmap data for a specific tile
    public void SetTileHeightMapData(Vector2Int tilePosition, float[,] heightMapData)
    {
        if (tileHeightMaps.ContainsKey(tilePosition))
        {
            // Tile already exists, so update its heightmap data
            tileHeightMaps[tilePosition] = heightMapData;
        }
        else
        {
            // Tile doesn't exist, so add it along with its heightmap data
            tileHeightMaps.Add(tilePosition, heightMapData);
        }

    }
    public float[,] GetTileHeightMapData(Vector2Int tilePosition)
    {
        if (tileHeightMaps.ContainsKey(tilePosition))
        {
            return tileHeightMaps[tilePosition];
        }
        else
        {
            Debug.LogWarning("No heightmap data found for tile at position " + tilePosition);
            return null;
        }
    }
}
