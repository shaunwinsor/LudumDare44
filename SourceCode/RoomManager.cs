using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Transform startPoint;
    [SerializeField]
    GameObject roomPrefab, bossRoom;
    int maxRoomCount;
    [SerializeField]
    Transform[] roomSpawnPoints;
    public List<GameObject> roomsSpawned = new List<GameObject>();
    

    public bool AddRoom()
    {
        int roomNum = roomsSpawned.Count;
        if (roomNum >= maxRoomCount)
        {
            StartCoroutine(FindObjectOfType<UIManager>().ShowErrorMessage("No more rooms can be created!"));
            return false;
        }
        else
        {
            if (roomNum == 0 || FindObjectOfType<GameController>().SpendMoney(FindObjectOfType<GameController>().roomCost))
            {
                GameObject room = Instantiate(roomPrefab, roomSpawnPoints[roomNum].position, Quaternion.identity);
                room.transform.name = "Room";
                room.transform.parent = roomSpawnPoints[roomNum];
                roomsSpawned.Add(room);
                return true;
            }
            else
            {
                StartCoroutine(FindObjectOfType<UIManager>().ShowErrorMessage("Not enough money!"));
                return false;
            }
        }
    }

    public void MoveToNext(Hero hero)
    {
        if (hero.inRoom >= roomsSpawned.Count-1)
        {
            //Move to Boss Room
            hero.transform.position = bossRoom.transform.Find("StartPoint").position;
            hero.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            hero.SetTarget(bossRoom.transform.Find("EndPoint"));
        }
        else
        {
            hero.inRoom++;
            hero.transform.position = roomsSpawned[hero.inRoom].transform.Find("StartPoint").position;
            hero.SetTarget(roomsSpawned[hero.inRoom].transform.Find("EndPoint"));
        }
    }

    public bool Init()
    {
        maxRoomCount = roomSpawnPoints.Length;
        roomsSpawned.Clear();
        if (AddRoom())
        {
            startPoint = roomsSpawned[0].transform.Find("StartPoint");
            return true;
        }
        else
        {
            Debug.LogError("Dungeon not created!");
            return false;
        }
    }

    public void ClearRooms()
    {
        foreach(GameObject room in roomsSpawned)
        {
            Destroy(room);
        }
    }
}
