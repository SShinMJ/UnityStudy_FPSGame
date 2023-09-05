using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 목적 : SpawnPoint에 Player를 위치시킨다.
// 목적2: 특정 시간이 지나면 MainManager.cs가 명령을 내린다.
// 목적3: Player의  GameManger가 명령을 받아 게임을 시작한다.

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;

    // SpawnPoint들을 담는 배열
    public Transform[] spawnPoints;

    // setSpawnPoints 수행 여부 flag
    public bool isloadPoints = false;

    // 특정 시간, 현재시간
    public float gameStartTime = 10f;

    // 게임 시작 명령 flag
    public bool isGameStarted = false;

    // 최대 참여 인원수
    public int maxHeadCount = 4;

    public void GetGameState(out bool _isGameStarted)
    {
        _isGameStarted = isGameStarted;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        // 입장 제한 시간(게임 시작 시간)이 지나거나 최대 인원이 다 차면 게임이 시작되게 한다.
        if (maxHeadCount <= 0)
        {
            isGameStarted = true;
        }
    }

    IEnumerator setSpawnCoroutine()
    {
        yield return new WaitForSeconds(1f);

        // 모든 스폰 포인트를 가져오기 위해, 스폰포인트 오브젝트가 담긴 부모를 불러와 담는다.
        Transform spawnPointsParent = GameObject.Find("SpawnPoints").transform;
        spawnPoints = new Transform[spawnPointsParent.childCount];

        for (int i = 0; i < spawnPointsParent.childCount; i++)
        {
            // 부모에 속한 모든 스폰 포인트의 위치값을 spawnPoints에 담는다.
            spawnPoints[i] = spawnPointsParent.GetChild(i);
        }

        isloadPoints = true;
    }

    public void setSpawnPoints()
    {
        StartCoroutine(setSpawnCoroutine());
    }

    // 입장 제한 시간이 지날 때까지 대기하게 한다.
    IEnumerator TimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        isGameStarted = true;
    }

    // MainGameManger의 타이머를 시작하는 함수
    public void StartTimer()
    {
        StartCoroutine(TimerCoroutine(gameStartTime));
    }
}
