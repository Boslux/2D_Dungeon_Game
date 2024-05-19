using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Corner Prefabs")]
    public GameObject topLeftCornerPrefab;
    public GameObject topRightCornerPrefab;
    public GameObject bottomLeftCornerPrefab;
    public GameObject bottomRightCornerPrefab;

    [Header("Wall Prefabs")]
    public GameObject topWallPrefab;
    public GameObject bottomWallPrefab;
    public GameObject leftWallPrefab;
    public GameObject rightWallPrefab;

    [Header("Floor Prefab")]
    public GameObject floorPrefab;

    [Header("Door Prefab")]
    public GameObject doorPrefab;

    [Header("Spawner Prefab")]
    public GameObject spawnerPrefab;

    [Header("Room Settings")]
    public int minRoomWidth = 8; // Odanın minimum genişliği
    public int maxRoomWidth = 12; // Odanın maksimum genişliği
    public int minRoomHeight = 8; // Odanın minimum yüksekliği
    public int maxRoomHeight = 12; // Odanın maksimum yüksekliği

    private GameObject currentRoom;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); // Oyuncu objesini bul
        CreateRoom(Vector2.zero);
    }

    void CreateRoom(Vector2 position)
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom);
        }

        currentRoom = new GameObject("Room");

        int roomWidth = Random.Range(minRoomWidth, maxRoomWidth);
        int roomHeight = Random.Range(minRoomHeight, maxRoomHeight);

        // Zemin spritelarını yerleştir
        for (int x = -1; x < roomWidth; x++)
        {
            for (int y = -1; y < roomHeight; y++)
            {
                Instantiate(floorPrefab, position + new Vector2(x, y), Quaternion.identity, currentRoom.transform);
            }
        }

        // Köşe duvar spritelarını yerleştir
        Instantiate(topLeftCornerPrefab, position + new Vector2(-1, roomHeight - 1), Quaternion.identity, currentRoom.transform);
        Instantiate(topRightCornerPrefab, position + new Vector2(roomWidth - 1, roomHeight - 1), Quaternion.identity, currentRoom.transform);
        Instantiate(bottomLeftCornerPrefab, position + new Vector2(-1, -1), Quaternion.identity, currentRoom.transform);
        Instantiate(bottomRightCornerPrefab, position + new Vector2(roomWidth - 1, -1), Quaternion.identity, currentRoom.transform);

        // Üst ve alt duvar spritelarını yerleştir
        for (int x = 0; x < roomWidth - 1; x++)
        {
            Instantiate(topWallPrefab, position + new Vector2(x, roomHeight - 1), Quaternion.identity, currentRoom.transform);
            Instantiate(bottomWallPrefab, position + new Vector2(x, -1), Quaternion.identity, currentRoom.transform);
        }

        // Sol ve sağ duvar spritelarını yerleştir
        for (int y = 0; y < roomHeight - 1; y++)
        {
            Instantiate(leftWallPrefab, position + new Vector2(-1, y), Quaternion.identity, currentRoom.transform);
            Instantiate(rightWallPrefab, position + new Vector2(roomWidth - 1, y), Quaternion.identity, currentRoom.transform);
        }

        // Kapıyı yerleştir (odanın ortasında rastgele bir konumda)
        Vector2 doorPosition = position + new Vector2(Random.Range(1, roomWidth - 2), Random.Range(1, roomHeight - 2));
        StartCoroutine(CreateDoor(doorPosition, Vector2.zero));

        // Spawner yetleştir (odanın ortasına rastgele bir konumda)
        Vector2 spawnerPosition = position + new Vector2(Random.Range(1, roomWidth - 2), Random.Range(1, roomHeight - 2));
        CreateSpawner(spawnerPosition, Vector2.zero);

        // Oyuncuyu odanın ortasına ışınla
        if (player != null)
        {
            player.transform.position = position + new Vector2(roomWidth / 2, roomHeight / 2);
        }
    }

    void CreateSpawner(Vector2 position, Vector2 direction)
    {
        GameObject spawner = Instantiate(spawnerPrefab, position, Quaternion.identity, currentRoom.transform);
        EnemySpawner spawnerScript = spawner.GetComponent<EnemySpawner>();
        spawnerScript.Initialize(player); // Oyuncu referansını geçir
    }

    IEnumerator CreateDoor(Vector2 position, Vector2 direction)
    {
        yield return new WaitForSeconds(25);
        GameObject door = Instantiate(doorPrefab, position, Quaternion.identity, currentRoom.transform);
        DoorController doorScript = door.GetComponent<DoorController>();
        doorScript.Initialize(this, direction);
    }

    public void OnDoorTriggered()
    {
        Vector2 newPosition = currentRoom.transform.position;
        newPosition += new Vector2(Random.Range(-maxRoomWidth, maxRoomWidth), Random.Range(-maxRoomHeight, maxRoomHeight));
        CreateRoom(newPosition);
    }
}
