using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]
public class MapSaver : MonoBehaviour
{
    [Header("Level Prefabs")]
    #region 
    public GameObject roadPrefab;
    public GameObject road2Prefab;
    public GameObject road3Prefab;
    public GameObject jumpObstacle;
    public GameObject walkableObstacle;
    public GameObject doorObstacle;
    public GameObject enemyPrefab;
    public GameObject coinPrefab;
    public GameObject boostPrefab;
    public GameObject fishPrefab;
    public GameObject toxicPrefab;
    public GameObject waterPrefab;
    public GameObject deadlyPrefab;
    public GameObject dogObstacle;
    #endregion

    [Header("Map Editor")]
    public int level;


    //Unity
    string path;
    int mapIndex;
    IFormatter formatter;
    MapObject mapObject;
    List<MapObject> mapObjects;



    [ExecuteInEditMode]
    [Serializable]
    public class MapObject
    {
        public int ID;
        public int objectCount;
        public List<Map_GameObject> gameObjects = new List<Map_GameObject>();
    }

    [ExecuteInEditMode]
    [Serializable]
    public class Map_GameObject
    {
        public float[] _position = new float[3];
        public string _tag;
    }

    [Obsolete]
    private void Awake()
    {
        //Streaming Assets folder is read only. A file will be written down on %appdata%/local low/project folder,
        //at the end of the level design section, you can copy it to streaming assets folder.
        FindPath();
        formatter = new BinaryFormatter();
    }

