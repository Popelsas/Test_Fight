using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Transform plane;
    public GameObject playerPrefab;
    public float planeSize = 25;
    public int playerCount = 25;
    public Transform curCamera;
    public List<GameObject> players;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Фиксируем камеру на приемлимом расстоянии
        curCamera.position = new Vector3(0, planeSize * 10, 0);
        //Создаем площадку
        InstantiatePlane();
        //Создаем игоков
        InstantiatePlayers();
    }

    private void InstantiatePlayers()
    {
        for (int i = 0; i < playerCount; i++)
        {
            GameObject pl = GameObject.Instantiate(playerPrefab);
            players.Add(pl);
            var min = planeSize * -5 + 10;
            var max = planeSize * 5 - 10;
            pl.transform.position = new Vector3(Random.Range(min, max), 5, Random.Range(min, max));
        }
    }

    private void InstantiatePlane()
    {
        GameObject planeStart = GameObject.Instantiate(plane.gameObject);
        planeStart.transform.position = Vector3.zero;
        planeStart.transform.localScale = new Vector3(planeSize, 1, planeSize);
    }
}
