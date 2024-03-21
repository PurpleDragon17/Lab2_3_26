using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabinGen2 : MonoBehaviour
{
    //This Generates the back wall of the Cabin 
    //It's basicaly the same code as CabinGen1 
    int xWidth;
    int zWidth;
    int yWidth;
    private Renderer rend;
    private Vector3[] vertices;
    private Mesh mesh;

    void Start()
    {
        xWidth = 19;
        zWidth = 3;
        yWidth = 15;
        //These values changing and the fact that it's a differnt prefab is why this had to be a different script. 
        GenerateW2();
    }
    void GenerateW2()
    {
        int xSize = xWidth;
        int zSize = zWidth;
        int ySize = yWidth;
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        vertices = new Vector3[(xSize + 1) * (zSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                for (int y = 0; y <= ySize; y++, i++)
                {
                    vertices[i] = new Vector3(x, y, z);
                    uv[i] = new Vector2((float)x / xSize, (float)z / zSize);
                    tangents[i] = tangent;
                    GetComponent<MeshRenderer>().material.color = new Color(150, 75, 0);
                    //Debug.Log("MEOW!!!!!!!!");
                }

            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        int[] triangles = new int[xSize * zSize * ySize * 6];
        for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++)
        {
            for (int x = 0; x < xSize; x++, vi++)
            {
                for (int y = 0; y < ySize; y++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + ySize + 1;
                    triangles[ti + 5] = vi + ySize + 2;
                    mesh.triangles = triangles;
                }

            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
