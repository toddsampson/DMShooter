using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public RoomContainer[,] rooms;

    public Vector2Int mapSize = new Vector2Int(5, 5);

    public Vector2 maxRoomSize = new Vector2();

    public GameObject roomPrefab;

    private bool[,] filledRooms;

    public class RoomContainer
    {
        public GameObject room;
        public Room info;
        public int slotChoice;
        public List<GameObject> enemies;

        public RoomContainer(GameObject room, int slotChoice, List<GameObject> enemies)
        {
            this.room = room;
            info = room.GetComponent<Room>();
            this.slotChoice = slotChoice;
            this.enemies = enemies;
        }
    }

    public void Start()
    {
        rooms = new RoomContainer[mapSize.x, mapSize.y];
        filledRooms = new bool[mapSize.x, mapSize.y];
        List<GameObject> e = new List<GameObject>();
        e.Add(Resources.Load<GameObject>("Ogre"));
        AddRoom(roomPrefab, new Vector2Int(0, 0), 0, e);
        AddRoom(roomPrefab, new Vector2Int(1, 0), 0, new List<GameObject>());
        AddRoom(roomPrefab, new Vector2Int(0, 1), 0, new List<GameObject>());
        string mapJSon = JSONTools.SaveMapData(rooms, mapSize);
        LoadMapFromJson(mapJSon);
        //BuildMap();
        //rooms[0, 0].info.ActivateRoom();
    }

    public void LoadMapFromJson(string mapJson)
    {
        MapData mapData = JSONTools.LoadMapData(mapJson);
        mapSize = mapData.mapSize;
        rooms = new RoomContainer[mapSize.x, mapSize.y];
        filledRooms = new bool[mapSize.x, mapSize.y];

        for (int i = 0; i < mapSize.x; i++)
        {
            for(int j = 0; j < mapSize.y; j++)
            {
                if(mapData.map[i, j].prefabName != "empty")
                {
                    AddRoom(Resources.Load<GameObject>(mapData.map[i, j].prefabName), new Vector2Int(i, j), mapData.map[i, j].slotOption, LoadEnemyPrefabs(mapData.map[i, j].enemies));
                    // LoadSlotsIntoRoom(rooms[i, j], mapData.map[i, j]);
                }
            }
        }

        BuildMap();
        rooms[0, 0].info.ActivateRoom();
    }

    public void AddRoom(GameObject room, Vector2Int location, int slotChoice, List<GameObject> enemies)
    {
        rooms[location.x, location.y] = new RoomContainer(room, slotChoice, enemies);
    }

    public void RemoveRoom(Vector2Int location)
    {
        rooms[location.x, location.y] = null;
    }

    public void BuildMap()
    {
        for(int i = 0; i < mapSize.x; i++)
        {
            for(int j = 0; j < mapSize.y; j++)
            {
                if(rooms[i, j] != null)
                {
                    GenerateRoom(rooms[i, j], i, j);
                }
            }
        }

        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                if (rooms[i, j] != null)
                {
                    ConnectNeighbors(i, j);
                }
            }
        }
    }

    private void ConnectNeighbors(int x, int y)
    {
        // Try Connect North
        if (y + 1 < mapSize.y && rooms[x, y].info.doorPoints[0] != null && rooms[x, y + 1] != null && rooms[x, y + 1].info.doorPoints[1] != null)
        {
            rooms[x, y].info.doorPoints[0].GetComponent<Teleporter>().target = rooms[x, y + 1].info.doorPoints[1];
        }

        // Try Connect South
        if (y - 1 >= 0 && rooms[x, y].info.doorPoints[1] != null && rooms[x, y - 1] != null && rooms[x, y - 1].info.doorPoints[0] != null)
        {
            rooms[x, y].info.doorPoints[1].GetComponent<Teleporter>().target = rooms[x, y - 1].info.doorPoints[0];
        }

        // Try Connect East
        if (x + 1 < mapSize.x && rooms[x, y].info.doorPoints[2] != null && rooms[x + 1, y] != null && rooms[x + 1, y].info.doorPoints[3] != null)
        {
            rooms[x, y].info.doorPoints[2].GetComponent<Teleporter>().target = rooms[x + 1, y].info.doorPoints[3];
        }

        // Try Connect West
        if (x - 1 >= 0 && rooms[x, y].info.doorPoints[3] != null && rooms[x - 1, y] != null && rooms[x - 1, y].info.doorPoints[2] != null)
        {
            rooms[x, y].info.doorPoints[3].GetComponent<Teleporter>().target = rooms[x - 1, y].info.doorPoints[2];
        }
    }

    private void GenerateRoom(RoomContainer room, int x, int y)
    {
        Vector3 gridSpot = new Vector3((maxRoomSize.x * x) + (maxRoomSize.x / 2), 0, (maxRoomSize.y * y) + (maxRoomSize.y / 2));

        var newRoom = Instantiate(room.room, gridSpot, Quaternion.identity, transform);
        room.room = newRoom;
        room.info = newRoom.GetComponent<Room>();
        room.info.selection = room.info.slotOptions[room.slotChoice];
        int i = 0;

        foreach(GameObject enemy in room.enemies)
        {
            room.info.PlaceEnemyInSlot(enemy, i);
            i++;
        }
    }

    private List<GameObject> LoadEnemyPrefabs(string[] enemies)
    {
        List<GameObject> prefabs = new List<GameObject>();

        foreach(string enemy in enemies)
        {
            prefabs.Add(Resources.Load<GameObject>(enemy));
        }

        return prefabs;
    }
}
