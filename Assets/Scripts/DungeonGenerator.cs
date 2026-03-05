using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    RectInt baseRoom = new RectInt(0, 0, 200, 100);
    List<RectInt> roomsToSplit = new();
    List<RectInt> doneRooms = new();
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
        if (roomsToSplit[0].height <= minHeight && roomsToSplit[0].width > minWidth) { splitRoomVertcally(); }
        else if (roomsToSplit[0].width <= minWidth && roomsToSplit[0].height > minHeight) { splitRoomHorizontally(); }
        else if (roomsToSplit[0].height > minHeight && roomsToSplit[0].width > minWidth)
        {
            if(Random.Range(0,2) == 0) {splitRoomHorizontally();}
            else {splitRoomVertcally(); }
        }
        else
        {
            doneRooms.Add(roomsToSplit[0]);
            roomsToSplit.Remove(roomsToSplit[0]);
        }
    }

    private void splitRoomHorizontally()
    {
        int x = Random.Range(roomsToSplit[0].yMin + minHeight, roomsToSplit[0].height - minHeight + 1);
        Debug.Log("original " + roomsToSplit[0]);
        RectInt roomA = new RectInt(roomsToSplit[0].xMin, roomsToSplit[0].yMin, roomsToSplit[0].width,roomsToSplit[0].height - x);
        Debug.Log("room A " + roomA);
        roomsToSplit.Add(roomA);
   
        RectInt roomB = new RectInt(roomsToSplit[0].xMin, roomsToSplit[0].height - x, roomsToSplit[0].width, x);
        roomsToSplit.Add(roomB);
        Debug.Log("room B " + roomB);
        roomsToSplit.Remove(roomsToSplit[0]);
    }

    private void splitRoomVertcally()
    {
        int y = Random.Range(roomsToSplit[0].xMin + minWidth, roomsToSplit[0].width - minWidth + 1);
        Debug.Log("original " + roomsToSplit[0]);
        RectInt roomA = new RectInt(roomsToSplit[0].xMin, roomsToSplit[0].yMin, roomsToSplit[0].width - y, roomsToSplit[0].height);
        roomsToSplit.Add(roomA);
        Debug.Log("room A " + roomA);
 
        RectInt roomB = new RectInt(roomsToSplit[0].width - y, roomsToSplit[0].yMin, y, roomsToSplit[0].height);
        roomsToSplit.Add(roomB);
        Debug.Log("room B " + roomB);
        roomsToSplit.Remove(roomsToSplit[0]);
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
