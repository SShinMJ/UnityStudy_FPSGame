using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���� : ������ ���¸� �����ϰ�, ������ ���۰� ���� Text UI�� ǥ��.
//       'Ready'���¿��� 2�� �� 'Start' ���·� ����Ǹ� ���� ����.
//       'Ready'���¿��� �÷��̾�, �� ��� ������ �� ����.

// ����2: player�� hp�� 0���� �۰ų� ������, 'GameOver' ���·� �ٲٰ�, �ؽ�Ʈ�� ���̰� �Ѵ�.

// ����3: �÷��̾��� hp�� 0 ���϶�� �ִϸ��̼��� �����.

// ����4: Setting ��ư�� ������ Option UI�� ������. ���ÿ� ���� �ӵ��� �����Ѵ�.(0 or 1)
// ����5: ���� ���� �� Retry, Exit ��ư�� Ȱ��ȭ.

// ����6: ������ ���۵Ǹ� Photon Network�� Player�� ����
// ����7: Clent�μ� ������ �Ǹ�(GameManager�� �����Ǹ�),
//        PhotonNetwork�� ������ �÷��̾���� Ȯ���ؼ� �� ��ȣ�� �����Ѵ�.
public class GameManager : MonoBehaviour
{
    // �̱��� ����
    public static GameManager Instance;

    // ���� ���� (4. �Ͻ����� �߰�)
    public enum GameStatus
    {
        Ready,
        Start,
        Pause,
        GameOver
    }

    public GameStatus status = GameStatus.Ready;

    // Text UI
    public TMP_Text gameStatusTxt;

    // hp�� ����ִ� playerMove
    PlayerMovement player;

    Animator animator;

    // 4. OptionUI ���� ������Ʈ
    public GameObject optionUI;

    // 6. Player prefab PhotonView
    public PhotonView playerPrefab;
    // �÷��̾� ��
    public int myPlayerNum = 0;

    // ���� ���� �� Ȱ��ȭ�� �� �׷�
    public GameObject groupEnemies;

    // SpawnPoint���� ��� �迭
    public Transform[] spawnPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // 7. �÷��̾� �� Ȯ��
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player p in players)
        {
            // LocalPlayer == �� �̹Ƿ� ���� �ƴ϶��
            if (p != PhotonNetwork.LocalPlayer)
            {
                myPlayerNum++;
            }
        }
    }

    void Start()
    {
        gameStatusTxt.text = "Ready";
        gameStatusTxt.color = new Color(255, 185, 0, 255);

        StartCoroutine(GameStart());

        animator = GetComponentInChildren<Animator>();
    }

    bool isGameStarted = false;
    IEnumerator GameStart()
    {
        // �濡 �ִ� �������� ��Ʈ��ũ���� Ȯ���ϸ�
        yield return new WaitUntil(() => PhotonNetwork.InRoom);

        // �÷��̾�, �� ������Ʈ ����
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[myPlayerNum].position, Quaternion.identity);

        player = playerObj.GetComponent<PlayerMovement>();
        animator = player.GetComponent<Animator>();

        groupEnemies.SetActive(true);

        GameObject[] enemyCanvasList = GameObject.FindGameObjectsWithTag("Enemy Canvas");
        foreach(var canvas in enemyCanvasList)
        {
            canvas.GetComponent<HpBarTarget>().target = playerObj.transform;
        }

        // 2�� ���
        yield return new WaitForSeconds(2);

        // start�� ����
        gameStatusTxt.text = "Game Start";
        gameStatusTxt.color = new Color(0, 255, 0, 255);

        // 0.5�� ���
        yield return new WaitForSeconds(0.5f);

        // �ؽ�Ʈ ������
        gameStatusTxt.gameObject.SetActive(false);

        // ���� ����
        status = GameStatus.Start;
    }

    void CheckGameOver()
    {
        if(player.playerHp <= 0)
        {
            // �ִϸ��̼� ���߱�
            animator.SetFloat("MoveMotion", 0f);

            // �ؽ�Ʈ ���̱�
            gameStatusTxt.gameObject.SetActive(true);
            // �ؽ�Ʈ ����
            gameStatusTxt.text = "Game Over";
            // ������ �ؽ�Ʈ
            gameStatusTxt.color = new Color(255, 0, 0, 255);

            // Retry, Exit ��ư Ȱ��ȭ
            GameObject retryBtn = gameStatusTxt.transform.GetChild(0).gameObject;
            retryBtn.SetActive(true);
            GameObject exitBtn = gameStatusTxt.transform.GetChild(1).gameObject;
            exitBtn.SetActive(true);

            // HP�ٿ� Weapon Mode Text�� ��Ȱ��ȭ.
            player.hpSlider.gameObject.SetActive(false);
            player.GetComponent<PlayerFire>().weaponModeTxt.gameObject.SetActive(false);

            // ���� ���� ����
            status = GameStatus.GameOver;
        }
    }

    void Update()
    {
        if(player != null)
            CheckGameOver();
    }

    internal void SetActive(bool v)
    {
        throw new NotImplementedException();
    }

    // 4. optionUI �ѱ�
    public void OpenOptionWindow()
    {
        // UI â ����
        optionUI.SetActive(true);

        // ���� �ð� �帣�� �� ���߱�
        Time.timeScale = 0;

        // ���� ���·� ����
        status = GameStatus.Pause;
    }
    // ����ϱ�(Continue)
    public void CloseOptionWindow()
    {
        optionUI.SetActive(false);

        Time.timeScale = 1;

        status = GameStatus.Start;
    }
    // �ٽ��ϱ�(Retry)
    public void RestartGame()
    {
        Time.timeScale = 1;

        // ���� �� ��ȣ�� �ٽ� �ε��Ѵ�. GetActiveScene().buildIndex: ���� ����ִ� ���� �����´�.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // ������(Exit)
    public void QuitGame()
    {
        Application.Quit();
    }
}
