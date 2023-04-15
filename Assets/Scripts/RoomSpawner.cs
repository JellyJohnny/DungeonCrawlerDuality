using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using static UnityEngine.UI.GridLayoutGroup;

public class RoomSpawner : MonoBehaviour
{
    public static RoomSpawner Instance;
    public int count;
    Vector3[] dirs;
    public Vector3[] rPos;
    public Vector3 checkPos;
    Vector3 prevPos;
    bool contains;
    public GameObject entrance;
    public GameObject destination;
    public GameObject hallway;
    public GameObject[] corners;
    GameObject newRoom;
    public GameObject[] rooms;
    public int cornerCount;
    int randDirIndex;
    int[] randomDirectionsIndex;

    public GameObject playerPrefab;

    private void Start()
    {
        Instance = this;
        newRoom = hallway;
        
        //up,right,down,left
        dirs = new Vector3[4] { new Vector3(0, 0, 5), new Vector3(5, 0, 0), new Vector3(0, 0, -5), new Vector3(-5, 0, 0) };

        rooms = new GameObject[count];
        randomDirectionsIndex = new int[count];
        rPos = new Vector3[count];
        SpawnRooms();
    }

    void SpawnRooms()
    {
        for (int i = 0; i < count; i++) //for each room...
        {
            int l = 0;
            int maxLoops = 10000;
            while (CheckArray(checkPos) && l < maxLoops)//check if position is already taken by a room
            {
                l++;
                randDirIndex = Random.Range(0, dirs.Length); //choose one of 4 random directions

                checkPos = prevPos + dirs[randDirIndex]; //store the position for future use
            }

            randomDirectionsIndex[i] = randDirIndex;
            GameObject newRoomObj = Instantiate(newRoom, checkPos, Quaternion.identity); //create the room at the checked position
            rooms[i] = newRoomObj;
            rPos[i] = checkPos; //add position to array
            prevPos = checkPos;
        }

        ModifyRooms();
    }

    bool CheckArray(Vector3 p) //this function checks an array of positions and returns true or false
    {
        contains = false;
        for (int i = 0; i < rPos.Length; i++)
        {
            if (p == rPos[i])
            {
                contains = true;
            }
        }
        return contains;
    }

    void ModifyRooms() //this function's sole purpose is to change all of the rooms to make them align properly
    {
        rooms[0].SetActive(false);
        Vector3 ang = rPos[1] - rPos[0];
        Quaternion r = Quaternion.LookRotation(ang);
        GameObject d = Instantiate(entrance, rPos[0], r);

        rooms[count-1].SetActive(false);
        Vector3 ang2 = rPos[count-2] - rPos[count-1];
        Quaternion r2 = Quaternion.LookRotation(ang2);
        GameObject d2 = Instantiate(destination, rPos[count-1], r2);

        for (int i = 1; i < rPos.Length-1; i++)
        {
            Vector3 prevRoomPos = rPos[i - 1];
            Vector3 currentRoomPos = rPos[i];
            Vector3 nextRoomPos = rPos[i + 1];

            //CORNERS
            if (prevRoomPos.x != nextRoomPos.x && prevRoomPos.z != nextRoomPos.z
                && i != 0 && i != rPos.Length-1)
            {
                rooms[i].SetActive(false);

                switch (randomDirectionsIndex[i + 1])
                {
                    case 0: //forward

                        if(currentRoomPos.x > prevRoomPos.x)
                        {
                            GameObject corn = Instantiate(corners[0], currentRoomPos, corners[0].transform.rotation);
                            rPos[i] = corn.transform.position;
                            rooms[i] = corn;
                        }
                        else
                        {
                            GameObject corn = Instantiate(corners[1], currentRoomPos, corners[1].transform.rotation);
                            rPos[i] = corn.transform.position;
                            rooms[i] = corn;
                        }
                        

                        break;
                    case 1: //right

                        if (currentRoomPos.z > prevRoomPos.z)
                        {
                            GameObject corn2 = Instantiate(corners[2], currentRoomPos, corners[2].transform.rotation);
                            rPos[i] = corn2.transform.position;
                            rooms[i] = corn2;
                        }
                        else
                        {
                            GameObject corn2 = Instantiate(corners[1], currentRoomPos, corners[1].transform.rotation);
                            rPos[i] = corn2.transform.position;
                            rooms[i] = corn2;
                        }

                        break;
                    case 2: //backward

                        if(currentRoomPos.x > prevRoomPos.x)
                        {
                            GameObject corn3 = Instantiate(corners[3], currentRoomPos, corners[3].transform.rotation);
                            rPos[i] = corn3.transform.position;
                            rooms[i] = corn3;
                        }
                        else
                        {
                            GameObject corn3 = Instantiate(corners[2], currentRoomPos, corners[2].transform.rotation);
                            rPos[i] = corn3.transform.position;
                            rooms[i] = corn3;
                        }
                        

                        break;
                    case 3: //left

                        if (currentRoomPos.z > prevRoomPos.z)
                        {
                            GameObject corn4 = Instantiate(corners[3], currentRoomPos, corners[3].transform.rotation);
                            rPos[i] = corn4.transform.position;
                            rooms[i] = corn4;
                        }
                        else
                        {
                            GameObject corn5 = Instantiate(corners[0], currentRoomPos, corners[0].transform.rotation);
                            rPos[i] = corn5.transform.position;
                            rooms[i] = corn5;
                        }
                        break;
                }
            }
            else
            {
                Vector3 ang3 = rPos[i + 1] - rPos[i];
                Quaternion r3 = Quaternion.LookRotation(ang3);
                rooms[i].transform.rotation = r3;
            }
        }
        SpawnPlayer(rPos[0],r); //spawn the player at the first room
    }

    void SpawnPlayer(Vector3 p,Quaternion q)
    {
        Vector3 offset = new Vector3(0,-0.5f,0); 
        GameObject player = Instantiate(playerPrefab,p + offset,q);
    }
}