    private void FindPath()
    {
        string tempPath = System.IO.Path.Combine(Application.streamingAssetsPath, "map.levels");
        string filePath;
        
        UnityWebRequest www = UnityWebRequest.Get(tempPath);
        www.SendWebRequest();

        while (!www.isDone) { }
        filePath = Application.persistentDataPath + "/map.levels";
        System.IO.File.WriteAllBytes(filePath, www.downloadHandler.data);

        path = filePath;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveMap();

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadMap(0);
        }
    }

    public void SaveMap(bool updateMap = false)
    {
        BringMaps(out mapIndex, out formatter, out mapObject, out mapObjects);

        Transform[] objectTransforms = GameObject.FindObjectsOfType<Transform>();


        if (!updateMap)
        {
            if (mapObjects.Count > 0)
            {
                mapIndex = mapObjects.LastOrDefault().ID + 1;

                Debug.Log("MapNo: " + mapIndex + " SAVED.");
            }
            else
            {
                mapIndex = 1;
                Debug.Log("MapNo: " + mapIndex + " SAVED.");
            }
        }


        mapObject = new MapObject();
        mapObjects = new List<MapObject>();

        if (updateMap)
        {
            mapIndex = level;
        }

        foreach (Transform trns in objectTransforms)
        {
            mapObject.ID = mapIndex;

            Map_GameObject obj = new Map_GameObject();
            //we save the position and tag of the object. We are going to decide which prefab to create by its tag.
            obj._position[0] = trns.position.x;
            obj._position[1] = trns.position.y;
            obj._position[2] = trns.position.z;
            obj._tag = trns.tag;

            mapObject.gameObjects.Add(obj);
            mapObject.objectCount++;

            using (Stream w = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                formatter.Serialize(w, mapObject);
            }
        }


    }

    public void showMaps()
    {
        BringMaps(out mapIndex, out formatter, out mapObject, out mapObjects);

        foreach (MapObject map in mapObjects)
        {
            Debug.Log("Map:" + map.ID);
        }
    }

    public int mapCount()
    {
        FindPath();
        BringMaps(out mapIndex, out formatter, out mapObject, out mapObjects);
        int mapCount = 0;
        foreach (MapObject map in mapObjects)
        {
            mapCount++;
        }
        SaveManager.instance.SetLevelCount(mapCount);
        return mapCount;
    }

    public void LoadMap(int mapIndex)
    {
        formatter = new BinaryFormatter();
        mapObject = new MapObject();
        MapObject mapObjectToCreate = new MapObject();

        using (Stream w = new FileStream(path, FileMode.Open))
        {
            while (w.Position < w.Length)
            {
                mapObject = (MapObject)formatter.Deserialize(w);
                if (mapObject.ID == mapIndex)
                    mapObjectToCreate = mapObject;
            }
        }

        if (mapObjectToCreate != null)
            CreateMap(mapObjectToCreate);

    }
    public void DeleteMap(int mapIndex)
    {
        IFormatter formatter = new BinaryFormatter();
        MapObject mapObject = new MapObject();
        List<MapObject> mapObjects = new List<MapObject>();
        using (Stream w = new FileStream(path, FileMode.Open))
        {
            while (w.Position < w.Length)
            {
                mapObject = (MapObject)formatter.Deserialize(w);
                if (mapIndex != mapObject.ID)
                    mapObjects.Add(mapObject);
            }
        }


        using (Stream w = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            foreach (MapObject map in mapObjects)
            {
                formatter.Serialize(w, map);
            }

        }
    }

    public void UpdateMap(int mapIndex)
    {
        DeleteMap(mapIndex);
        SaveMap(true);

    }

    public void LoadMapInEditor(int mapIndex)
    {
        IFormatter formatter = new BinaryFormatter();
        GameObject[] deleteThese = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in deleteThese)
        {
            if (go.transform.parent == null && go.name != "Core") // destroy everything except core and objects in it.
            {
                DestroyImmediate(go);
            }
        }

        LoadMap(mapIndex);

    }

    private void CreateMap(MapObject mapObject)
    {
        Map_GameObject[] gameObjects = mapObject.gameObjects.ToArray();

        //spawn saved objects according to their saved positions.
        foreach (Map_GameObject obj in gameObjects)
        {
            if (obj._tag == "Road1")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(roadPrefab, pos, roadPrefab.transform.rotation);
            }
            if (obj._tag == "Road2")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(road2Prefab, pos, road2Prefab.transform.rotation);
            }
            if (obj._tag == "Road3")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(road3Prefab, pos, road3Prefab.transform.rotation);
            }

            if (obj._tag == "JumpObstacle")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(jumpObstacle, pos, jumpObstacle.transform.rotation);
            }

            if (obj._tag == "WalkableObstacle")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(walkableObstacle, pos, walkableObstacle.transform.rotation);
            }

            if (obj._tag == "DoorObstacle")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(doorObstacle, pos, doorObstacle.transform.rotation);
            }

            if (obj._tag == "Enemy")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(enemyPrefab, pos, Quaternion.identity);
            }

            if (obj._tag == "Coin")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(coinPrefab, pos, coinPrefab.transform.rotation);
            }

            if (obj._tag == "Boost")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(boostPrefab, pos, boostPrefab.transform.rotation);
            }

            if (obj._tag == "Fish")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(fishPrefab, pos, fishPrefab.transform.rotation);
            }

            if (obj._tag == "ToxicObstacle")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(toxicPrefab, pos, toxicPrefab.transform.rotation);
            }

            if (obj._tag == "WaterObstacle")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(waterPrefab, pos, waterPrefab.transform.rotation);
            }
            if (obj._tag == "DeadlyObstacle")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(deadlyPrefab, pos, deadlyPrefab.transform.rotation);
            }
            if (obj._tag == "DogObstacle")
            {
                Vector3 pos = new Vector3(obj._position[0], obj._position[1], obj._position[2]);
                Instantiate(dogObstacle, pos, dogObstacle.transform.rotation);
            }
        }
    }


    private void BringMaps(out int mapIndex, out IFormatter formatter, out MapObject mapObject, out List<MapObject> mapObjects)
    {
        mapIndex = 0;
        formatter = new BinaryFormatter();
        mapObject = new MapObject();
        mapObjects = new List<MapObject>();
        MapObject mapObjectHolder = mapObject;
        mapObjectHolder.ID = -1;
        using (Stream w = new FileStream(path, FileMode.OpenOrCreate))
        {
            while (w.Position < w.Length)
            {
                mapObject = (MapObject)formatter.Deserialize(w);

                if (mapObjectHolder != null && mapObject.ID != mapObjectHolder.ID)
                    mapObjects.Add(mapObject);

                mapObjectHolder = mapObject;
            }
        }
    }
}



