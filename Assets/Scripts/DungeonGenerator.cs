using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    RectInt baseRoom = new RectInt(0, 0, 100, 50);
    private bool splitHorizontally;
    List<RectInt> roomsToSplit = new List<RectInt>();
    List<RectInt> doneRooms = new List<RectInt>();
    public int minHeight;
    public int minWidth;
    private RectInt roomA;
    private RectInt roomB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomsToSplit.Add(baseRoom);
        StartCoroutine(SplitCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

        foreach (RectInt room in roomsToSplit)
        {
            AlgorithmsUtils.DebugRectInt(room, Color.yellow);
        }
        foreach (RectInt room in doneRooms)
        {
            AlgorithmsUtils.DebugRectInt(room, Color.green);
        }

    }

    private void SplitRoom()
    {
        if (roomsToSplit[0].width > minWidth && roomsToSplit[0].height > minHeight)
        {
            if (Random.Range(0, 2) == 0)
            {
                splitHorizontally = true;
            }
            else { splitHorizontally = false; }
            if (splitHorizontally == true)
            {
                Debug.Log("original " + roomsToSplit[0]);
                RectInt roomA = new RectInt(roomsToSplit[0].xMin, roomsToSplit[0].yMin, roomsToSplit[0].width, roomsToSplit[0].height / 2 + 1);
                Debug.Log("room A " + roomA);
                roomsToSplit.Add(roomA);
                if (roomsToSplit[0].height / 2 - 1 < roomsToSplit[0].yMin)
                {
                    RectInt roomB = new RectInt(roomsToSplit[0].xMin, roomsToSplit[0].height / 2 - 1 + roomsToSplit[0].yMin, roomsToSplit[0].width, roomsToSplit[0].height / 2 + 1);
                    roomsToSplit.Add(roomB);
                    Debug.Log("room B " + roomB);
                }
                else
                {
                    RectInt roomB = new RectInt(roomsToSplit[0].xMin, roomsToSplit[0].height / 2 - 1, roomsToSplit[0].width, roomsToSplit[0].height / 2 + 1);
                    roomsToSplit.Add(roomB);
                    Debug.Log("room B " + roomB);
                }
                roomsToSplit.Remove(roomsToSplit[0]);
            }
            if (splitHorizontally == false)
            {
                Debug.Log("original " + roomsToSplit[0]);
                RectInt roomA = new RectInt(roomsToSplit[0].xMin, roomsToSplit[0].yMin, roomsToSplit[0].width / 2 + 1, roomsToSplit[0].height);
                roomsToSplit.Add(roomA);
                Debug.Log("room A " + roomA);
                if (roomsToSplit[0].width / 2 - 1 < roomsToSplit[0].xMin)
                {
                    RectInt roomB = new RectInt(roomsToSplit[0].width / 2 - 1 + roomsToSplit[0].xMin, roomsToSplit[0].yMin, roomsToSplit[0].width / 2 + 1, roomsToSplit[0].height);
                    roomsToSplit.Add(roomB);
                    Debug.Log("room B " + roomB);
                }
                else
                {
                    RectInt roomB = new RectInt(roomsToSplit[0].width / 2 - 1, roomsToSplit[0].yMin, roomsToSplit[0].width / 2 + 1, roomsToSplit[0].height);
                    roomsToSplit.Add(roomB);
                    Debug.Log("room B " + roomB);
                }
                roomsToSplit.Remove(roomsToSplit[0]);
            }
        }
        else
        {
            RectInt doneRoom = roomsToSplit[0];
            doneRooms.Add(doneRoom);
            roomsToSplit.Remove(roomsToSplit[0]);
        }
    }

    IEnumerator SplitCoroutine()
    {
        while (roomsToSplit.Count > 0)
        {
            yield return new WaitForSeconds(0.1f);
            SplitRoom();
        }

    }
}
