using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // �� �̸��� ���� InputField
    public TMP_InputField roomNameInput;

    // �÷��̾� ����
    public int maxPlayerNum = 5;

    public TMP_Text logText;

    // �κ� ���� �����.
    public void CreatRoom()
    {
        // inputFeild�� ������ ���� ��, ���� �ش� inputField�� �������� �����.
        if(roomNameInput.text != "")
        {
            PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, new RoomOptions { MaxPlayers = maxPlayerNum }, null);
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("�濡 �����.");
        logText.text = "Room Joined!";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("�� ���� ����.");
        logText.text = "Room Create Fail";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("�� ���� ����.");
        logText.text = "Room Joined Fail";
    }
}
