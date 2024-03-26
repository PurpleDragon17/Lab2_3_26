using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvGen : MonoBehaviour
{
    //This code is doing a lot 
    //This spawns the tiles as children of this Level 
    //This combinds the meshes of the tiles into one mesh 
    //This spawns the objects (cyalnders and Spheres) 
    //This spawns the cabbin 
    [SerializeField]
    private int mapWidth, mapDepth;

    [SerializeField]
    private GameObject tilePrefab;

    public List<MeshFilter> tileMeshes;

    [SerializeField]
    private GameObject lvl;

    private MeshFilter[] mfs;

    private Mesh mesh;
    private Vector3[] vertices;
    [SerializeField]
    private GameObject thing1Prefab;
    private int xWidth;
    private int zWidth;

    private int tileWidth;
    private int tileDepth;

    private List<Vector3> vert;

    [SerializeField]
    private GameObject cabin;

    [SerializeField]
    private GameObject cabin2;

    [SerializeField]
    private GameObject cabin3;

    [SerializeField]
    private GameObject thing2Prefab;

    public List<GameObject> orblist;

    private Vector3[] cat;

    public int[] heightvalues;

    private List<Vector3> verticesList;

    private Vector3[] verticesArray;

    public TileMapData tileMapData;

    public TileMapData[,] tilesMapsDatas;
    public TileMapData tileMapDataNow;

    //public TileMapData tileMapData;
    public MeshFilter meshFilter;
    public float heightMultiplier = 1.0f;
    void Start()
    {
        // tileMapData = GameObject.FindObjectOfType<TileMapData>();
        GenerateMap();
       // CombineMeshes(gameObject);
       // RayCasting();
        
        AddThings();

       // int[] heightvalues = new int[leg];        // Debug.Log("Done");
    }
    void GenerateMap()
    {
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        tileWidth = (int)tileSize.x;
        tileDepth = (int)tileSize.z;
        //checking how big the tile is

        for (int xTileIndex = 0; xTileIndex < mapWidth; xTileIndex++)
        {
            for (int zTileIndex = 0; zTileIndex < mapDepth; zTileIndex++)
            {
                //This will tell the tiles where they need to be
                Vector3 tilePosition = new Vector3(
                    this.gameObject.transform.position.x + xTileIndex * tileWidth,
                    this.gameObject.transform.position.y,
                    this.gameObject.transform.position.z + zTileIndex * tileDepth);
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
             //  Vector3 Uu = tile.GetComponent<MeshFilter>().mesh.vertice
                tile.transform.parent = transform;
                TileMapData tileMapData = tile.GetComponent<TileGen>().GenerateTile();

                tilesMapsDatas[xTileIndex, zTileIndex] = tileMapData;
                //Still getting an error that this is not set to an isstance of an object 
                //Code based on GDD316 example generation 
            }
        }
        //Make the tiles apper, and in the right possitions
    }

   

    Vector3Int MapToTileMapPosition(Vector3 vertexPosition)
    {
        // Implement logic to map mesh vertex position to corresponding tile map position
        // Example: Convert world coordinates to grid coordinates
        Vector3Int tileMapPosition = new Vector3Int(
            Mathf.FloorToInt(vertexPosition.x),
            Mathf.FloorToInt(vertexPosition.y),
            Mathf.FloorToInt(vertexPosition.z)
        );
        return tileMapPosition;
    }
    void CombineMeshes(GameObject parentObject)
    {
        {
            // Create a list to store the MeshFilter components
            List<MeshFilter> meshFilters = new List<MeshFilter>();

            // Get all MeshFilter components in the children of parentObject
            meshFilters.AddRange(parentObject.GetComponentsInChildren<MeshFilter>());

            // Create an array of CombineInstance objects
            CombineInstance[] combine = new CombineInstance[meshFilters.Count];

            // Populate the CombineInstance array with mesh data
            for (int i = 0; i < meshFilters.Count; i++)
            {
                // Check if the MeshFilter has a shared mesh
                if (meshFilters[i].sharedMesh != null)
                {
                    combine[i].mesh = meshFilters[i].sharedMesh;
                    combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                }
                else
                {
                    // Handle the case where a shared mesh is null
                    Debug.LogError("MeshFilter at index " + i + " has a null shared mesh.");
                    return;
                }
            }

            // Create a new GameObject to hold the combined mesh
            GameObject combinedMeshObject = new GameObject("CombinedMesh");
            MeshFilter combinedMeshFilter = combinedMeshObject.AddComponent<MeshFilter>();
            combinedMeshFilter.mesh = new Mesh();

            // Combine meshes
            combinedMeshFilter.mesh.CombineMeshes(combine);

            // Optional: Add MeshRenderer component to display the combined mesh
            combinedMeshObject.AddComponent<MeshRenderer>().sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;

            MeshFilter meshFilter = GetComponent<MeshFilter>();

            // Check if MeshFilter component exists
            if (meshFilter != null)
            {
                // Get the Mesh component from the MeshFilter
                Mesh mesh = meshFilter.mesh;

                // Check if Mesh component exists
                if (mesh != null)
                {
                    // Access the vertices of the mesh
                    Vector3[] vertices = mesh.vertices;

                    // Log the vertices to the console
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        Debug.Log("Vertex " + i + ": " + vertices[i]);
                    }
                }
                else
                {
                    Debug.LogError("Mesh component not found.");
                }
            }
            else
            {
                Debug.LogError("MeshFilter component not found.");
            }
        
    }





    }

   void RayCasting()
    {
        int numVertices = 10;
        float raycastDistance = 10f;
        for (int i = 0; i < numVertices; i++)
        {
            float angel = i * (2 * Mathf.PI / numVertices);
            Vector3 rayDirection = new Vector3(Mathf.Cos(angel), 0f, Mathf.Sin(angel));

            RaycastHit hit; 
            if(Physics.Raycast(transform.position, rayDirection, out hit, raycastDistance))
            {
                verticesList.Add(hit.point);
                Debug.Log(verticesList);
            }
        }
    }

    void AddThings()
        //Code based on scene seven, generating terrian with objects 
    {
        //Vector3[] verticesArray = verticesList.ToArray();
        Vector3 aThingSize = thing1Prefab.GetComponent<MeshRenderer>().bounds.size;
        Vector3 ThingSize = thing2Prefab.GetComponent<MeshRenderer>().bounds.size;
        float aThingHeight = aThingSize.y;
        float ThingHeight = ThingSize.y;
        //Vector3 mapSize = GetComponent<MeshRenderer>().bounds.size;
        //Debug.Log(mapSize);
        xWidth = tileWidth * mapWidth;
        zWidth = tileDepth * mapDepth;
        Debug.Log(xWidth);
        Debug.Log(zWidth);
     
       // mesh = GetComponent<MeshFilter>().mesh;
       
        Debug.Log(vertices.Length);
        //  Vector3[] vertices = verticesArray;
        //vertices = new Vector3[101 * 101];
        for (int xTileIndex = 0; xTileIndex < mapWidth; xTileIndex++)
        {
            for (int zTileIndex = 0; zTileIndex < mapDepth; zTileIndex++)
            {
               
               

                tileMapDataNow = tilesMapsDatas[xTileIndex, zTileIndex];
                for (int zIndex = 0; zIndex < mapDepth; zIndex++)
                {
                    for (int xIndex = 0; xIndex < mapWidth; xIndex++)
                    {
                        //Add Stuff here
                        //using the itteration code bellow, you can make items apper 
                        //That code is makeing refernaces to other forms of getting the hight data that did not work 
                        //In another itteration I will have the itteration apper here. 
                    }
                }
                //Code based on GDD316 example generation 
            }
        }

        //float[,] highMap = new float[101, 101]; // Corrected dimensions for highMap
        float[] heights = new float[vertices.Length];
        for (int k = 0; k < vertices.Length; k++)
        {
        }
        

        // Populate highMap and adjust loop bounds
        

        // Populate vertices with correct y values from highMap
        int i = 0; // Initialize vertex index
        for (int z = 0; z < vertices.Length; z++)
        {
            for (int x = 0; x < vertices.Length; x++, i++)
            {
                Debug.Log(vertices.Length);
                Vector3 vertexPosition = transform.TransformPoint(vertices[i]);
                float y = vertexPosition.y;
                heights[i] = y;
                Debug.Log(y);
                vertices[i] = new Vector3(x, y, z);
            }
        }

        mesh.vertices = vertices;

        //Read the next section carefuly, as it's nested if else loops 
        Vector3 thisPos;
        for (int j = 0, z = 0; z <= zWidth; z++)
        {
            for (int x = 0; x <= xWidth; x++, j++)
            {
                //thisPos = vertices[j];
                if (x >= 5)
                {
                    if (x == 5)
                    {
                        if (z == 10)
                        {
                            thisPos = vertices[j];
                            GameObject cccabin = Instantiate(cabin, thisPos, Quaternion.identity) as GameObject;
                            

                        }
                    }
                    if (x == 6)
                    {
                        if (z == 23)
                        {
                            thisPos = vertices[j];
                            GameObject caaabin = Instantiate(cabin2, thisPos, Quaternion.identity) as GameObject;
                        }
                    }
                    if (x == 15)
                    {
                        if (z == 17)

                        {
                            thisPos = vertices[j];
                            Vector3 Qqq = new Vector3(thisPos.x, 15, thisPos.z);
                            GameObject cabbbin = Instantiate(cabin3, Qqq, Quaternion.identity) as GameObject;
                            cabbbin.GetComponent<MeshRenderer>().material.color = new Color(150, 75, 0);
                        }
                    }
                    if (x == 25)
                    {
                        if (z == 10)
                        {
                            thisPos = vertices[j];
                            GameObject cccabin = Instantiate(cabin, thisPos, Quaternion.identity) as GameObject;
                        }
                    }
                    if (x <= 26)
                    {
                        if (z >= 11)
                        {
                            if (z <= 26)
                            {
                                Debug.Log("SKIP");
                                //Just to make sure the code knows not to fuck with the scene 
                            }
                            else
                            {
                                float W = Random.value;
                                if (W > .7f)
                                {

                                    thisPos = vertices[j];
                                    GameObject orbthing = Instantiate(thing2Prefab, thisPos, Quaternion.identity);
                                    Debug.Log(thisPos.y);                                    orbthing.transform.localScale = 0.5f * Vector3.one;
                                    orbthing.transform.position = vertices[j] + 0.45f * Vector3.up * aThingHeight / 2;
                                    orblist.Add(orbthing);

                                }
                                if (W < .2f)
                                {
                                    thisPos = vertices[j];
                                    GameObject thing = Instantiate(thing1Prefab, thisPos, Quaternion.identity);
                                    thing.transform.localScale = 0.5f * Vector3.one;
                                    thing.transform.position = vertices[j] + 0.45f * Vector3.up * aThingHeight / 2;

                                }

                                else
                                {
                                    Debug.Log("Miss");
                                }
                            }

                        }
                        else
                        {
                            float W = Random.value;
                            if (W > .7f)
                            {
                                if (j >= 0 && j < vertices.Length)
                                {
                                    thisPos = vertices[j];
                                    GameObject orbthing = Instantiate(thing2Prefab, thisPos, Quaternion.identity);
                                    orbthing.transform.localScale = 0.5f * Vector3.one;
                                    orbthing.transform.position = vertices[j] + 0.45f * Vector3.up * aThingHeight / 2;
                                    orblist.Add(orbthing);
                                }

                            }
                            if (W < .2f)
                            {
                                if (j >= 0 && j < vertices.Length)
                                {
                                    thisPos = vertices[j];
                                    GameObject thing = Instantiate(thing1Prefab, thisPos, Quaternion.identity);
                                    thing.transform.localScale = 0.5f * Vector3.one;
                                    thing.transform.position = vertices[j] + 0.45f * Vector3.up * aThingHeight / 2;
                                }

                            }

                            else
                            {
                                Debug.Log("Miss");
                            }
                        }

                    }
                    else
                    {
                        float W = Random.value;
                        if (W > .7f)
                        {
                            if (j >= 0 && j < vertices.Length)
                            {
                                thisPos = vertices[j];
                                GameObject orbthing = Instantiate(thing2Prefab, thisPos, Quaternion.identity);
                                orbthing.transform.localScale = 0.5f * Vector3.one;
                                orbthing.transform.position = vertices[j] + 0.45f * Vector3.up * aThingHeight / 2;
                                orblist.Add(orbthing);
                            }

                        }
                        if (W < .2f)
                        {
                            if (j >= 0 && j < vertices.Length)
                            {
                                thisPos = vertices[j];
                                GameObject thing = Instantiate(thing1Prefab, thisPos, Quaternion.identity);
                                thing.transform.localScale = 0.5f * Vector3.one;
                                thing.transform.position = vertices[j] + 0.45f * Vector3.up * aThingHeight / 2;
                            }

                        }

                        else
                        {
                            Debug.Log("Miss");
                        }
                    }
                }


                else
                {
                    float W = Random.value;
                    if (W > .7f)
                    {
                        if( j >= 0 && j < vertices.Length)
                        {
                            thisPos = vertices[j];
                            GameObject orbthing = Instantiate(thing2Prefab, thisPos, Quaternion.identity);
                            orbthing.transform.localScale = 0.5f * Vector3.one;
                            orbthing.transform.position = vertices[j] + 0.45f * Vector3.up * aThingHeight / 2;
                            orblist.Add(orbthing);
                        }
                       

                    }
                    if (W < .2f)
                    {
                        if ( j >= 0 && j < vertices.Length)
                        {
                            thisPos = vertices[j];
                            GameObject thing = Instantiate(thing1Prefab, thisPos, Quaternion.identity);
                            thing.transform.localScale = 0.5f * Vector3.one;
                            thing.transform.position = vertices[j] + 0.45f * Vector3.up * aThingHeight / 2;
                        }

                    }

                    else
                    {
                        Debug.Log("Miss");
                    }
                }

                }
            }
        }
    }
 



                                       