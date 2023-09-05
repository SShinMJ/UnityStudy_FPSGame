using Photon.Pun;
using UnityEngine.SceneManagement;

// 목적 : Photon 게임 서버에 연결한다.
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // 1. Photon 게임 서버에 연결. 
        Connect();
    }

    // 한줄자리 코드는 람다식으로 표현 가능하다.
    public void Connect()
    {
        // AutomaticallySyncScene: 마스터 클라이언트와 일반 클라이언트들이 레벨을 동기화할지 결정
        // true로 설정하면 마스터 클라에서 LoadLevel()로 레벨을 변경하면 모든 클라이언트들이 자동으로 동일한 레벨을 로드.
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // PUN에 정의된 함수를 사용.(f12눌러 확인 가능)
    // 포톤 서버 연결시 호출된다.
    public override void OnConnected()
    {
        base.OnConnected();
/*        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("Callback - 연결됨.");*/
    }

    // 2. 마스터 서버 연결 시 호출된다.
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }

    // JoinLobby : Photon이 제공하는 함수.
    // 버튼 클릭 시 실행된다.
    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        // Lobby 씬으로 이동
        SceneManager.LoadScene("LobbyScene");
    }

    public void EnterSoloPlayeMode()
    {
        // Loading 씬으로 이동
        SceneManager.LoadScene("LoadingScene");
    }
}
