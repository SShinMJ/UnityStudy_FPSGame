using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ���� : ������ ���¸� �����ϰ�, ������ ���۰� ���� Text UI�� ǥ��.
//       'Ready'���¿��� 2�� �� 'Start' ���·� ����Ǹ� ���� ����.
//       'Ready'���¿��� �÷��̾�, �� ��� ������ �� ����.

// ����2: player�� hp�� 0���� �۰ų� ������, 'GameOver' ���·� �ٲٰ�, �ؽ�Ʈ�� ���̰� �Ѵ�.
public class GameManager : MonoBehaviour
{
    // �̱��� ����
    public static GameManager Instance;

    // ���� ����
    public enum GameStatus
    {
        Ready,
        Start,
        GameOver
    }

    public GameStatus status = GameStatus.Ready;

    // Text UI
    public TMP_Text gameStatusTxt;

    // hp�� ����ִ� playerMove
    PlayerMovement player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        gameStatusTxt.text = "Ready";
        gameStatusTxt.color = new Color(255, 185, 0, 255);

        StartCoroutine(GameStart());

        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    IEnumerator GameStart()
    {
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
            // �ؽ�Ʈ ���̱�
            gameStatusTxt.gameObject.SetActive(true);
            // �ؽ�Ʈ ����
            gameStatusTxt.text = "Game Over";
            // ������ �ؽ�Ʈ
            gameStatusTxt.color = new Color(255, 0, 0, 255);

            // ���� ���� ����
            status = GameStatus.GameOver;
        }
    }

    void Update()
    {
        CheckGameOver();
    }
}
