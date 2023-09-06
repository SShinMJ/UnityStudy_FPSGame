using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 목적 : 게임의 상태를 구별하고, 게임의 시작과 끝을 Text UI로 표현.
//       'Ready'상태에서 2초 후 'Start' 상태로 변경되며 게임 시작.
//       'Ready'상태에는 플레이어, 적 모두 움직일 수 없다.

// 목적2: player의 hp가 0보다 작거나 같으면, 'GameOver' 상태로 바꾸고, 텍스트를 보이게 한다.

// 목적3: 플레이어의 hp가 0 이하라면 애니메이션을 멈춘다.

// 목적4: Setting 버튼을 누르면 Option UI가 켜진다. 동시에 게임 속도를 조절한다.(0 or 1)
// 목적5: 게임 오버 시 Retry, Exit 버튼을 활성화.

// 목적6: 게임이 시작되면 Photon Network용 Player를 생성
// 목적7: Clent로서 접속이 되면(GameManager가 생성되면),
//        PhotonNetwork에 접속한 플레이어들을 확인해서 내 번호를 지정한다.
public class GameManager : MonoBehaviour
{
    // 싱글톤 선엉
    public static GameManager Instance;

    // 게임 상태 (4. 일시정지 추가)
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

    // hp가 들어있는 playerMove
    PlayerMovement player;

    Animator animator;

    // 4. OptionUI 게임 오브젝트
    public GameObject optionUI;

    // 6. Player prefab PhotonView
    public PhotonView playerPrefab;
    // 플레이어 수
    public int myPlayerNum = 0;

    // 게임 시작 시 활성화할 적 그룹
    public GameObject groupEnemies;

    // SpawnPoint들을 담는 배열
    public Transform[] spawnPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // 7. 플레이어 수 확인
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player p in players)
        {
            // LocalPlayer == 나 이므로 내가 아니라면
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
        // 방에 있는 상태임을 네트워크에서 확인하면
        yield return new WaitUntil(() => PhotonNetwork.InRoom);

        // 플레이어, 적 오브젝트 생성
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[myPlayerNum].position, Quaternion.identity);

        player = playerObj.GetComponent<PlayerMovement>();
        animator = player.GetComponent<Animator>();

        groupEnemies.SetActive(true);

        GameObject[] enemyCanvasList = GameObject.FindGameObjectsWithTag("Enemy Canvas");
        foreach(var canvas in enemyCanvasList)
        {
            canvas.GetComponent<HpBarTarget>().target = playerObj.transform;
        }

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
            // 애니메이션 멈추기
            animator.SetFloat("MoveMotion", 0f);

            // 텍스트 보이기
            gameStatusTxt.gameObject.SetActive(true);
            // 텍스트 변경
            gameStatusTxt.text = "Game Over";
            // 빨간색 텍스트
            gameStatusTxt.color = new Color(255, 0, 0, 255);

            // Retry, Exit 버튼 활성화
            GameObject retryBtn = gameStatusTxt.transform.GetChild(0).gameObject;
            retryBtn.SetActive(true);
            GameObject exitBtn = gameStatusTxt.transform.GetChild(1).gameObject;
            exitBtn.SetActive(true);

            // HP바와 Weapon Mode Text를 비활성화.
            player.hpSlider.gameObject.SetActive(false);
            player.GetComponent<PlayerFire>().weaponModeTxt.gameObject.SetActive(false);

            // 게임 상태 변경
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

    // 4. optionUI 켜기
    public void OpenOptionWindow()
    {
        // UI 창 띄우기
        optionUI.SetActive(true);

        // 게임 시간 흐르는 것 멈추기
        Time.timeScale = 0;

        // 멈춤 상태로 변경
        status = GameStatus.Pause;
    }
    // 계속하기(Continue)
    public void CloseOptionWindow()
    {
        optionUI.SetActive(false);

        Time.timeScale = 1;

        status = GameStatus.Start;
    }
    // 다시하기(Retry)
    public void RestartGame()
    {
        Time.timeScale = 1;

        // 현재 씬 번호를 다시 로드한다. GetActiveScene().buildIndex: 현재 살아있는 씬을 가져온다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // 끝내기(Exit)
    public void QuitGame()
    {
        Application.Quit();
    }
}
