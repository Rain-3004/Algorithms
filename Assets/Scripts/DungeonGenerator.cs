using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    RectInt baseRoom = new RectInt(0, 0, 200, 100);
    private bool splitHorizontally;
    List<RectInt> roomsToSplit = new List<RectInt>();
    List<RectInt> doneRooms = new List<RectInt>();
    public int minHeight;
    public int minWidth;
    private RectInt roomA;
    private RectInt roomB;

    enum WayToSplit
    {
        instant, overTime, onButtonPress
    }
    [SerializeField] private WayToSplit wayToSplit;
    void Start()
    {
        roomsToSplit.Add(baseRoom); //add the base room to the list so it can be split
        StartCoroutine(SplitCoroutine());
    }

    void Update()
    {

        foreach (RectInt room in roomsToSplit)
        {
            AlgorithmsUtils.DebugRectInt(room, Color.yellow); //drawing the rooms that still can be split in yellow
        }
        foreach (RectInt room in doneRooms)
        {
            AlgorithmsUtils.DebugRectInt(room, Color.green); //drawing the rooms that won't be split anymore in green
        }

    }

    private void SplitRoom()
    {
        if (roomsToSplit[0].width > minWidth && roomsToSplit[0].height > minHeight) //checking if the room that is going to be split is bigger then the minimum size
        {
            if (Random.Range(0, 2) == 0) //randomly splitting it either horizontally or vertically
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
            switch(wayToSplit)
            {
                case WayToSplit.instant: SplitRoom();
                    break;
                case WayToSplit.overTime:
                    yield return new WaitForSeconds(0.1f);
                    SplitRoom();
                    break;
                case WayToSplit.onButtonPress:
                    yield return new WaitUntil(()=> Input.GetKeyDown(KeyCode.Space)) ;
                    SplitRoom();
                    break;
            }

        }

    }
}
