using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEditor;

public class DungeonGenerator : MonoBehaviour
{
    RectInt baseRoom = new RectInt(0, 0, 200, 100);
    List<RectInt> roomsToSplit = new();
    List<RectInt> doneRooms = new();
    public int minHeight;
    public int minWidth;

    private RectInt? currentRoom = null;
    private RectInt? splitRoomA = null;
    private RectInt? splitRoomB = null;
    private RectInt roomA;
    private RectInt roomB; 
    private int x;
    private int y;
    private int Xmin;
    private int Xmax;
    private int Ymin;
    private int Ymax;

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

        if (currentRoom != null)  AlgorithmsUtils.DebugRectInt(currentRoom.Value, Color.cyan);
       // AlgorithmsUtils.DebugRectInt(roomsToSplit[0], Color.cyan);
        if (splitRoomA != null) AlgorithmsUtils.DebugRectInt(splitRoomA.Value, Color.magenta);
        if (splitRoomB != null) AlgorithmsUtils.DebugRectInt(splitRoomB.Value, Color.red);

    }

    private IEnumerator SplitRoom()
    {
        if (roomsToSplit[0].height <= minHeight && roomsToSplit[0].width > minWidth) { yield return splitRoomVertcally(); }
        else if (roomsToSplit[0].width <= minWidth && roomsToSplit[0].height > minHeight) { yield return splitRoomHorizontally(); }
        else if (roomsToSplit[0].height > minHeight && roomsToSplit[0].width > minWidth)
        {
            if(Random.Range(0,2) == 0) {yield return splitRoomHorizontally();}
            else {yield return splitRoomVertcally(); }
        }
        else
        {
  
            doneRooms.Add(roomsToSplit[0]);
            roomsToSplit.Remove(roomsToSplit[0]);

        }
    }

    private IEnumerator splitRoomHorizontally()
    {
        yield return null;
        Xmin = roomsToSplit[0].yMin + minHeight;
        Xmax = roomsToSplit[0].height + roomsToSplit[0].yMin - minHeight + 1;
        if(Xmax > Xmin)
        {
            x = Random.Range(Xmin, Xmax);
        }
        else if(Xmax == Xmin)
        {
            x = Xmin;
        }
        Debug.Log("spliting original horizontally " + roomsToSplit[0]);

        currentRoom = roomsToSplit[0];

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        roomA = new RectInt(roomsToSplit[0].xMin, roomsToSplit[0].yMin, roomsToSplit[0].width, roomsToSplit[0].height - x);
        Debug.Log("room A " + roomA);
        roomsToSplit.Add(roomA);
        splitRoomA = roomA;

        roomB = new RectInt(roomsToSplit[0].xMin, roomA.yMin + roomA.height, roomsToSplit[0].width, x);
        roomsToSplit.Add(roomB);
        Debug.Log("room B " + roomB);
        roomsToSplit.Remove(roomsToSplit[0]);
        splitRoomB = roomB;
        currentRoom = null;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        splitRoomA = null;
        splitRoomB = null;
    }

    private IEnumerator splitRoomVertcally()
    {
        yield return null;
        Ymin = roomsToSplit[0].xMin + minWidth;
        Ymax = roomsToSplit[0].width + roomsToSplit[0].xMin - minWidth + 1;
        if (Ymax > Ymin)
        {
            y = Random.Range(Ymin, Ymax);
        }
        else if (Ymax == Ymin) { y = Ymin; }

    Debug.Log("splitting original vertcally " + roomsToSplit[0]);
        currentRoom = roomsToSplit[0];

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        roomA = new RectInt(roomsToSplit[0].xMin, roomsToSplit[0].yMin, roomsToSplit[0].width - y, roomsToSplit[0].height);
        roomsToSplit.Add(roomA);
        Debug.Log("room A " + roomA);
        splitRoomA = roomA;

        roomB = new RectInt(roomA.xMin + roomA.width, roomsToSplit[0].yMin, y, roomsToSplit[0].height);
        roomsToSplit.Add(roomB);
        Debug.Log("room B " + roomB);
        roomsToSplit.Remove(roomsToSplit[0]);

        splitRoomB = roomB;
        currentRoom = null;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        splitRoomA = null;
        splitRoomB = null;

    }
    IEnumerator SplitCoroutine()
    {
        while (roomsToSplit.Count > 0)
        {
            switch(wayToSplit)
            {
                case WayToSplit.overTime:
                    yield return new WaitForSeconds(0.1f);
                    break;
                case WayToSplit.onButtonPress:
                    yield return new WaitUntil(()=> Input.GetKeyDown(KeyCode.Space)) ;
                    yield return null;
                    break;
            }
            yield return SplitRoom();
        }

    }
}
