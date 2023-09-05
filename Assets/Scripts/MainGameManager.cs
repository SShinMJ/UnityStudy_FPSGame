using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� : SpawnPoint�� Player�� ��ġ��Ų��.
// ����2: Ư�� �ð��� ������ MainManager.cs�� ����� ������.
// ����3: Player��  GameManger�� ����� �޾� ������ �����Ѵ�.

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;

    // SpawnPoint���� ��� �迭
    public Transform[] spawnPoints;

    // setSpawnPoints ���� ���� flag
    public bool isloadPoints = false;

    // Ư�� �ð�, ����ð�
    public float gameStartTime = 10f;

    // ���� ���� ��� flag
    public bool isGameStarted = false;

    // �ִ� ���� �ο���
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
        // ���� ���� �ð�(���� ���� �ð�)�� �����ų� �ִ� �ο��� �� ���� ������ ���۵ǰ� �Ѵ�.
        if (maxHeadCount <= 0)
        {
            isGameStarted = true;
        }
    }

    IEnumerator setSpawnCoroutine()
    {
        yield return new WaitForSeconds(1f);

        // ��� ���� ����Ʈ�� �������� ����, ��������Ʈ ������Ʈ�� ��� �θ� �ҷ��� ��´�.
        Transform spawnPointsParent = GameObject.Find("SpawnPoints").transform;
        spawnPoints = new Transform[spawnPointsParent.childCount];

        for (int i = 0; i < spawnPointsParent.childCount; i++)
        {
            // �θ� ���� ��� ���� ����Ʈ�� ��ġ���� spawnPoints�� ��´�.
            spawnPoints[i] = spawnPointsParent.GetChild(i);
        }

        isloadPoints = true;
    }

    public void setSpawnPoints()
    {
        StartCoroutine(setSpawnCoroutine());
    }

    // ���� ���� �ð��� ���� ������ ����ϰ� �Ѵ�.
    IEnumerator TimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        isGameStarted = true;
    }

    // MainGameManger�� Ÿ�̸Ӹ� �����ϴ� �Լ�
    public void StartTimer()
    {
        StartCoroutine(TimerCoroutine(gameStartTime));
    }
}
