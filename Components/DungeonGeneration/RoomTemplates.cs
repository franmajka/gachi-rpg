using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] rightRooms;
    public GameObject[] leftRooms;
    public GameObject closedRoom;
    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedPilon;
    public GameObject pilon;

    private void Update()
    {
        if (waitTime <= 0 && !spawnedPilon)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Instantiate(pilon, rooms[i].transform.position, Quaternion.identity);
                    spawnedPilon = true;
                }
            }
        } else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
