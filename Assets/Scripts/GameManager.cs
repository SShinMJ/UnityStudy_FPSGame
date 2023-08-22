using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 목적 : 게임의 상태를 구별하고, 게임의 시작과 끝을 Text UI로 표현.
//       'Ready'상태에서 2초 후 'Start' 상태로 변경되며 게임 시작.
//       'Ready'상태에는 플레이어, 적 모두 움직일 수 없다.

// 목적2: player의 hp가 0보다 작거나 같으면, 'GameOver' 상태로 바꾸고, 텍스트를 보이게 한다.
public class GameManager : MonoBehaviour
{
    // 싱글톤 선엉
    public static GameManager Instance;

    // 게임 상태
    public enum GameStatus
    {
        Ready,
        Start,
        GameOver
    }

    public GameStatus status = GameStatus.Ready;

    // Text UI
    public TMP_Text gameStatusTxt;

    // hp가 들어있는 playerMove
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
        // 2초 대기
        yield return new WaitForSeconds(2);

        // start로 변경
        gameStatusTxt.text = "Game Start";
        gameStatusTxt.color = new Color(0, 255, 0, 255);

        // 0.5초 대기
        yield return new WaitForSeconds(0.5f);

        // 텍스트 내리기
        gameStatusTxt.gameObject.SetActive(false);

        // 상태 변경
        status = GameStatus.Start;
    }

    void CheckGameOver()
    {
        if(player.playerHp <= 0)
        {
            // 텍스트 보이기
            gameStatusTxt.gameObject.SetActive(true);
            // 텍스트 변경
            gameStatusTxt.text = "Game Over";
            // 빨간색 텍스트
            gameStatusTxt.color = new Color(255, 0, 0, 255);

            // 게임 상태 변경
            status = GameStatus.GameOver;
        }
    }

    void Update()
    {
        CheckGameOver();
    }
}
