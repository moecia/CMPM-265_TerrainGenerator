using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour {

    const int mapChunkSize = 241;
    [Range(0, 6)]
    public int lod;

    public float noiseScale = 0.3f;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public enum DrawMode { NoiseMap, ColorMap, Mesh};
    public DrawMode m_drawMode;
    public TerrainType[] regions;

    [Header("UI Sliders setting")]
    public Slider s_lod;
    public Slider s_noiseScale;
    public Slider s_octaves;
    public Slider s_persistance;
    public Slider s_lacunarity;
    public Slider s_bumpiness;

    private void Start()
    {
        s_lod.value = lod;
        s_noiseScale.value = noiseScale;
        s_octaves.value = octaves;
        s_persistance.value = persistance;
        s_lacunarity.value = lacunarity;
        s_bumpiness.value = meshHeightMultiplier;
    }

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, noiseScale, seed, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        // Map noise map height to color map
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay m_display = FindObjectOfType<MapDisplay>();
        if(m_drawMode == DrawMode.NoiseMap)
            m_display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (m_drawMode == DrawMode.ColorMap)
            m_display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        else if (m_drawMode == DrawMode.Mesh)
            m_display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, lod), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
    }

    void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
    }

    public void SetLOD()
    {
        lod = (int)s_lod.value;
    }

    public void SetNoiseScale()
    {
        noiseScale = s_noiseScale.value;
    }

    public void SetOctaves()
    {
        octaves = (int)s_octaves.value;
    }

    public void SetPersistance()
    {
        persistance = s_persistance.value;
    }

    public void SetLacunarity()
    {
        lacunarity = s_lacunarity.value;
    }

    public void SetBumpiness()
    {
        meshHeightMultiplier = s_bumpiness.value;
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }

}
