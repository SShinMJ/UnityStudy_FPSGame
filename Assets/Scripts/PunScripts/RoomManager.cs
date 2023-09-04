using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� : ���� ������ �����ְ�, leave Room ��ư�� ������ ���� ���� �� �ִ�.

// ����2: Photon view��  ���� �÷��̾ �����Ѵ�.
public class RoomManager : MonoBehaviourPunCallbacks
{
    // �� ����
    public TMP_Text roomInfoText;

    public int lobbySceneNumber = 1;

    // Photon view �÷��̾�
    public PhotonView[] playerPrefab;

    public bool isConnect = false;

    int cnt = 0;

    private void Update()
    {
        ShowRoomInfo();

        if (cnt == 0 && Input.anyKey)
        {
            int ranNum = Random.Range(0, playerPrefab.Length);
            PhotonNetwork.Instantiate(playerPrefab[ranNum].name, Vector3.zero, Quaternion.identity);
            cnt++;
        }
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
