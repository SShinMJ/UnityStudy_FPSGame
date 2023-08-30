using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// 목적 : 방의 정보를 보여주고, leave Room 버튼을 눌러서 방을 나갈 수 있다.
public class RoomManager : MonoBehaviourPunCallbacks
{
    // 방 정보
    public TMP_Text roomInfoText;

    public int lobbySceneNumber = 1;

    private void Update()
    {
        ShowRoomInfo();
    }

    // 방 정보 보여주기
    public void ShowRoomInfo()
    {
        // 방에 있다면
        if(PhotonNetwork.InRoom)
        {
            // 방 이름, 방 인원수, 최대 인원수 가져오기, 플레이어 이름
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

    // leave Room 버튼을 눌러서 방 나가기
    public void LeaveRoom()
    {
        // 방에 있다면
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
