using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] enemyRooms;

    public int enemyCount;
    Vector3 offset;

    Quaternion lookRot;

    private void Start()
    {
        offset = new Vector3(0, -0.5f, 0);
        StartCoroutine(SetConditions());
    }

    IEnumerator SetConditions()
    {
        yield return new WaitForSeconds(0.1f);
        enemyCount = Mathf.RoundToInt(RoomSpawner.Instance.count / 4);
        enemyRooms = new GameObject[RoomSpawner.Instance.count];

        for (int i = 1; i < enemyRooms.Length-1; i++)
        {
            enemyRooms[i] = RoomSpawner.Instance.rooms[i];

            Vector3 prevRoomPos = RoomSpawner.Instance.rPos[i - 1];
            Vector3 currentRoomPos  = RoomSpawner.Instance.rPos[i];
            Vector3 prevRoomDirection = prevRoomPos - currentRoomPos;



            lookRot = Quaternion.LookRotation(prevRoomDirection);

            if(enemyCount > 0)
            {
                int randomInt = Random.Range(1, enemyRooms.Length - 1);

                Instantiate(enemyPrefab, RoomSpawner.Instance.rPos[randomInt] + offset, lookRot);
                enemyCount--;
            }
        }
    }

}
