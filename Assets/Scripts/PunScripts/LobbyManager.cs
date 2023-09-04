using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // 방 이름을 넣을 InputField
    public TMP_InputField roomNameInput;

    // 플레이어 정원
    public int maxPlayerNum = 5;

    public TMP_Text logText;

    public string sceneName = "LoadingScene";

    public GameObject mainGameManager;

    // 로비에 방을 만든다.
    public void CreatRoom()
    {
        // inputFeild에 내용이 있을 때, 방을 해당 inputField의 내용으로 만든다.
        if(roomNameInput.text != "")
        {
            PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, new RoomOptions { MaxPlayers = maxPlayerNum }, null);
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("방에 입장됨.");
        logText.text = "Room Joined!";

        MainGameManager.Instance.maxHeadCount--;
        SceneManager.LoadScene(sceneName);
    }

    // 방을 만든 사람만(방장만) MainGameManager를 가진다.
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        // DontDestroyOnLoad : 다음씬으로 넘어가도 해당 오브젝트가 계속 유지된다.
        DontDestroyOnLoad(mainGameManager);
        mainGameManager.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("방 개설 실패.");
        logText.text = "Room Create Fail";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("방 입장 실패.");
        logText.text = "Room Joined Fail";
    }
}
