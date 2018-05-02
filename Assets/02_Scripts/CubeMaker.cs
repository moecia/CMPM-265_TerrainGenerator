using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CubeMaker : MonoBehaviour
{
    public Vector3 size = Vector3.one;

    MeshCreator mc = new MeshCreator();

    private float timer = 0;

    public int row = 35;
    public int col = 35;
    public float sizeMultiplier = .5f;
    public float bumpiness = 1;
    public float scale = 10.0f;
    public float distance = 1.2f;

    private bool firstBuild = true;
    private Vector3 lastT2;
    private Vector3 lastT3;


    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        // one submesh for each face
        Vector3 center = new Vector3(transform.position.x, 0, transform.position.z);

        mc.Clear(); // Clear internal lists and mesh



        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                center.Set(j * size.x * (float)distance, 
                    0, 
                    i * size.z * (float)distance);

                CreateCube(center, center.x, center.z, i, j);
            }
        }

        meshFilter.mesh = mc.CreateMesh();
    }

    void CreateCube(Vector3 center, float x, float z, int row, int col)
    {
        Vector3 cubeSize = size * sizeMultiplier;
        
        float heightInc = 0;

        heightInc = .9f * bumpiness * Perlin.Noise((x + 1) / scale, (z + 1) / scale);

        if (heightInc > 3)
        {
            //for (int i = -3; i < 3; i++)
            //{
            //    for (int j = -3; j < 3; j++)
            //    {
            //        CreateCube(center, x, z, i, j);
            //    }
            //}
        }

        // top of the cube
        // t0 is top left point
        Vector3 t0 = new Vector3(center.x + cubeSize.x, center.y + heightInc * bumpiness, center.z - cubeSize.z);
        Vector3 t1 = new Vector3(center.x - cubeSize.x, center.y + heightInc * bumpiness, center.z - cubeSize.z);
        Vector3 t2 = new Vector3(center.x - cubeSize.x, center.y + heightInc * bumpiness, center.z + cubeSize.z);
        Vector3 t3 = new Vector3(center.x + cubeSize.x, center.y + heightInc * bumpiness, center.z + cubeSize.z);
        // bottom of the cube
        Vector3 b0 = new Vector3(center.x + cubeSize.x, center.y - cubeSize.y, center.z - cubeSize.z);
        Vector3 b1 = new Vector3(center.x - cubeSize.x, center.y - cubeSize.y, center.z - cubeSize.z);
        Vector3 b2 = new Vector3(center.x - cubeSize.x, center.y - cubeSize.y, center.z + cubeSize.z);
        Vector3 b3 = new Vector3(center.x + cubeSize.x, center.y - cubeSize.y, center.z + cubeSize.z);


        if (center.y + heightInc * bumpiness < 0)
        {
            t0 = new Vector3(center.x + cubeSize.x, center.y, center.z - cubeSize.z);
            t1 = new Vector3(center.x - cubeSize.x, center.y, center.z - cubeSize.z);
            t2 = new Vector3(center.x - cubeSize.x, center.y, center.z + cubeSize.z);
            t3 = new Vector3(center.x + cubeSize.x, center.y, center.z + cubeSize.z);

            b0 = new Vector3(center.x + cubeSize.x, center.y - cubeSize.y + heightInc * bumpiness, center.z - cubeSize.z);
            b1 = new Vector3(center.x - cubeSize.x, center.y - cubeSize.y + heightInc * bumpiness, center.z - cubeSize.z);
            b2 = new Vector3(center.x - cubeSize.x, center.y - cubeSize.y + heightInc * bumpiness, center.z + cubeSize.z);
            b3 = new Vector3(center.x + cubeSize.x, center.y - cubeSize.y + heightInc * bumpiness, center.z + cubeSize.z);

        }

        // Top square
        mc.BuildTriangle(t0, t1, t2);
        mc.BuildTriangle(t0, t2, t3);

        // Bottom square
        //mc.BuildTriangle(b2, b1, b0);
        //mc.BuildTriangle(b3, b2, b0);

        //// Back square
        //mc.BuildTriangle(b0, t1, t0);
        //mc.BuildTriangle(b0, b1, t1);

        //mc.BuildTriangle(b1, t2, t1);
        //mc.BuildTriangle(b1, b2, t2);

        //mc.BuildTriangle(b2, t3, t2);
        //mc.BuildTriangle(b2, b3, t3);

        //mc.BuildTriangle(b3, t0, t3);
        //mc.BuildTriangle(b3, b0, t0);
    }
}