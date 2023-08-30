using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� : ���� ������ �����ְ�, leave Room ��ư�� ������ ���� ���� �� �ִ�.
public class RoomManager : MonoBehaviourPunCallbacks
{
    // �� ����
    public TMP_Text roomInfoText;

    public int lobbySceneNumber = 1;

    private void Update()
    {
        ShowRoomInfo();
    }

    // �� ���� �����ֱ�
    public void ShowRoomInfo()
    {
        // �濡 �ִٸ�
        if(PhotonNetwork.InRoom)
        {
            // �� �̸�, �� �ο���, �ִ� �ο��� ��������, �÷��̾� �̸�
            string roomName = PhotonNetwork.CurrentRoom.Name;
            int roomHeadCnt = PhotonNetwork.CurrentRoom.PlayerCount;
            int roomMaxHeadCnt = PhotonNetwork.CurrentRoom.MaxPlayers;

            string playerNames = "< Player List >\n";
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerNames += (i+1) + "\t" + PhotonNetwork.PlayerList[i].NickName + "\n";
            }

            roomInfoText.text = string.Format("Room : {0}\t{1} / {2}\n\n{3}", roomName, roomHeadCnt, roomMaxHeadCnt, playerNames);
        }
        else
        {
            roomInfoText.text = "You are not IN Room";
        }
    }

    // leave Room ��ư�� ������ �� ������
    public void LeaveRoom()
    {
        // �濡 �ִٸ�
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();

            SceneManager.LoadScene(lobbySceneNumber);
        }
        else
        {
            roomInfoText.text = "You are not IN Room";
        }
    }
}
