using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TerrainGenerator : MonoBehaviour {

    public int depth = 20;

    public int width = 256;
    public int height = 256;

    public float scale = 20.0f;

    [Header("UI Sliders setting")]
    public Slider s_bumpiness;
    public Slider s_noiseScale;

    // Use this for initialization
    void Start()
    {
        Terrain m_terrain = transform.GetComponent<Terrain>();
        m_terrain.terrainData = GenerateTerrain(m_terrain.terrainData);

        s_bumpiness.value = depth;
        s_noiseScale.value = scale;
    }

    // Update is called once per frame
    void Update() {
        Terrain m_terrain = transform.GetComponent<Terrain>();
        m_terrain.terrainData = GenerateTerrain(m_terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeight());

        return terrainData;
    }

    float[,] GenerateHeight()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < width; y++)
                heights[x, y] = CalculateHeight(x, y);

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCood = (float)x / width * scale;
        float yCood = (float)y / height * scale;

        return Mathf.PerlinNoise(xCood, yCood);
    }

    public void SetBumpiness()
    {
        depth = (int)s_bumpiness.value;
    }

    public void SetNoiseScale()
    {
        scale = s_noiseScale.value;
    }

}
