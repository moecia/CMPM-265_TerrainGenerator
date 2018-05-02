using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    public override void OnInspectorGUI()
    {
        MapGenerator m_mapGenerator = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (m_mapGenerator.autoUpdate)
                m_mapGenerator.GenerateMap();
        }

        if (GUILayout.Button("Generate"))
            m_mapGenerator.GenerateMap();
    }
}
