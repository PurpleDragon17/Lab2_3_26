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
    void Start()
    {
        GenerateMap();
        CombineMeshes(gameObject);
        AddThings();
       // Debug.Log("Done");
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
                tile.transform.parent = transform;
                //Code based on GDD316 example generation 
            }
        }
        //Make the tiles apper, and in the right possitions
    }

    void CombineMeshes(GameObject parentObject)
    {
        // tileMeshes.Add(parentObject.GetComponentInChildren<MeshFilter>());
        MeshFilter[] mfs = tileMeshes.ToArray();
        //  MeshFilter[] mfs = parentObject.GetComponentInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[mfs.Length];
        for (int i = 0; i < mfs.Length; i++)
        {
            combine[i].mesh = mfs[i].sharedMesh;
            //combine[i].transform = mfs[i].transform.localToWorldMatrix;
            mfs[i].gameObject.SetActive(false);
            parentObject.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

            //Thanks to Joshua Wag for the help with this
        }
    }

    void AddThings()
        //Code based on scene seven, generating terrian with objects 
    {
       
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
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = new Vector3[(xWidth + 1) * (zWidth + 1)];
        for (int i = 0, z = 0; z <= zWidth; z++)
        {
            for (int x = 0; x <= xWidth; x++, i++)
            {
                vertices[i] = new Vector3(x, 0, z);
            }
        }
        mesh.vertices = vertices;

        //Read the next section carefuly, as it's nested if else loops 
        Vector3 thisPos;
        for (int i = 0, z = 0; z <= zWidth; z++)
        {
            for (int x = 0; x <= xWidth; x++, i++)
            {
                thisPos = vertices[i];
                if (x >= 5)
                {
                    if (x == 5)
                    {
                        if (z == 10)
                        {
                            GameObject cccabin = Instantiate(cabin, thisPos, Quaternion.identity) as GameObject;

                        }
                    }
                    if (x == 6)
                    {
                        if (z == 23)
                        {
                            GameObject caaabin = Instantiate(cabin2, thisPos, Quaternion.identity) as GameObject;
                        }
                    }
                    if (x == 15)
                    {
                        if (z == 17)
                        {
                            Vector3 Qqq = new Vector3(thisPos.x, 15, thisPos.z);
                            GameObject cabbbin = Instantiate(cabin3, Qqq, Quaternion.identity) as GameObject;
                            cabbbin.GetComponent<MeshRenderer>().material.color = new Color(150, 75, 0);
                        }
                    }
                    if (x == 25)
                    {
                        if (z == 10)
                        {
                            
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


                                    GameObject orbthing = Instantiate(thing2Prefab, thisPos, Quaternion.identity);
                                    orbthing.transform.localScale = 0.5f * Vector3.one;
                                    orbthing.transform.position = vertices[i] + 0.45f * Vector3.up * aThingHeight / 2;
                                    orblist.Add(orbthing);

                                }
                                if (W < .2f)
                                {
                                    GameObject thing = Instantiate(thing1Prefab, thisPos, Quaternion.identity);
                                    thing.transform.localScale = 0.5f * Vector3.one;
                                    thing.transform.position = vertices[i] + 0.45f * Vector3.up * aThingHeight / 2;

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


                                GameObject orbthing = Instantiate(thing2Prefab, thisPos, Quaternion.identity);
                                orbthing.transform.localScale = 0.5f * Vector3.one;
                                orbthing.transform.position = vertices[i] + 0.45f * Vector3.up * aThingHeight / 2;
                                orblist.Add(orbthing);

                            }
                            if (W < .2f)
                            {
                                GameObject thing = Instantiate(thing1Prefab, thisPos, Quaternion.identity);
                                thing.transform.localScale = 0.5f * Vector3.one;
                                thing.transform.position = vertices[i] + 0.45f * Vector3.up * aThingHeight / 2;

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


                            GameObject orbthing = Instantiate(thing2Prefab, thisPos, Quaternion.identity);
                            orbthing.transform.localScale = 0.5f * Vector3.one;
                            orbthing.transform.position = vertices[i] + 0.45f * Vector3.up * aThingHeight / 2;
                            orblist.Add(orbthing);

                        }
                        if (W < .2f)
                        {
                            GameObject thing = Instantiate(thing1Prefab, thisPos, Quaternion.identity);
                            thing.transform.localScale = 0.5f * Vector3.one;
                            thing.transform.position = vertices[i] + 0.45f * Vector3.up * aThingHeight / 2;

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
                    
                        
                            GameObject orbthing = Instantiate(thing2Prefab, thisPos, Quaternion.identity);
                            orbthing.transform.localScale = 0.5f * Vector3.one;
                            orbthing.transform.position = vertices[i] + 0.45f * Vector3.up * aThingHeight / 2;
                        orblist.Add(orbthing);

                    }
                    if (W < .2f)
                    {
                        GameObject thing = Instantiate(thing1Prefab, thisPos, Quaternion.identity);
                        thing.transform.localScale = 0.5f * Vector3.one;
                        thing.transform.position = vertices[i] + 0.45f * Vector3.up * aThingHeight / 2;

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
 



                                       