using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // 방 이름을 넣을 InputField
    public TMP_InputField roomNameInput;

    // 플레이어 정원
    public int maxPlayerNum = 5;

    public TMP_Text logText;

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
