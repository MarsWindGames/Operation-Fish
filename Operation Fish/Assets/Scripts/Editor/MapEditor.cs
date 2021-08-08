using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MapSaver))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapSaver mapSaver = (MapSaver)target;
        if (GUILayout.Button("Save Level (Auto Increment)"))
        {
            mapSaver.SaveMap();
        }

        if (GUILayout.Button("Load Level (Enter ID)"))
        {
            mapSaver.LoadMapInEditor(mapSaver.level);
        }

        if (GUILayout.Button("Update Level (Enter ID)"))
        {
            mapSaver.UpdateMap(mapSaver.level);
        }

        if (GUILayout.Button("Delete Level (Enter ID)"))
        {
            mapSaver.DeleteMap(mapSaver.level);
        }

        if (GUILayout.Button("Show Maps"))
        {
            mapSaver.showMaps();
        }
    }
}
